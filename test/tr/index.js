/* Gallery Section */

document.addEventListener('DOMContentLoaded', function () {
  // Elementleri seç
  const galleryWrapper = document.getElementById('galleryWrapper');
  const prevBtn = document.getElementById('prevBtn');
  const nextBtn = document.getElementById('nextBtn');

  // Kaydırma miktarı - bir kart genişliği kadar
  const scrollAmount = 300; // Kart genişliği

  // İleri butonu tıklandığında
  nextBtn.addEventListener('click', function () {
    galleryWrapper.scrollBy({
      left: scrollAmount,
      behavior: 'smooth'
    });
  });

  // Geri butonu tıklandığında
  prevBtn.addEventListener('click', function () {
    galleryWrapper.scrollBy({
      left: -scrollAmount,
      behavior: 'smooth'
    });
  });
});

/*========================================================Gallery End====================================================================================================*/

/* Countdown */
const targetDate = new Date('2025-09-09T10:00:00');

function updateCountdown() {
  const now = new Date();
  const diff = targetDate - now;

  if (diff <= 0) {
    document.querySelectorAll(".time-value").forEach(el => el.textContent = "00");
    clearInterval(interval);
    return;
  }

  const days = Math.floor(diff / (1000 * 60 * 60 * 24));
  const hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
  const minutes = Math.floor((diff / 1000 / 60) % 60);
  const seconds = Math.floor((diff / 1000) % 60);

  document.getElementById("days").textContent = String(days).padStart(2, '0');
  document.getElementById("hours").textContent = String(hours).padStart(2, '0');
  document.getElementById("minutes").textContent = String(minutes).padStart(2, '0');
  document.getElementById("seconds").textContent = String(seconds).padStart(2, '0');
}

const interval = setInterval(updateCountdown, 1000);
updateCountdown();


/* splash screen */
    document.addEventListener('DOMContentLoaded', function() {
            const splash = document.getElementById('splash');
            const mainContent = document.getElementById('main-content');
            
            // Check if user has seen splash screen in this session
            const hasSeenSplashThisSession = sessionStorage.getItem('lesExpoSplashSeen');
            
            if (hasSeenSplashThisSession) {
                // User has seen splash in this session (navigating within site), skip splash screen
                splash.style.display = 'none';
                mainContent.style.display = 'block';
                document.body.style.overflow = 'auto'; // Re-enable scrolling
            } else {
                // New session (new tab/window or first visit), show splash screen
                setTimeout(function() {
                    // Mark that user has seen splash in this session
                    sessionStorage.setItem('lesExpoSplashSeen', 'true');
                    
                    // Fade out splash screen
                    splash.style.opacity = '0';
                    
                    // After transition completes, hide splash and show main content
                    setTimeout(function() {
                        splash.style.display = 'none';
                        mainContent.style.display = 'block';
                        document.body.style.overflow = 'auto'; // Re-enable scrolling
                    }, 300);
                }, 2000); // Exactly 2 seconds as requested
            }
        });


/* Section-1 sc */

document.addEventListener("DOMContentLoaded", () => {
  // Get all slider elements
  const slides = document.querySelectorAll(".slide-sc")
  const dots = document.querySelectorAll(".dot-sc")
  const prevArrow = document.querySelector(".prev-arrow-sc")
  const nextArrow = document.querySelector(".next-arrow-sc")
  const sliderContainer = document.querySelector(".slider-container-sc")
  const controlsContainer = document.querySelector(".controls-container-sc")

  let currentSlide = 0
  const totalSlides = slides.length
  let autoPlayInterval
  const autoPlayDuration = 5000 // 5 seconds
  let isAutoPlaying = false

  // Preload all images to prevent layout shifts
  function preloadImages() {
    slides.forEach((slide) => {
      const img = slide.querySelector("img")
      if (img) {
        const src = img.getAttribute("src")
        if (src) {
          const preloadImg = new Image()
          preloadImg.src = src
        }
      }
    })
  }

  // Initialize slider
  function initSlider() {
    // Preload images
    preloadImages()

    // Set first slide as active
    slides[0].classList.add("active-sc")
    dots[0].classList.add("active-sc")

    // Set up event listeners
    prevArrow.addEventListener("click", () => {
      pauseAutoPlay()
      showPreviousSlide()
    })

    nextArrow.addEventListener("click", () => {
      pauseAutoPlay()
      showNextSlide()
    })

    // Add click event to pagination dots
    dots.forEach((dot) => {
      dot.addEventListener("click", function () {
        pauseAutoPlay()
        const slideIndex = Number.parseInt(this.getAttribute("data-index"))
        goToSlide(slideIndex)
      })
    })

    // Handle video playback
    slides.forEach((slide) => {
      const iframe = slide.querySelector("iframe")
      if (iframe) {
        // When a slide with video becomes active
        const observer = new MutationObserver((mutations) => {
          mutations.forEach((mutation) => {
            if (mutation.attributeName === "class") {
              if (slide.classList.contains("active-sc")) {
                // Reset iframe src to restart video when slide becomes active
                const src = iframe.getAttribute("src")
                iframe.setAttribute("src", src)
              }
            }
          })
        })

        observer.observe(slide, { attributes: true })
      }
    })

    // Start autoplay
    startAutoPlay()

    // Pause autoplay on hover
    sliderContainer.addEventListener("mouseenter", pauseAutoPlay)
    controlsContainer.addEventListener("mouseenter", pauseAutoPlay)

    // Resume autoplay on mouse leave
    sliderContainer.addEventListener("mouseleave", startAutoPlay)
    controlsContainer.addEventListener("mouseleave", startAutoPlay)
  }

  // Update slide classes for animation
  function updateSlideClasses() {
    // Remove all classes first
    slides.forEach((slide) => {
      slide.classList.remove("active-sc", "prev-sc", "next-sc")
    })

    // Add active class to current slide
    slides[currentSlide].classList.add("active-sc")

    // Add prev class to previous slide
    const prevSlide = currentSlide - 1 < 0 ? totalSlides - 1 : currentSlide - 1
    slides[prevSlide].classList.add("prev-sc")

    // Add next class to next slide
    const nextSlide = (currentSlide + 1) % totalSlides
    slides[nextSlide].classList.add("next-sc")
  }

  // Go to specific slide
  function goToSlide(index) {
    if (index < 0) {
      index = totalSlides - 1
    } else if (index >= totalSlides) {
      index = 0
    }

    // Remove active class from current dot
    dots[currentSlide].classList.remove("active-sc")

    // Set new current slide
    currentSlide = index

    // Update slide classes for animation
    updateSlideClasses()

    // Add active class to new current dot
    dots[currentSlide].classList.add("active-sc")
  }

  // Show previous slide
  function showPreviousSlide() {
    goToSlide(currentSlide - 1)
  }

  // Show next slide
  function showNextSlide() {
    goToSlide(currentSlide + 1)
  }

  // Start auto play
  function startAutoPlay() {
    if (!isAutoPlaying) {
      isAutoPlaying = true

      autoPlayInterval = setInterval(() => {
        showNextSlide()
      }, autoPlayDuration)
    }
  }

  // Pause auto play
  function pauseAutoPlay() {
    if (isAutoPlaying) {
      isAutoPlaying = false
      clearInterval(autoPlayInterval)
    }
  }

  // Handle keyboard navigation
  document.addEventListener("keydown", (e) => {
    if (e.key === "ArrowLeft") {
      pauseAutoPlay()
      showPreviousSlide()
    } else if (e.key === "ArrowRight") {
      pauseAutoPlay()
      showNextSlide()
    }
  })

  // Handle swipe gestures for mobile
  let touchStartX = 0
  let touchEndX = 0

  sliderContainer.addEventListener(
    "touchstart",
    (e) => {
      pauseAutoPlay()
      touchStartX = e.changedTouches[0].screenX
    },
    false,
  )

  sliderContainer.addEventListener(
    "touchend",
    (e) => {
      touchEndX = e.changedTouches[0].screenX
      handleSwipe()
    },
    false,
  )

  function handleSwipe() {
    const swipeThreshold = 50 // Minimum distance required for swipe

    if (touchEndX < touchStartX - swipeThreshold) {
      // Swipe left, show next slide
      showNextSlide()
    } else if (touchEndX > touchStartX + swipeThreshold) {
      // Swipe right, show previous slide
      showPreviousSlide()
    }
  }

  // Initialize the slider
  initSlider()
})


