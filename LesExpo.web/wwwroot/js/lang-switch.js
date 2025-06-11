document.addEventListener('DOMContentLoaded', function() {
    // Get current path and language
    var currentPath = window.location.pathname;
    var currentLang = currentPath.startsWith('/en') ? 'en' : 'tr';
    var otherLang = currentLang === 'en' ? 'tr' : 'en';
    
    // Update language switcher links
    var langLinks = document.querySelectorAll('a[data-lang]');
    langLinks.forEach(function(link) {
        var linkLang = link.getAttribute('data-lang');
        var newHref = '';
        
        if (linkLang === 'tr') {
            if (currentPath === '/' || currentPath === '') {
                // Root homepage - stay as root for Turkish (default)
                newHref = '/';
            } else if (currentPath.startsWith('/en')) {
                // English page - convert to Turkish
                newHref = currentPath.replace('/en', '/tr');
            } else {
                // Already Turkish path or other
                newHref = currentPath;
            }
            link.href = newHref;
            link.classList.toggle('active', currentLang === 'tr');
        } else if (linkLang === 'en') {
            if (currentPath === '/' || currentPath === '') {
                // Root homepage - go to English
                newHref = '/en';
            } else if (currentPath.startsWith('/tr')) {
                // Turkish page - convert to English  
                newHref = currentPath.replace('/tr', '/en');
            } else {
                // Already English path or other
                newHref = currentPath;
            }
            link.href = newHref;
            link.classList.toggle('active', currentLang === 'en');
        }
    });
}); 