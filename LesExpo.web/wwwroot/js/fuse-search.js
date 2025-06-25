/**
 * LES-EXPO Fuse.js Search Engine
 * Provides fuzzy search functionality with real-time results
 */
class LesExpoFuseSearch {
    constructor() {
        this.searchData = [];
        this.fuse = null;
        this.currentLanguage = this.getCurrentLanguage();
        this.searchTimeout = null;
        this.isInitialized = false;
        
        // Initialize when DOM is ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.init());
        } else {
            this.init();
        }
    }

    getCurrentLanguage() {
        const path = window.location.pathname;
        const segments = path.split('/');
        return segments[1] === 'en' ? 'en' : 'tr';
    }

    async init() {
        try {
            this.showLoading(true);
            
            // Load basic search data first
            await this.loadSearchData();
            this.initializeFuse();
            this.bindEvents();
            this.isInitialized = true;
            
            // Check for URL query parameter and perform initial search
            const urlParams = new URLSearchParams(window.location.search);
            const initialQuery = urlParams.get('q') || '';
            
            const searchInput = document.getElementById('searchInput');
            if (searchInput && initialQuery) {
                searchInput.value = initialQuery;
            }
            
            // Perform initial search if there's a query
            if (initialQuery) {
                console.log('Performing initial search for:', initialQuery);
                await this.performSearch(initialQuery);
            } else {
                this.showRecentContent();
            }
            
        } catch (error) {
            console.error('Search initialization failed:', error);
            this.showError();
        } finally {
            this.showLoading(false);
        }
    }

    async loadSearchData(query = '') {
        try {
            const queryParam = query ? `?q=${encodeURIComponent(query)}` : '';
            const response = await fetch(`/${this.currentLanguage}/api/search-index${queryParam}`);
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: Failed to load search data`);
            }
            this.searchData = await response.json();
            console.log(`Loaded ${this.searchData.length} searchable items for ${this.currentLanguage}`);
        } catch (error) {
            console.error('Error loading search data:', error);
            throw error;
        }
    }

    initializeFuse() {
        const fuseOptions = {
            keys: [
                { name: 'title', weight: 0.4 },
                { name: 'content', weight: 0.3 },
                { name: 'summary', weight: 0.2 },
                { name: 'keywords', weight: 0.1 }
            ],
            threshold: 0.3,              // Fuzzy matching sensitivity (0 = exact, 1 = match anything)
            distance: 100,               // Maximum distance between characters
            minMatchCharLength: 2,       // Minimum characters to start matching
            includeScore: true,          // Include relevance score
            includeMatches: true,        // Include matched text positions
            ignoreLocation: true,        // Don't consider location of match
            useExtendedSearch: true      // Enable extended search syntax
        };

        this.fuse = new Fuse(this.searchData, fuseOptions);
        console.log('Fuse.js initialized with options:', fuseOptions);
    }

    bindEvents() {
        const searchInput = document.getElementById('searchInput');
        const searchSpinner = document.getElementById('searchSpinner');

        if (searchInput) {
            // Real-time search with debounce
            searchInput.addEventListener('input', (e) => {
                const query = e.target.value.trim();
                
                // Show/hide spinner
                if (searchSpinner) {
                    searchSpinner.style.display = query ? 'block' : 'none';
                }

                // Debounce search
                clearTimeout(this.searchTimeout);
                this.searchTimeout = setTimeout(() => {
                    this.performSearch(query);
                    if (searchSpinner) searchSpinner.style.display = 'none';
                }, 300);
            });

            // Handle Enter key
            searchInput.addEventListener('keypress', (e) => {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    clearTimeout(this.searchTimeout);
                    this.performSearch(e.target.value.trim());
                    if (searchSpinner) searchSpinner.style.display = 'none';
                }
            });
        }

        // Make suggestion search function globally available
        window.performSuggestionSearch = (term) => {
            if (searchInput) {
                searchInput.value = term;
                searchInput.focus();
                this.performSearch(term);
            }
        };
    }



    async performSearch(query = '') {
        if (!this.isInitialized) {
            console.warn('Search not initialized yet');
            return;
        }

        if (!query) {
            this.showRecentContent();
            return;
        }

        try {
            // Reload search data with contextual summaries for the query
            await this.loadSearchData(query);
            this.initializeFuse();

            // Perform fuzzy search
            let results = this.fuse.search(query);

            // Convert fuse results to display format
            const searchResults = results.map(result => ({
                ...result.item,
                score: result.score,
                matches: result.matches
            }));

            this.displayResults(searchResults, query);
            this.updateStats(searchResults.length, query);
            this.updateUrl(query);

        } catch (error) {
            console.error('Search failed:', error);
            this.showError();
        }
    }

    displayResults(results, query) {
        const resultsContainer = document.getElementById('searchResults');
        const noResultsState = document.getElementById('noResultsState');

        if (!resultsContainer) return;

        if (results.length === 0) {
            resultsContainer.style.display = 'none';
            if (noResultsState) noResultsState.style.display = 'block';
            return;
        }

        if (noResultsState) noResultsState.style.display = 'none';
        resultsContainer.style.display = 'block';

        const html = results.map(item => this.createResultCard(item, query)).join('');
        resultsContainer.innerHTML = html;

        // Add click tracking for analytics
        this.addResultClickTracking();
    }

    createResultCard(item, query) {
        const highlightedTitle = this.highlightMatches(item.title, query);
        const highlightedSummary = this.highlightMatches(item.summary, query);
        
        const readMoreText = this.currentLanguage === 'en' ? 'Read More' : 'Devamını Oku';

        return `
            <div class="search-result-card" data-score="${item.score || 0}">
                <div class="result-content">
                    <h3 class="result-title">
                        <a href="${item.url}" class="result-link">${highlightedTitle}</a>
                    </h3>
                    
                    <p class="result-summary">${highlightedSummary}</p>
                </div>
                
                <div class="result-actions">
                    <a href="${item.url}" class="result-button">
                        ${readMoreText} <i class="fas fa-arrow-right"></i>
                    </a>
                </div>
            </div>
        `;
    }

    highlightMatches(text, query) {
        if (!text || !query) return text;

        // Simple highlighting for now - can be enhanced with Fuse.js matches
        const regex = new RegExp(`(${this.escapeRegExp(query)})`, 'gi');
        return text.replace(regex, '<mark class="search-highlight">$1</mark>');
    }

    escapeRegExp(string) {
        return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    }

    showRecentContent() {
        // Show recent or featured content when no search query
        const featuredItems = this.searchData
            .filter(item => item.category !== 'Contact') // Exclude contact page
            .slice(0, 6);

        this.displayResults(featuredItems, '');
        
        const message = this.currentLanguage === 'en' 
            ? 'Featured content' 
            : 'Öne çıkan içerik';
        this.updateStats(featuredItems.length, message, false);
    }

    updateStats(count, query, isSearch = true) {
        const statsElement = document.getElementById('searchStats');
        if (!statsElement) return;

        let message;
        if (isSearch && query) {
            message = this.currentLanguage === 'en'
                ? `${count} results found for "${query}"`
                : `"${query}" için ${count} sonuç bulundu`;
        } else {
            message = query || (this.currentLanguage === 'en' 
                ? 'Ready to search' 
                : 'Arama yapmaya hazır');
        }

        statsElement.textContent = message;
    }

    updateUrl(query) {
        if (!query) return;

        const url = new URL(window.location);
        url.searchParams.set('q', query);
        window.history.replaceState({}, '', url);
    }

    showLoading(show) {
        const loadingState = document.getElementById('loadingState');
        const resultsContainer = document.getElementById('searchResults');
        const noResultsState = document.getElementById('noResultsState');
        const errorState = document.getElementById('errorState');

        if (show) {
            if (loadingState) loadingState.style.display = 'block';
            if (resultsContainer) resultsContainer.style.display = 'none';
            if (noResultsState) noResultsState.style.display = 'none';
            if (errorState) errorState.style.display = 'none';
        } else {
            if (loadingState) loadingState.style.display = 'none';
        }
    }

    showError() {
        const errorState = document.getElementById('errorState');
        const loadingState = document.getElementById('loadingState');
        const resultsContainer = document.getElementById('searchResults');
        const noResultsState = document.getElementById('noResultsState');

        if (errorState) errorState.style.display = 'block';
        if (loadingState) loadingState.style.display = 'none';
        if (resultsContainer) resultsContainer.style.display = 'none';
        if (noResultsState) noResultsState.style.display = 'none';
    }

    addResultClickTracking() {
        // Add analytics tracking for result clicks
        const resultLinks = document.querySelectorAll('.result-link');
        resultLinks.forEach(link => {
            link.addEventListener('click', (e) => {
                const title = e.target.textContent.trim();
                console.log(`Search result clicked: ${title}`);
                
                // You can add Google Analytics or other tracking here
                // Example: gtag('event', 'search_result_click', { 'search_term': query, 'result_title': title });
            });
        });
    }
}

// Initialize search when page loads
new LesExpoFuseSearch(); 