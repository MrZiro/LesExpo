// LES-EXPO Popup Logic with Cookie (cPanel uyumlu)
(function() {
  document.addEventListener('DOMContentLoaded', function() {
    var popup = document.getElementById('lesexpo-popup');
    var closeBtn = document.querySelector('.lesexpo-popup-close');
    var overlay = document.querySelector('.lesexpo-popup-overlay');
    // Dil kodunu belirle
    var lang = document.documentElement.lang || 'tr';
    var cookieKey = lang === 'en' ? '' : '';

    function getCookie(name) {
      var value = "; " + document.cookie;
      var parts = value.split("; " + name + "=");
      if (parts.length === 2) return parts.pop().split(";").shift();
    }

    function setCookie(name, value, days) {
      var expires = "";
      if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days*24*60*60*1000));
        expires = "; expires=" + date.toUTCString();
      }
      // Path'i her zaman kök dizin yap!
      document.cookie = name + "=" + (value || "")  + expires + "; path=/";
    }

    if (!popup) {
      console.error('LES-EXPO popup div bulunamadı!');
      return;
    }

    function showPopup() {
      popup.style.display = 'flex';
      document.body.style.overflow = 'hidden';
      setCookie(cookieKey, 'true', 1); // 1 gün boyunca tekrar gösterme
    }

    function hidePopup() {
      popup.style.display = 'none';
      document.body.style.overflow = '';
      setCookie(cookieKey, 'true', 1);
    }

    // Eğer çerez yoksa 4 saniye sonra göster
    if (!getCookie(cookieKey)) {
      setTimeout(showPopup, 4000);
    } else {
      // Debug için: console.log('Popup zaten gösterildi, tekrar açılmayacak.');
    }

    // Kapatma butonu
    if (closeBtn) {
      closeBtn.addEventListener('click', hidePopup);
    }
    // Overlay tıklanınca da kapansın
    if (overlay) {
      overlay.addEventListener('click', hidePopup);
    }
    // ESC ile kapama
    document.addEventListener('keydown', function(e) {
      if (e.key === 'Escape' && popup.style.display === 'flex') {
        hidePopup();
      }
    });
  });
})();