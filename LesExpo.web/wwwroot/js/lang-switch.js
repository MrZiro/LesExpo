document.addEventListener('DOMContentLoaded', function() {
    // Get current path and language
    var currentPath = window.location.pathname;
    var currentLang = currentPath.startsWith('/en') ? 'en' : 'tr';
    var otherLang = currentLang === 'en' ? 'tr' : 'en';
    
    // Update language switcher links
    var langLinks = document.querySelectorAll('a[data-lang]');
    langLinks.forEach(function(link) {
        var linkLang = link.getAttribute('data-lang');
        
        if (linkLang === 'tr') {
            link.href = currentPath.replace('/en', '/tr');
            link.classList.toggle('active', currentLang === 'tr');
        } else if (linkLang === 'en') {
            link.href = currentPath.replace('/tr', '/en');
            link.classList.toggle('active', currentLang === 'en');
        }
    });
}); 