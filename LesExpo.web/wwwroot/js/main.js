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
      } else if (button.closest('.fullscreen-menu') && fullscreenMenu.classList.contains('active')) {
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
    contactForm.addEventListener('submit', function (e) {
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
    if (!captchaTextEl) return;
    // Rastgele CAPTCHA kodu oluştur
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let captcha = '';
    for (let i = 0; i < 5; i++) {
      captcha += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    captchaTextEl.textContent = captcha;
    // Clear previous input
    const captchaInputEl = document.getElementById('captchaInput');
    if (captchaInputEl) captchaInputEl.value = '';
  }
});


/* Search Functionality */
document.addEventListener('DOMContentLoaded', function () {
  // Arama butonunu seç
  const searchToggleButton = document.querySelector('.search-toggle-button');
  const searchOverlay = document.querySelector('.search-overlay');
  const searchClose = document.querySelector('.search-close');
  const searchInput = document.querySelector('.search-input');

  // Arama butonuna tıklandığında
  searchToggleButton.addEventListener('click', function () {
    searchOverlay.classList.add('active');
    searchInput.focus();
  });

  // Kapatma butonuna tıklandığında
  searchClose.addEventListener('click', function () {
    searchOverlay.classList.remove('active');
  });

  // ESC tuşuna basıldığında
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && searchOverlay.classList.contains('active')) {
      searchOverlay.classList.remove('active');
    }
  });

  // Arama çubuğuna tıklandığında overlay'i kapatma
  searchOverlay.addEventListener('click', function (e) {
    if (e.target === searchOverlay) {
      searchOverlay.classList.remove('active');
    }
  });
});


// ================================== Footer KVKK KISMI ==================================
// Privacy Modal Functionality
document.addEventListener('DOMContentLoaded', function () {
  const modal = document.getElementById('privacyModal');
  const modalTitle = document.getElementById('modalTitle');
  const modalContent = document.getElementById('modalContent');
  const closeBtn = document.querySelector('.privacy-modal-close');
  const privacyLinks = document.querySelectorAll('.privacy-link');

  // KVKK and Privacy Policy content
  const modalContents = {
    kvkk: {
      title: 'KVKK Aydınlatma Metni',
      content: `
        <h3>6698 SAYILI KİŞİSEL VERİLERİN KORUNMASI KANUNU GEREĞİNCE BİLGİLENDİRME</h3>
        <p><strong>Veri sorumlusunun ve varsa temsilcisinin kimliği;</strong><br>
        Bu bilgilendirme, 6698 sayılı Kişisel Verilerin Korunması Kanunu ("KVKK") kapsamında, Veri
        Sorumlusu sıfatıyla, "KFA Fuarcılık A.Ş" (Mersis No:0548143315700001)'nin iş ortaklarına,
        müşterilerine, fuar ziyaretçilerine, iletişimde bulunduğu gerçek ya da tüzel kişilere aydınlatma
        yükümlülüğünü yerine getirmek üzere yapılmaktadır.</p>

        <p><strong>Kişisel verilerin hangi amaçla işleneceği;</strong><br>
        Kişisel verileriniz aşağıdaki durum ve koşullarda veri sorumlusu ya da atayacağı tüzel/gerçek kişiler
        tarafından işlenebilmektedir;</p>
        <ul>
          <li>Ürün ve hizmetlerimizi iyileştirmek, geliştirmek, çeşitlendirmek ve ticari ilişki içerisinde
        olduğumuz tüzel/gerçek kişilere sunabilmek ya da bilgi vermek amacıyla,</li>
          <li>Fuarlarımızda ürün ve hizmetlerini sergileyen katılımcılarımız ile bu ürün ve hizmetleri
        görmeye gelen ziyaretçilerimizin, bu organizasyonlardan sağlayacağı faydayı ve niteliği
        arttırmak amacıyla,</li>
          <li>Fuar kapsamlarının katılımcı, ziyaretçi beklentilerine uygun şekilde planlanması, etkinliğinin
        izlenmesi ve sürekliliğinin sağlanması amacıyla,</li>
          <li>Fuarların katılımcı, ziyaretçi, basın, ilgili sivil toplum kuruluşları ve ilgili kamu kurum ve
        kuruluşlarınca bilinirliğinin arttırılması, aralarında iş ortaklıklarının oluşturulması ve ticari ya da
        sosyal faaliyetlerinin zenginleştirilmesi amacıyla,</li>
          <li>Geliştirilen yeni ürün ve hizmetlerin değerlendirilmesi ve şirketimizin ticari ya da iş
        stratejilerinin belirlenmesi ve uygulanması amacıyla,</li>
          <li>Çalışanlarımızın verilerini, performans düzeyini ve çalışan memnuniyetini arttırmak, eğitim ve
        kariyer planlarının belirlenmesi veya iş güvenliğinin sağlanması amacıyla,</li>
          <li>Bunlarla birlikte gerekli kalite ve standart denetimlerimizi yapabilmek ya da kanun ve
        yönetmelikler ile belirlenmiş raporlama ve sair yükümlülüklerimizin yerine getirilmesi gibi
        amaçlar için,</li>
          <li>Ayrıca, düzenleyici ve denetleyici kurumlarla, yasal düzenlemelerin gerektirdiği veya zorunlu
        kıldığı şekilde, KVKK'da belirtilmiş yasal yükümlülüklerin yerine getirilmesini sağlamak üzere
        belirlenmiş gereklilik ve zorunluluklar kapsamında.</li>
        </ul>

        <p><strong>İşlenen kişisel verilerin kimlere ve hangi amaçla aktarılabileceği,</strong><br>
        İşlenen kişisel verileriniz yukarıdaki amaçlar ve KVKK'nın belirlediği koşullar içerisinde;</p>
        <ul>
          <li>Ticari faaliyetlerin yerine getirilebilmesi ve sürekliliğinin sağlanabilmesi amacıyla, şirketimiz iş
        ortaklıklarına ya da iştiraklerimize,</li>
          <li>Dış kaynaklı ürün ve hizmetlerin sağlanabilmesi amacıyla sınırlı olarak tedarikçilerimize,</li>
          <li>İlgili mevzuat hükümlerine göre ticari faaliyetlerimizin denetimi amacıyla ilgili sözleşmeler
        kapsamında denetim firmalarına,</li>
          <li>Şirketimizin ticari faaliyetlerine ilişkin stratejilerin tasarlanması amacıyla sınırlı olarak
        hissedarlarımıza,</li>
          <li>İlgili kamu kurum ve kuruluşlarının hukuki yetkisi dâhilinde talepleri kapsamında sınırlı olarak
        hukuken yetkili kamu kurum ve kuruluşlarına,</li>
          <li>KVKK'nda belirtilmiş yasal yükümlülüklerin yerine getirilmesini sağlamak üzere belirlenmiş
        kurum ve kuruluşlarına,</li>
        </ul>
        <p>açık rızanız ya da kanun ile belirlenmiş koşullar çerçevesinde aktarılabilecektir.</p>

        <p><strong>Kişisel veri toplamanın yöntemi ve hukuki sebebi,</strong><br>
        Kişisel verileriniz, şirketimiz veya şirketimiz adına veri işleyen gerçek ya da tüzel kişiler tarafından;
        internet sitesi, muhtelif sözleşmeler, her türlü ilgi bilgi formları, anketler, iş başvuru formları, iş
        sözleşmeleri, sosyal medya uygulamaları, çağrı merkezleri ve burada sayılanlarla sınırlı olmamak
        üzere sözlü, yazılı veya elektronik kanallar aracılığı ile açık rızanız ile toplanmaktadır.</p>

        <p>Bu bilgiler, ticari ve idari faaliyetlerimizin, iş ve sosyal hayatı düzenleyen yasalar çerçevesinde
        sunulabilmesi ve bu kapsamda şirketimizin ticari hayatını sürdürebilmesi, yasalardan doğan
        mesuliyetlerini eksiksiz ve doğru bir şekilde yerine getirebilmesi gayesi ile edinilir.</p>

        <p><strong>Veri Sahibinin Hakları,</strong><br>
        KVKK kapsamında, kişisel verilerinize ilişkin olarak aşağıdaki haklara sahipsiniz:</p>
        <ul>
          <li>Kişisel veri işlenip işlenmediğini öğrenme,</li>
          <li>Kişisel verileri işlenmişse buna ilişkin bilgi talep etme,</li>
          <li>Kişisel verilerin işlenme amacını ve bunların amacına uygun kullanılıp kullanılmadığını
        öğrenme,</li>
          <li>Yurt içinde veya yurt dışında kişisel verilerin aktarıldığı üçüncü kişileri bilme,</li>
          <li>Kişisel verilerin eksik veya yanlış işlenmiş olması hâlinde bunların düzeltilmesini isteme,</li>
          <li>Kişisel verinizin işlenmesini gerektiren sebeplerin ortadan kalkması hâlinde, verilerin
        silinmesini veya yok edilmesini isteme,</li>
          <li>Talebinizce düzeltilen ya da silinen bilgilerinizin, eğer aktarılmış ise kişisel verilerin aktarıldığı
        üçüncü kişilere bildirilmesini isteme,</li>
          <li>İşlenen verilerin münhasıran otomatik sistemler vasıtasıyla analiz edilmesi suretiyle kişinin
        kendisi aleyhine bir sonucun ortaya çıkmasına itiraz etme,</li>
          <li>Kişisel verilerin kanuna aykırı olarak işlenmesi sebebiyle zarara uğraması hâlinde zararın
        giderilmesini talep etme.</li>
        </ul>

        <p>Yukarıda belirtilen haklarınızı kullanmak için kimliğinizi tespit edici gerekli bilgiler ve kullanmak
        istediğiniz hakkınıza yönelik açıklamalarınızla birlikte yazılı talebinizi "[…]" adresine ıslak imzalı olarak
        ve/veya […] kayıtlı elektronik posta adresimize güvenli elektronik imza ile imzalanmış olarak
        gönderebilirsiniz.</p>

        <p>Kişisel veri sahibi olarak sahip olduğunuz ve yukarıda belirtilen haklarınızı kullanmak için yapacağınız
        ve kullanmayı talep ettiğiniz hakka ilişkin açıklamalarınızı içeren başvuruda; talep ettiğiniz hususun
        açık ve anlaşılır olması, talep ettiğiniz konunun şahsınız ile ilgili olması veya başkası adına hareket
        ediyor iseniz bu konuda özel olarak yetkili olmanız ve yetkinizi belgelendirilmesi, başvurunun kimlik ve
        adres bilgilerini içermesi ve başvuruya kimliğinizi tevsik edici belgelerin eklenmesi gerekmektedir.</p>

        <p>Bu kapsamda yapacağınız başvurular mümkün olan en kısa zaman diliminde ve en çok 30 gün
        içerisinde sonuçlandırılacaktır.</p>
      `
    },
    privacy: {
      title: 'Gizlilik Politikası',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE WEB SİTESİ KULLANIM ŞARTLARI</h3>
        <p>Lütfen bu web sitesini kullanmadan önce aşağıdaki maddeleri dikkatlice okuyunuz ve inceleyiniz.</p>
        <p>https://les-expo.com alan adına sahip web sitesi ("Site" veya "Web Sitesi") KFA Fuarcılık A.Ş'ye ("KFA") aittir.</p>
        <p>Bu Web Sitesini kullanarak, aşağıdaki Kullanıcı Sözleşmesini kabul etmiş olursunuz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret ederek, bu Gizlilik Politikası ve Kullanım Şartlarında ("Sözleşme") belirtilen şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul edersiniz.</p>
        <p>KFA, bu web sitesindeki Gizlilik Politikası dahil olmak üzere bu koşulları ve diğer bilgileri gerekli gördüğünde zaman zaman ve herhangi bir bildirimde bulunmaksızın değiştirme hakkını saklı tutar. Bu Sözleşmenin ve Gizlilik Politikasının güncel versiyonuna her zaman web sitesinden erişebilirsiniz. Bu Web Sitesine girmekle, bu Sözleşmeyi ve Sözleşmede herhangi bir zamanda meydana gelebilecek değişiklikleri kabul etmiş olursunuz. Web Sitesi üzerindeki tüm kullanım ve tasarruf hakları KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetlerde değişiklik yapma hakkını her zaman saklı tutar. Web sitesinin kullanımında kaynak kodunu bulmaya veya elde etmeye yönelik başka bir işlem yapmayacağınızı, aksi takdirde üçüncü kişilere gelebilecek zararlardan sorumlu olacağınızı ve hakkınızda yasal ve cezai işlem başlatılacağını önceden kabul edersiniz.</p>

        <h3>3- Yayınlanan Bilgilere Güvenme</h3>
        <p>Web Sitesindeki bilgiler sadece genel bilgi amaçlıdır ve bu bilgilerin doğruluğu garanti edilmez. Web Sitesinden elde edilen bilgilere güvenmekten doğan tüm sorumluluk size aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>Web sitesine sağladığınız kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, sizi kişisel olarak tanımlayamayan bilgilerdir. Örneğin; kullanım saatleri, Sitenin kullanıldığı konum, görüntülenen sayfalar vb.</p>

        <h3>6- Çerez Kullanımı</h3>
        <p>Çerezler, Siteyi ziyaret ettiğinizde tercihlerinizi bilgisayarınıza kaydeden küçük metin dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlama, web sitesine ve/veya mobil uygulamaya tekrar giriş yaptığınızda sizi tanıma ve en çok ziyaret ettiğiniz sayfalarda ilginizi çekebilecek reklam ve duyuruları yayınlama gibi işlemlerde size en kaliteli hizmeti sunmak için kullanılır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan finansal bilgiler, satın aldığınız ürünleri fatura etmek için kullanılır. Web sitesinde veya mobil uygulamada çevrimiçi ödeme satın alımı yaptığınızda, finansal bilgilerinizin işleminizi gerçekleştirmek için gerekli üçüncü taraflara (bankalar, kredi kartı şirketleri vb.) verileceğini kabul edersiniz.</p>

        <h3>8- Diğer Web Sitelerine Bağlantılar</h3>
        <p>Bu web sitesinden üçüncü taraflara ait diğer web sitelerine bağlantılar sağlanabilir. KFA, bağlantı verilen web sitelerindeki içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu web sitesi ve tüm sayfaları, bu internet uygulama yazılımı ve içerdiği tüm görsel çalışmalar, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenleri ve bunlarla sınırlı olmayan tüm haklar münhasıran KFA'ya, lisans verenlere veya diğer sağlayıcılara aittir.</p>

        <h3>10- Zarar Tazmini</h3>
        <p>Bu hükümleri ihlal etmenizden veya Web Sitesinin yanlış kullanımından kaynaklanan herhangi bir talep, yükümlülük, zarar, karar, hüküm, kayıp, gider veya ücretten (avukat ücretleri dahil) KFA'yı, bağlı kuruluşlarını, lisans verenlerini ve bunların çalışanlarını, yöneticilerini, temsilcilerini, tedarikçilerini ve haleflerini tazmin etmeyi ve zararları karşılamayı kabul edersiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-posta adresini kullanın.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>Bu Sözleşmeye herhangi bir çelişki olmaksızın Türk hukuku uygulanacak olup, uyuşmazlığın karşılıklı görüşmeler yoluyla çözülemediği durumlarda ilgili uyuşmazlık Bursa Mahkemeleri ve İcra Daireleri önünde çözümlenecektir.</p>
      `
    },
    'user-agreement': {
      title: 'Kullanıcı Sözleşmesi',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE WEB SİTESİ KULLANIM ŞARTLARI</h3>
        <p>Lütfen bu web sitesini kullanmadan önce aşağıdaki maddeleri dikkatlice okuyunuz ve inceleyiniz.</p>
        <p>https://les-expo.com alan adına sahip web sitesi ("Site" veya "Web Sitesi") KFA Fuarcılık A.Ş'ye ("KFA") aittir.</p>
        <p>Bu Web Sitesini kullanarak, aşağıdaki Kullanıcı Sözleşmesini kabul etmiş olursunuz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret ederek, bu Gizlilik Politikası ve Kullanım Şartlarında ("Sözleşme") belirtilen şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul edersiniz.</p>
        <p>KFA, bu web sitesindeki Gizlilik Politikası dahil olmak üzere bu koşulları ve diğer bilgileri gerekli gördüğünde zaman zaman ve herhangi bir bildirimde bulunmaksızın değiştirme hakkını saklı tutar. Bu Sözleşmenin ve Gizlilik Politikasının güncel versiyonuna her zaman web sitesinden erişebilirsiniz. Bu Web Sitesine girmekle, bu Sözleşmeyi ve Sözleşmede herhangi bir zamanda meydana gelebilecek değişiklikleri kabul etmiş olursunuz. Web Sitesi üzerindeki tüm kullanım ve tasarruf hakları KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetlerde değişiklik yapma hakkını her zaman saklı tutar. Web sitesinin kullanımında kaynak kodunu bulmaya veya elde etmeye yönelik başka bir işlem yapmayacağınızı, aksi takdirde üçüncü kişilere gelebilecek zararlardan sorumlu olacağınızı ve hakkınızda yasal ve cezai işlem başlatılacağını önceden kabul edersiniz.</p>

        <h3>3- Yayınlanan Bilgilere Güvenme</h3>
        <p>Web Sitesindeki bilgiler sadece genel bilgi amaçlıdır ve bu bilgilerin doğruluğu garanti edilmez. Web Sitesinden elde edilen bilgilere güvenmekten doğan tüm sorumluluk size aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>Web sitesine sağladığınız kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, sizi kişisel olarak tanımlayamayan bilgilerdir. Örneğin; kullanım saatleri, Sitenin kullanıldığı konum, görüntülenen sayfalar vb.</p>

        <h3>6- Çerez Kullanımı</h3>
        <p>Çerezler, Siteyi ziyaret ettiğinizde tercihlerinizi bilgisayarınıza kaydeden küçük metin dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlama, web sitesine ve/veya mobil uygulamaya tekrar giriş yaptığınızda sizi tanıma ve en çok ziyaret ettiğiniz sayfalarda ilginizi çekebilecek reklam ve duyuruları yayınlama gibi işlemlerde size en kaliteli hizmeti sunmak için kullanılır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan finansal bilgiler, satın aldığınız ürünleri fatura etmek için kullanılır. Web sitesinde veya mobil uygulamada çevrimiçi ödeme satın alımı yaptığınızda, finansal bilgilerinizin işleminizi gerçekleştirmek için gerekli üçüncü taraflara (bankalar, kredi kartı şirketleri vb.) verileceğini kabul edersiniz.</p>

        <h3>8- Diğer Web Sitelerine Bağlantılar</h3>
        <p>Bu web sitesinden üçüncü taraflara ait diğer web sitelerine bağlantılar sağlanabilir. KFA, bağlantı verilen web sitelerindeki içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu web sitesi ve tüm sayfaları, bu internet uygulama yazılımı ve içerdiği tüm görsel çalışmalar, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenleri ve bunlarla sınırlı olmayan tüm haklar münhasıran KFA'ya, lisans verenlere veya diğer sağlayıcılara aittir.</p>

        <h3>10- Zarar Tazmini</h3>
        <p>Bu hükümleri ihlal etmenizden veya Web Sitesinin yanlış kullanımından kaynaklanan herhangi bir talep, yükümlülük, zarar, karar, hüküm, kayıp, gider veya ücretten (avukat ücretleri dahil) KFA'yı, bağlı kuruluşlarını, lisans verenlerini ve bunların çalışanlarını, yöneticilerini, temsilcilerini, tedarikçilerini ve haleflerini tazmin etmeyi ve zararları karşılamayı kabul edersiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-posta adresini kullanın.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>Bu Sözleşmeye herhangi bir çelişki olmaksızın Türk hukuku uygulanacak olup, uyuşmazlığın karşılıklı görüşmeler yoluyla çözülemediği durumlarda ilgili uyuşmazlık Bursa Mahkemeleri ve İcra Daireleri önünde çözümlenecektir.</p>
      `
    }
  };

  // Open modal
  privacyLinks.forEach(link => {
    link.addEventListener('click', function (e) {
      e.preventDefault();
      const modalType = this.getAttribute('data-modal');
      const content = modalContents[modalType];

      modalTitle.textContent = content.title;
      modalContent.innerHTML = content.content;
      modal.style.display = 'block';
      document.body.style.overflow = 'hidden';
    });
  });

  // Close modal
  function closeModal() {
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
  }

  closeBtn.addEventListener('click', closeModal);

  // Close modal when clicking outside
  window.addEventListener('click', function (e) {
    if (e.target === modal) {
      closeModal();
    }
  });

  // Close modal with Escape key
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && modal.style.display === 'block') {
      closeModal();
    }
  });
});




// ================================== Footer KVKK KISMI ==================================
// Privacy Modal Functionality
document.addEventListener('DOMContentLoaded', function () {
  const modal = document.getElementById('privacyModal-en');
  const modalTitle = document.getElementById('modalTitle-en');
  const modalContent = document.getElementById('modalContent-en');
  const closeBtn = document.querySelector('.privacy-modal-close-en');
  const privacyLinks = document.querySelectorAll('.privacy-link-en');

  // KVKK and Privacy Policy content
  const modalContents = {
    kvkk: {
      title: 'LPPD Information Text',
      content: `
        <h3>INFORMATION PURSUANT TO THE PERSONAL DATA PROTECTION LAW NO. 6698</h3>
        <p><strong>The identity of the data controller and its representative, if any;</strong><br>
        This information is provided within the scope of the Personal Data Protection Law No. 6698 ("KVKK"), in the capacity of the Data Controller, in order to fulfill the obligation of informing "KFA Fuarcılık A.Ş" (Mersis No:0548143315700001) to its business partners, customers, fair visitors, and real or legal persons with whom it communicates.</p>

        <p><strong>The purpose for which personal data will be processed;</strong><br>
        Your personal data may be processed by the data controller or the legal/natural persons appointed by the data controller under the following circumstances and conditions;</p>
        <ul>
          <li>In order to improve, develop and diversify our products and services and to offer them to legal entities/real persons with whom we have commercial relations or to provide information,</li>
          <li>In order to improve, develop and diversify our products and services and to offer them to legal entities/real persons with whom we have commercial relations or to provide information,</li>
          <li>In order to plan the fair scopes in accordance with the expectations of the participants and visitors, to monitor its effectiveness and to ensure its continuity,</li>
          <li>To increase the awareness of the fairs among participants, visitors, the press, relevant civil society organizations and relevant public institutions and organizations, to establish business partnerships between them and to enrich their commercial or social activities,</li>
          <li>In order to evaluate the newly developed products and services and to determine and implement our company's commercial or business strategies,</li>
          <li>To increase our employees' data, performance level and employee satisfaction, to determine training and career plans or to ensure job security,</li>
          <li>In addition, for purposes such as conducting our necessary quality and standard audits or fulfilling our reporting and other obligations determined by laws and regulations,</li>
          <li>In addition, within the scope of the requirements and obligations determined by regulatory and supervisory authorities to ensure the fulfillment of the legal obligations specified in the KVKK, as required or made mandatory by legal regulations.</li>
        </ul>

        <p><strong>To whom and for what purpose the processed personal data can be transferred,</strong><br>
Your processed personal data can be transferred within the above purposes and the conditions determined by the KVKK;</p>
        <ul>
          <li>In order to carry out commercial activities and ensure their continuity, our company, our business partnerships or affiliates,</li>
          <li>To our suppliers, to a limited extent, in order to provide external products and services,</li>
          <li>To auditing firms within the scope of relevant contracts for the purpose of auditing our commercial activities in accordance with the relevant legislation,</li>
          <li>To our shareholders, limited to the purpose of designing strategies for our company's commercial activities,</li>
          <li>Within the scope of requests of relevant public institutions and organizations within the scope of their legal authority, to legally authorized public institutions and organizations,</li>
          <li>Institutions and organizations designated to ensure the fulfillment of legal obligations specified in the KVKK,</li>
        </ul>
        <p>may be transferred with your explicit consent or under the conditions determined by law.</p>

        <p><strong>The method and legal reason for collecting personal data,</strong><br>
        Kişisel verileriniz, şirketimiz veya şirketimiz adına veri işleyen gerçek ya da tüzel kişiler tarafından;
        internet sitesi, muhtelif sözleşmeler, her türlü ilgi bilgi formları, anketler, iş başvuru formları, iş
        sözleşmeleri, sosyal medya uygulamaları, çağrı merkezleri ve burada sayılanlarla sınırlı olmamak
        üzere sözlü, yazılı veya elektronik kanallar aracılığı ile açık rızanız ile toplanmaktadır.</p>

        <p>Bu bilgiler, ticari ve idari faaliyetlerimizin, iş ve sosyal hayatı düzenleyen yasalar çerçevesinde
        sunulabilmesi ve bu kapsamda şirketimizin ticari hayatını sürdürebilmesi, yasalardan doğan
        mesuliyetlerini eksiksiz ve doğru bir şekilde yerine getirebilmesi gayesi ile edinilir.</p>

        <p><strong>Veri Sahibinin Hakları,</strong><br>
        KVKK kapsamında, kişisel verilerinize ilişkin olarak aşağıdaki haklara sahipsiniz:</p>
        <ul>
          <li>Kişisel veri işlenip işlenmediğini öğrenme,</li>
          <li>Kişisel verileri işlenmişse buna ilişkin bilgi talep etme,</li>
          <li>Kişisel verilerin işlenme amacını ve bunların amacına uygun kullanılıp kullanılmadığını
        öğrenme,</li>
          <li>Yurt içinde veya yurt dışında kişisel verilerin aktarıldığı üçüncü kişileri bilme,</li>
          <li>Kişisel verilerin eksik veya yanlış işlenmiş olması hâlinde bunların düzeltilmesini isteme,</li>
          <li>Kişisel verinizin işlenmesini gerektiren sebeplerin ortadan kalkması hâlinde, verilerin
        silinmesini veya yok edilmesini isteme,</li>
          <li>Talebinizce düzeltilen ya da silinen bilgilerinizin, eğer aktarılmış ise kişisel verilerin aktarıldığı
        üçüncü kişilere bildirilmesini isteme,</li>
          <li>İşlenen verilerin münhasıran otomatik sistemler vasıtasıyla analiz edilmesi suretiyle kişinin
        kendisi aleyhine bir sonucun ortaya çıkmasına itiraz etme,</li>
          <li>Kişisel verilerin kanuna aykırı olarak işlenmesi sebebiyle zarara uğraması hâlinde zararın
        giderilmesini talep etme.</li>
        </ul>

        <p>Yukarıda belirtilen haklarınızı kullanmak için kimliğinizi tespit edici gerekli bilgiler ve kullanmak
        istediğiniz hakkınıza yönelik açıklamalarınızla birlikte yazılı talebinizi "[…]" adresine ıslak imzalı olarak
        ve/veya […] kayıtlı elektronik posta adresimize güvenli elektronik imza ile imzalanmış olarak
        gönderebilirsiniz.</p>

        <p>Kişisel veri sahibi olarak sahip olduğunuz ve yukarıda belirtilen haklarınızı kullanmak için yapacağınız
        ve kullanmayı talep ettiğiniz hakka ilişkin açıklamalarınızı içeren başvuruda; talep ettiğiniz hususun
        açık ve anlaşılır olması, talep ettiğiniz konunun şahsınız ile ilgili olması veya başkası adına hareket
        ediyor iseniz bu konuda özel olarak yetkili olmanız ve yetkinizi belgelendirilmesi, başvurunun kimlik ve
        adres bilgilerini içermesi ve başvuruya kimliğinizi tevsik edici belgelerin eklenmesi gerekmektedir.</p>

        <p>Bu kapsamda yapacağınız başvurular mümkün olan en kısa zaman diliminde ve en çok 30 gün
        içerisinde sonuçlandırılacaktır.</p>
      `
    },
    privacy: {
      title: 'Privacy Policy',
      content: `
        <h3>PRIVACY POLICY AND WEBSITE TERMS OF USE</h3>
        <p>Please read and review the following items carefully before using this website.</p>
        <p>The website with the domain name https://les-expo.com/ ("Site" or "Website") belongs to KFA Fuarcılık A.Ş ("KFA").</p>
        <p>By using this Website, you accept the User Agreement below.</p>

        <h3>1- Acceptance</h3>
        <p>By visiting this Site, you accept to act within the scope of the terms stated in this Privacy Policy and Terms of Use ("Agreement") and to comply with these conditions.</p>
        <p>KFA reserves the right to change these conditions and any other information, including the Privacy Policy on this website, from time to time and without any notice when deemed necessary. You can always access the current version of this Agreement and the Privacy Policy from the website. By entering this Website, you accept this Agreement and any changes that may occur in the Agreement at any time. All usage and disposal rights on the Website belong to KFA.</p>

        <h3>2- Responsibility</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetlerde değişiklik yapma hakkını her zaman saklı tutar. Web sitesinin kullanımında kaynak kodunu bulmaya veya elde etmeye yönelik başka bir işlem yapmayacağınızı, aksi takdirde üçüncü kişilere gelebilecek zararlardan sorumlu olacağınızı ve hakkınızda yasal ve cezai işlem başlatılacağını önceden kabul edersiniz.</p>

        <h3>3- Reliance on Published Information</h3>
        <p>The information on the Website is purely for general information purposes, and the accuracy of this information is not guaranteed. All responsibility arising from reliance on information obtained from the Website belongs to you.</p>

        <h3>4- Personal Data</h3>
        <p>Web sitesine sağladığınız kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Non-Personal Information</h3>
        <p>Non-personal information is information that cannot personally identify you. For example; usage hours, location where the Site is used, pages viewed, etc.</p>

        <h3>6- Cookie Usage</h3>
        <p>Cookies are small text files that save your preferences to your computer when you visit the Site. Cookies are used to provide you with the highest quality service in operations such as remembering which products are in your shopping cart, recognizing you when you log in to the website and/or mobile application again, and publishing advertisements and announcements that may interest you on the pages you visit most.</p>

        <h3>7- Payment Security</h3>
        <p>The financial information collected is used to invoice the products you purchase. When you make an online payment purchase on the website or mobile application, you accept that your financial information will be given to third parties (banks, credit card companies, etc.) necessary to carry out your transaction.</p>

        <h3>8- Links to Other Websites</h3>
        <p>Links to other websites belonging to third parties may be provided from this website. KFA is not responsible for the content on the linked websites.</p>

        <h3>9- Intellectual Property Rights</h3>
        <p>This website and all its pages, this internet application software, and all visual works, designs, interfaces, user controls, processes, programs, and software components contained therein, and all rights not limited to these exclusively belong to KFA, licensors, or other providers.</p>

        <h3>10- Compensation for Damages</h3>
        <p>You accept that you will indemnify KFA, its affiliates, licensors, and their employees, managers, representatives, suppliers, and successors from any claims, liabilities, damages, decisions, judgments, losses, expenses, or fees (including attorney fees) arising from your violation of these provisions or improper use of the Website, and compensate for the damages.</p>

        <h3>11- Contact Information</h3>
        <p>Use the e-mail address info@kfa.com.tr for your questions.</p>

        <h3>12- Applicable Law and Resolution of Disputes</h3>
        <p>Turkish law shall apply to this Agreement without any contradiction, and in cases where the dispute cannot be resolved through mutual negotiations, the relevant dispute will be resolved before the Bursa Courts and Enforcement Offices.</p>
      `
    },
    'user-agreement': {
      title: 'User Agreement',
      content: `
        <h3>PRIVACY POLICY AND WEBSITE TERMS OF USE</h3>
        <p>Please read and review the following items carefully before using this website.</p>
        <p>The website with the domain name https://les-expo.com.tr/ ("Site" or "Website") belongs to KFA Fuarcılık A.Ş ("KFA").</p>
        <p>By using this Website, you accept the User Agreement below.</p>

        <h3>1- Acceptance</h3>
        <p>By visiting this Site, you accept to act within the scope of the terms stated in this Privacy Policy and Terms of Use ("Agreement") and to comply with these conditions.</p>
        <p>KFA reserves the right to change these conditions and any other information, including the Privacy Policy on this website, from time to time and without any notice when deemed necessary. You can always access the current version of this Agreement and the Privacy Policy from the website. By entering this Website, you accept this Agreement and any changes that may occur in the Agreement at any time. All usage and disposal rights on the Website belong to KFA.</p>

        <h3>2- Responsibility</h3>
        <p>KFA always reserves the right to make changes to prices, organized fairs, and services provided. You accept in advance that you will not engage in any other operation aimed at finding or obtaining the source code in the use of the website, otherwise you will be responsible for any damages that may occur to third parties, and legal and criminal proceedings will be initiated against you.</p>

        <h3>3- Reliance on Published Information</h3>
        <p>The information on the Website is purely for general information purposes, and the accuracy of this information is not guaranteed. All responsibility arising from reliance on information obtained from the Website belongs to you.</p>

        <h3>4- Personal Data</h3>
        <p>Please read our KVKK forms to learn about the processing, recording, and protection of personal information you provide to the website.</p>

        <h3>5- Non-Personal Information</h3>
        <p>Non-personal information is information that cannot personally identify you. For example; usage hours, location where the Site is used, pages viewed, etc.</p>

        <h3>6- Cookie Usage</h3>
        <p>Cookies are small text files that save your preferences to your computer when you visit the Site. Cookies are used to provide you with the highest quality service in operations such as remembering which products are in your shopping cart, recognizing you when you log in to the website and/or mobile application again, and publishing advertisements and announcements that may interest you on the pages you visit most.</p>

        <h3>7- Payment Security</h3>
        <p>The financial information collected is used to invoice the products you purchase. When you make an online payment purchase on the website or mobile application, you accept that your financial information will be given to third parties (banks, credit card companies, etc.) necessary to carry out your transaction.</p>

        <h3>8- Links to Other Websites</h3>
        <p>Links to other websites belonging to third parties may be provided from this website. KFA is not responsible for the content on the linked websites.</p>

        <h3>9- Intellectual Property Rights</h3>
        <p>This website and all its pages, this internet application software, and all visual works, designs, interfaces, user controls, processes, programs, and software components contained therein, and all rights not limited to these exclusively belong to KFA, licensors, or other providers.</p>

        <h3>10- Compensation for Damages</h3>
        <p>You accept that you will indemnify KFA, its affiliates, licensors, and their employees, managers, representatives, suppliers, and successors from any claims, liabilities, damages, decisions, judgments, losses, expenses, or fees (including attorney fees) arising from your violation of these provisions or improper use of the Website, and compensate for the damages.</p>

        <h3>11- Contact Information</h3>
        <p>Use the e-mail address info@kfa.com.tr for your questions.</p>

        <h3>12- Applicable Law and Resolution of Disputes</h3>
        <p>Turkish law shall apply to this Agreement without any contradiction, and in cases where the dispute cannot be resolved through mutual negotiations, the relevant dispute will be resolved before the Bursa Courts and Enforcement Offices.</p>
      `
    }
  };

  // Open modal
  privacyLinks.forEach(link => {
    link.addEventListener('click', function (e) {
      e.preventDefault();
      const modalType = this.getAttribute('data-modal');
      const content = modalContents[modalType];

      modalTitle.textContent = content.title;
      modalContent.innerHTML = content.content;
      modal.style.display = 'block';
      document.body.style.overflow = 'hidden';
    });
  });

  // Close modal
  function closeModal() {
    modal.style.display = 'none';
    document.body.style.overflow = 'auto';
  }

  closeBtn.addEventListener('click', closeModal);

  // Close modal when clicking outside
  window.addEventListener('click', function (e) {
    if (e.target === modal) {
      closeModal();
    }
  });

  // Close modal with Escape key
  document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape' && modal.style.display === 'block') {
      closeModal();
    }
  });
});

