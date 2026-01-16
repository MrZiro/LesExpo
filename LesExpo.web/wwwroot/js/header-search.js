/**
 * Simple Header Search Integration
 * Redirects header search to our search pages
 */
document.addEventListener('DOMContentLoaded', function() {
    
    // Get current language from URL
    function getCurrentLanguage() {
        const path = window.location.pathname;
        const segments = path.split('/');
        return segments[1] === 'en' ? 'en' : 'tr';
    }

    // Perform search redirection
    function performHeaderSearch(query) {
        const language = getCurrentLanguage();
        const searchUrl = language === 'en' 
            ? `/en/search?q=${encodeURIComponent(query)}`
            : `/tr/arama?q=${encodeURIComponent(query)}`;
        
        window.location.href = searchUrl;
    }

    // Bind desktop header search
    const headerSearchInput = document.querySelector('.header-search-input');
    const headerSearchButton = document.querySelector('.header-search-button');

    if (headerSearchInput && headerSearchButton) {
        // Handle search button click
        headerSearchButton.addEventListener('click', function(e) {
            e.preventDefault();
            const query = headerSearchInput.value.trim();
            if (query) {
                performHeaderSearch(query);
            }
        });

        // Handle Enter key in search input
        headerSearchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                const query = e.target.value.trim();
                if (query) {
                    performHeaderSearch(query);
                }
            }
        });
    }

    // Bind mobile search toggle button (redirect to search page)
    const mobileSearchToggle = document.querySelector('.search-toggle-button');
    if (mobileSearchToggle) {
        mobileSearchToggle.addEventListener('click', function(e) {
            e.preventDefault();
            const language = getCurrentLanguage();
            const searchUrl = language === 'en' ? '/en/search' : '/tr/arama';
            window.location.href = searchUrl;
        });
    }
}); 