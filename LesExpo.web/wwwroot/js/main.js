/*=============== Navbar js =============================== */
document.addEventListener('DOMContentLoaded', () => {
  const menuToggleButtons = document.querySelectorAll('.nav-toggle-button'); // Menüyü açan/kapatan tüm butonlar/linkler
  const closeMenuButton = document.querySelector('.close-menu-btn');
  const fullscreenMenu = document.querySelector('.fullscreen-menu');
  const body = document.body;
  // Menü içindeki başlıklar (bunlara tıklanınca da kapanması için) - İsteğe bağlı
  const menuTitleLinks = document.querySelectorAll('.fullscreen-menu .menu-column h3');

  // Menüyü Açma Fonksiyonu
  const openMenu = () => {
      if (!fullscreenMenu.classList.contains('active')) {
          fullscreenMenu.classList.add('active');
          body.classList.add('menu-open'); // Scroll engelleme için body'e class ekle
      }
  };

  // Menüyü Kapatma Fonksiyonu
  const closeMenu = () => {
      if (fullscreenMenu.classList.contains('active')) {
          fullscreenMenu.classList.remove('active');
          body.classList.remove('menu-open'); // Scroll engellemeyi kaldır
      }
  };

  // Menüyü Aç/Kapat Butonlarına Tıklama Olayı
  menuToggleButtons.forEach(button => {
      button.addEventListener('click', (e) => {
          e.preventDefault(); // Linklerin (#) sayfayı yukarı atmasını engelle

          // Eğer tıklanan buton mobil toggle ise veya menü kapalıysa, menünün durumunu değiştir.
          // Eğer menü zaten açıksa ve tıklanan normal bir menü linkiyse, sadece kapat.
          if (button.classList.contains('mobile-menu-toggle')) {
               if (fullscreenMenu.classList.contains('active')) {
                  closeMenu();
               } else {
                  openMenu();
               }
          } else if (button.closest('.fullscreen-menu') && fullscreenMenu.classList.contains('active')){
               // Eğer açık menü içindeki bir başlığa tıklandıysa kapat
               closeMenu();
          }
          else {
              // Diğer tüm durumlarda (ana navbardaki linkler, arama ikonu) menüyü aç
              openMenu();
          }
      });
  });

  // Kapatma (X) Butonuna Tıklama Olayı
  if (closeMenuButton) {
      closeMenuButton.addEventListener('click', () => {
          closeMenu();
      });
  }

  // İsteğe Bağlı: Esc tuşu ile menüyü kapatma
  document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && fullscreenMenu.classList.contains('active')) {
          closeMenu();
      }
  });
});

      /* Navbar Fixed */
      window.addEventListener('scroll', function () {
        const navbar = document.querySelector('.main-header');
        if (window.scrollY > 50) {
          navbar.classList.add('scrolled');
        } else {
          navbar.classList.remove('scrolled');
        }
      });


/*============= Haberler Card Slider JS ================= */
document.addEventListener("DOMContentLoaded", () => {
  const slider = document.getElementById("slider-haberler")
  const prevBtn = document.querySelector(".prev-haberler")
  const nextBtn = document.querySelector(".next-haberler")

  // Variables for drag functionality
  let isDragging = false
  let startPos = 0
  let currentTranslate = 0
  let prevTranslate = 0
  let animationID = 0
  let currentIndex = 0

  // Calculate card width (including margins)
  function getCardWidth() {
    const card = document.querySelector(".card-haberler")
    const cardStyle = window.getComputedStyle(card)
    const cardWidth = card.offsetWidth
    const marginRight = Number.parseInt(cardStyle.marginRight)
    const marginLeft = Number.parseInt(cardStyle.marginLeft)
    return cardWidth + marginRight + marginLeft
  }

  // Get number of visible cards based on screen width
  function getVisibleCards() {
    const windowWidth = window.innerWidth
    if (windowWidth < 576) return 1
    if (windowWidth < 768) return 2
    if (windowWidth < 992) return 3
    return 4
  }

  // Calculate total number of cards
  const totalCards = document.querySelectorAll(".card-haberler").length

  // Button click events
  prevBtn.addEventListener("click", () => {
    if (currentIndex > 0) {
      currentIndex--
      updateSliderPosition()
    }
  })

  nextBtn.addEventListener("click", () => {
    const visibleCards = getVisibleCards()
    if (currentIndex < totalCards - visibleCards) {
      currentIndex++
      updateSliderPosition()
    }
  })

  // Update slider position based on current index
  function updateSliderPosition() {
    currentTranslate = -currentIndex * getCardWidth()
    prevTranslate = currentTranslate
    setSliderPosition()
  }

  // Set the slider position with transform
  function setSliderPosition() {
    slider.style.transform = `translateX(${currentTranslate}px)`
  }

  // Mouse and Touch Events for drag functionality
  slider.addEventListener("mousedown", dragStart)
  slider.addEventListener("touchstart", dragStart)
  slider.addEventListener("mouseup", dragEnd)
  slider.addEventListener("touchend", dragEnd)
  slider.addEventListener("mouseleave", dragEnd)
  slider.addEventListener("mousemove", drag)
  slider.addEventListener("touchmove", drag)

  // Prevent context menu on long press
  slider.addEventListener("contextmenu", (e) => e.preventDefault())

  function dragStart(event) {
    // Get the correct position whether it's a touch or mouse event
    startPos = getPositionX(event)
    isDragging = true
    animationID = requestAnimationFrame(animation)
    slider.classList.add("grabbing")
  }

  function drag(event) {
    if (isDragging) {
      const currentPosition = getPositionX(event)
      currentTranslate = prevTranslate + currentPosition - startPos
    }
  }

  function dragEnd() {
    isDragging = false
    cancelAnimationFrame(animationID)
    slider.classList.remove("grabbing")

    // Snap to closest card
    const movedBy = currentTranslate - prevTranslate

    // If moved enough to change card
    if (Math.abs(movedBy) > getCardWidth() * 0.3) {
      if (movedBy < 0) {
        // Moved right
        const visibleCards = getVisibleCards()
        if (currentIndex < totalCards - visibleCards) {
          currentIndex++
        }
      } else {
        // Moved left
        if (currentIndex > 0) {
          currentIndex--
        }
      }
    }

    // Update position to snap to grid
    currentTranslate = -currentIndex * getCardWidth()
    prevTranslate = currentTranslate

    setSliderPosition()
  }

  function animation() {
    setSliderPosition()
    if (isDragging) requestAnimationFrame(animation)
  }

  function getPositionX(event) {
    return event.type.includes("mouse") ? event.pageX : event.touches[0].clientX
  }

  // Handle window resize
  window.addEventListener("resize", () => {
    // Recalculate and update slider position when window is resized
    updateSliderPosition()
  })

  // Initialize slider position
  updateSliderPosition()

  // Add subtle parallax effect to cards
  document.querySelectorAll(".card-haberler").forEach((card) => {
    card.addEventListener("mousemove", (e) => {
      const rect = card.getBoundingClientRect()
      const x = e.clientX - rect.left
      const y = e.clientY - rect.top

      const cardImage = card.querySelector(".card-image-haberler img")
      const moveX = (x - rect.width / 2) / 20
      const moveY = (y - rect.height / 2) / 20

      cardImage.style.transform = `scale(1.08) translate(${moveX}px, ${moveY}px)`
    })

    card.addEventListener("mouseleave", () => {
      const cardImage = card.querySelector(".card-image img")
      cardImage.style.transform = "scale(1)"
    })
  })
})


/*====== index.html js end =============*/

/*====== iletisim.html js start ====*/
// Wrap in DOMContentLoaded if this script might run before the form exists
document.addEventListener('DOMContentLoaded', () => {
    const contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', function(e) {
            e.preventDefault();

            // CAPTCHA doğrulama
            const captchaTextEl = document.getElementById('captchaText');
            const captchaInputEl = document.getElementById('captchaInput');
            if (!captchaTextEl || !captchaInputEl) {
                console.error("Captcha elements not found");
                alert('CAPTCHA elements not found!');
                return;
            }
            const captchaText = captchaTextEl.textContent;
            const captchaInput = captchaInputEl.value;


            if (captchaText !== captchaInput) {
                alert('CAPTCHA kodu hatalı!');
                return;
            }

            // Form verilerini al
            const name = document.getElementById('name').value;
            const phone = document.getElementById('phone').value;
            const email = document.getElementById('email').value;
            const message = document.getElementById('message').value;

            // Normalde burada bir AJAX isteği ile form verileri sunucuya gönderilir
            console.log('Form gönderildi:', { name, phone, email, message });
            alert('Mesajınız başarıyla gönderildi!');

            // Formu sıfırla
            this.reset();
             // Optionally refresh captcha after successful submission
             refreshCaptcha();
        });
    }

     // CAPTCHA yenileme
     const refreshButton = document.querySelector('.captcha-refresh');
     if (refreshButton) {
         refreshButton.addEventListener('click', refreshCaptcha);
         // Initial captcha generation
         refreshCaptcha();
     }

     function refreshCaptcha() {
        const captchaTextEl = document.getElementById('captchaText');
        if(!captchaTextEl) return;
        // Rastgele CAPTCHA kodu oluştur
        const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        let captcha = '';
        for (let i = 0; i < 5; i++) {
            captcha += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        captchaTextEl.textContent = captcha;
        // Clear previous input
        const captchaInputEl = document.getElementById('captchaInput');
        if(captchaInputEl) captchaInputEl.value = '';
     }
});


