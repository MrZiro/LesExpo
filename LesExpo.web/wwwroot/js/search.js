document.addEventListener('DOMContentLoaded', function() {
    // Arama inputlarını seç
    const headerSearchInput = document.querySelector('.header-search-input');
    const searchResults = document.getElementById('search-results');
    const searchTermSpan = document.getElementById('search-term');
    const resultCountSpan = document.getElementById('result-count');

    // Arama fonksiyonu
    function performSearch(searchTerm) {
        // URL'den arama terimini al
        const urlParams = new URLSearchParams(window.location.search);
        const query = searchTerm || urlParams.get('q');

        if (!query) return;

        // Arama terimini göster
        searchTermSpan.textContent = query;

        // Tüm sayfaları içeren bir dizi
        const pages = [
            { title: 'Ana Sayfa', url: 'index.html', content: 'LES-EXPO Yük Mühendisliği Hizmetleri Fuarı' },
            { title: 'Hakkımızda', url: 'hakkimizda.html', content: 'LES-EXPO hakkında detaylı bilgi' },
            { title: 'Fuar Künyesi', url: 'fuar-kunyesi.html', content: 'Fuar künyesi ve detayları' },
            { title: 'Fuar Alanı', url: 'fuar-alani.html', content: 'Fuar alanı bilgileri' },
            { title: 'Ulaşım', url: 'ulasim.html', content: 'Fuar alanına ulaşım bilgileri' },
            { title: 'Katılımcı Profili', url: 'katilimci-profili.html', content: 'Katılımcı profili ve detayları' },
            { title: 'Ziyaretçi Profili', url: 'ziyaretci-profili.html', content: 'Ziyaretçi profili ve detayları' },
            { title: 'İletişim', url: 'iletisim.html', content: 'İletişim bilgileri' }
        ];

        // Arama sonuçlarını filtrele
        const results = pages.filter(page => {
            const searchContent = (page.title + ' ' + page.content).toLowerCase();
            return searchContent.includes(query.toLowerCase());
        });

        // Sonuç sayısını güncelle
        resultCountSpan.textContent = results.length;

        // Sonuçları göster
        if (results.length > 0) {
            searchResults.innerHTML = results.map(result => `
                <div class="search-result-item">
                    <h3>${result.title}</h3>
                    <p>${result.content}</p>
                    <a href="${result.url}" class="result-link">
                        Sayfaya Git <i class="fas fa-arrow-right"></i>
                    </a>
                </div>
            `).join('');
        } else {
            searchResults.innerHTML = `
                <div class="no-results">
                    <i class="fas fa-search"></i>
                    <h3>Sonuç Bulunamadı</h3>
                    <p>Aramanızla eşleşen sonuç bulunamadı. Lütfen farklı anahtar kelimelerle tekrar deneyin.</p>
                </div>
            `;
        }
    }

    // Header arama inputu için event listener
    if (headerSearchInput) {
        headerSearchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                const searchTerm = this.value.trim();
                if (searchTerm) {
                    window.location.href = `arama-sonucları.html?q=${encodeURIComponent(searchTerm)}`;
                }
            }
        });
    }

    // Sayfa yüklendiğinde arama yap
    if (window.location.pathname.includes('arama-sonucları.html')) {
        performSearch();
    }
}); 