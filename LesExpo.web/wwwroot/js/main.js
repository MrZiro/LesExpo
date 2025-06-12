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


/* Search Functionality */
document.addEventListener('DOMContentLoaded', function() {
  // Arama butonunu seç
  const searchToggleButton = document.querySelector('.search-toggle-button');
  const searchOverlay = document.querySelector('.search-overlay');
  const searchClose = document.querySelector('.search-close');
  const searchInput = document.querySelector('.search-input');

  // Arama butonuna tıklandığında
  searchToggleButton.addEventListener('click', function() {
    searchOverlay.classList.add('active');
    searchInput.focus();
  });

  // Kapatma butonuna tıklandığında
  searchClose.addEventListener('click', function() {
    searchOverlay.classList.remove('active');
  });

  // ESC tuşuna basıldığında
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape' && searchOverlay.classList.contains('active')) {
      searchOverlay.classList.remove('active');
    }
  });

  // Arama çubuğuna tıklandığında overlay'i kapatma
  searchOverlay.addEventListener('click', function(e) {
    if (e.target === searchOverlay) {
      searchOverlay.classList.remove('active');
    }
  });
});


// ================================== Footer KVKK KISMI ==================================
// Privacy Modal Functionality
document.addEventListener('DOMContentLoaded', function() {
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
      title: 'Gizlilik Sözleşmesi',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE İNTERNET SİTESİ KULLANIM KOŞULLARI</h3>
        <p>Bu internet sitesini kullanmadan önce lütfen aşağıdaki maddeleri dikkatle okuyup inceleyiniz.</p>
        <p>https://les-expo.com.tr/ alan adlı internet sitesi ("Site" veya "Internet Sitesi") KFA Fuarcılık A.Ş
        ("KFA")'ne aittir.</p>
        <p>Bu İnternet Sitesini kullanmakla aşağıda yer alan Kullanıcı Sözleşmesini kabul etmektesiniz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret etmekle, işbu Gizlilik Politikası ve Kullanım Şartları'nı ("Sözleşme") açıklanan
        şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul etmektesiniz.</p>
        <p>KFA bu koşulları ve ayrıca yine işbu internet sitesinde yer alan Gizlilik Politikası dahil diğer her
        türlü bilgiyi zaman zaman ve gerekli gördüğü takdirde herhangi bir bildirimde bulunmaksızın
        değiştirme hakkını saklı tutmaktadır. Bu Sözleşmenin güncel haline ve Gizlilik Politikası'na her
        zaman internet sitesinden ulaşabilirsiniz. İşbu İnternet Sitesine giriş yapmanız, işbu
        Sözleşme'yi ve herhangi bir zamanda Sözleşme'de meydana gelebilecek olan değişiklikleri
        kabul ettiğiniz anlamına gelmektedir. İnternet Sitesi üzerinde her türlü kullanım ve tasarruf
        yetkisi KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetler üzerinde değişiklik yapma hakkını her
        zaman saklı tutar. İnternet sitesinin kullanımında kaynak kodunu bulmak veya elde etmek
        amacına yönelik herhangi bir başka işlemde bulunmayacağınızı, aksi halde ve 3. Kişiler
        nezdinde doğacak zararlardan sorumlu olacağını, hakkında hukuki ve cezai işlem yapılacağını
        peşinen kabul etmektesiniz.</p>

        <h3>3- Yayınlanan Bilgiye Güven</h3>
        <p>İnternet Sitesinde yer alan bilgiler tamamen genel bilgi niteliğinde olup, bu bilgilerin doğruluğu
        garanti edilmemektedir. İnternet Sitesinden edinilen bilgilere güvenden doğacak tüm
        sorumluluk tarafınıza aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>İnternet sitesine sunduğunuz kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında
        bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, şahsen sizlerin tanımlanamayacağımız bilgilerdir. Örneğin; kullanım
        saatleri, Site'nin kullanıldığı lokasyon, incelenen sayfalar vb. bilgilerdir.</p>

        <h3>6- Çerez (Cookie) Kullanımı</h3>
        <p>Çerezler, Site'yi ziyaret ettiğiniz zaman bilgisayarınıza tercihlerinizi kaydeden küçük metin
        dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlamak, web sitesine
        ve/veya mobil uygulamaya tekrar giriş yaptığınızda sitenin sizi tanıması, ilginizi çekebilecek ilan
        ve reklamların en çok giriş yaptığınız sayfalarda yayınlanması gibi işlemlerde size en kaliteli
        hizmeti sağlamak için kullanılmaktadır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan mali bilgiler satın aldığınız ürünleri size fatura etmek için kullanılmaktadır. Web
        sitesinde veya mobil uygulamada online ödemeli bir satın alma yaptığınızda size ait mali
        bilgilerin, işleminizi gerçekleştirmek için gerekli 3. şahıslara (bankalar, kredi kartı şirketleri vb.)
        verilmesini kabul etmektesiniz.</p>

        <h3>8- Diğer İnternet Sitelerine Verilen Linkler</h3>
        <p>Bu internet sitesinden üçüncü kişilere ait başka internet sitelerine link verilebilmektedir. KFA
        link verilen internet sitelerinde yer alan içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu internet sitesi ve tüm sayfaları ve bu internet uygulaması yazılımı ve içerisindeki tüm görsel
        eserler, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenlerin
        ve bunlarla sınırlı kalmaksızın her türlü hakkı münhasıran KFA, lisans veren veya diğer
        sağlayıcılara aittir.</p>

        <h3>10- Zararların Tazmini</h3>
        <p>Tarafınızca işbu hükümlerin ihlal edilmesi veya İnternet Sitesi'nin aykırı kullanılmasından
        doğacak her türlü talepten, sorumluluktan, zarardan, karardan, hükümden, kayıptan,
        masraftan veya ücretten (vekalet ücreti dahil olmak üzere) KFA'yi, iştiraklerini, lisans
        verenlerini ve bunların çalışanlarını, yöneticileri, temsilcilerini, tedarikçilerini ve haleflerini ari
        tutacağınızı ve zararları tazmin edeceğinizi kabul etmektesiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-mail adresini kullanınız.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>İşbu Sözleşmeye herhangi bir çelişkiye mahal vermeksizin Türk hukuku uygulanacak olup,
        ihtilafın karşılıklı müzakereler neticesinde çözümünün mümkün olmadığı hallerde söz konusu
        uyuşmazlık Bursa Mahkemeleri ve İcra Müdürlükleri nezdinde çözümlenecektir.</p>
      `
    },
    'user-agreement': {
      title: 'Kullanıcı Sözleşmesi',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE İNTERNET SİTESİ KULLANIM KOŞULLARI</h3>
        <p>Bu internet sitesini kullanmadan önce lütfen aşağıdaki maddeleri dikkatle okuyup inceleyiniz.</p>
        <p>https://les-expo.com.tr/ alan adlı internet sitesi ("Site" veya "Internet Sitesi") KFA Fuarcılık A.Ş
        ("KFA")'ne aittir.</p>
        <p>Bu İnternet Sitesini kullanmakla aşağıda yer alan Kullanıcı Sözleşmesini kabul etmektesiniz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret etmekle, işbu Gizlilik Politikası ve Kullanım Şartları'nı ("Sözleşme") açıklanan
        şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul etmektesiniz.</p>
        <p>KFA bu koşulları ve ayrıca yine işbu internet sitesinde yer alan Gizlilik Politikası dahil diğer her
        türlü bilgiyi zaman zaman ve gerekli gördüğü takdirde herhangi bir bildirimde bulunmaksızın
        değiştirme hakkını saklı tutmaktadır. Bu Sözleşmenin güncel haline ve Gizlilik Politikası'na her
        zaman internet sitesinden ulaşabilirsiniz. İşbu İnternet Sitesine giriş yapmanız, işbu
        Sözleşme'yi ve herhangi bir zamanda Sözleşme'de meydana gelebilecek olan değişiklikleri
        kabul ettiğiniz anlamına gelmektedir. İnternet Sitesi üzerinde her türlü kullanım ve tasarruf
        yetkisi KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetler üzerinde değişiklik yapma hakkını her
        zaman saklı tutar. İnternet sitesinin kullanımında kaynak kodunu bulmak veya elde etmek
        amacına yönelik herhangi bir başka işlemde bulunmayacağınızı, aksi halde ve 3. Kişiler
        nezdinde doğacak zararlardan sorumlu olacağını, hakkında hukuki ve cezai işlem yapılacağını
        peşinen kabul etmektesiniz.</p>

        <h3>3- Yayınlanan Bilgiye Güven</h3>
        <p>İnternet Sitesinde yer alan bilgiler tamamen genel bilgi niteliğinde olup, bu bilgilerin doğruluğu
        garanti edilmemektedir. İnternet Sitesinden edinilen bilgilere güvenden doğacak tüm
        sorumluluk tarafınıza aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>İnternet sitesine sunduğunuz kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında
        bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, şahsen sizlerin tanımlanamayacağımız bilgilerdir. Örneğin; kullanım
        saatleri, Site'nin kullanıldığı lokasyon, incelenen sayfalar vb. bilgilerdir.</p>

        <h3>6- Çerez (Cookie) Kullanımı</h3>
        <p>Çerezler, Site'yi ziyaret ettiğiniz zaman bilgisayarınıza tercihlerinizi kaydeden küçük metin
        dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlamak, web sitesine
        ve/veya mobil uygulamaya tekrar giriş yaptığınızda sitenin sizi tanıması, ilginizi çekebilecek ilan
        ve reklamların en çok giriş yaptığınız sayfalarda yayınlanması gibi işlemlerde size en kaliteli
        hizmeti sağlamak için kullanılmaktadır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan mali bilgiler satın aldığınız ürünleri size fatura etmek için kullanılmaktadır. Web
        sitesinde veya mobil uygulamada online ödemeli bir satın alma yaptığınızda size ait mali
        bilgilerin, işleminizi gerçekleştirmek için gerekli 3. şahıslara (bankalar, kredi kartı şirketleri vb.)
        verilmesini kabul etmektesiniz.</p>

        <h3>8- Diğer İnternet Sitelerine Verilen Linkler</h3>
        <p>Bu internet sitesinden üçüncü kişilere ait başka internet sitelerine link verilebilmektedir. KFA
        link verilen internet sitelerinde yer alan içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu internet sitesi ve tüm sayfaları ve bu internet uygulaması yazılımı ve içerisindeki tüm görsel
        eserler, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenlerin
        ve bunlarla sınırlı kalmaksızın her türlü hakkı münhasıran KFA, lisans veren veya diğer
        sağlayıcılara aittir.</p>

        <h3>10- Zararların Tazmini</h3>
        <p>Tarafınızca işbu hükümlerin ihlal edilmesi veya İnternet Sitesi'nin aykırı kullanılmasından
        doğacak her türlü talepten, sorumluluktan, zarardan, karardan, hükümden, kayıptan,
        masraftan veya ücretten (vekalet ücreti dahil olmak üzere) KFA'yi, iştiraklerini, lisans
        verenlerini ve bunların çalışanlarını, yöneticileri, temsilcilerini, tedarikçilerini ve haleflerini ari
        tutacağınızı ve zararları tazmin edeceğinizi kabul etmektesiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-mail adresini kullanınız.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>İşbu Sözleşmeye herhangi bir çelişkiye mahal vermeksizin Türk hukuku uygulanacak olup,
        ihtilafın karşılıklı müzakereler neticesinde çözümünün mümkün olmadığı hallerde söz konusu
        uyuşmazlık Bursa Mahkemeleri ve İcra Müdürlükleri nezdinde çözümlenecektir.</p>
      `
    }
  };

  // Open modal
  privacyLinks.forEach(link => {
    link.addEventListener('click', function(e) {
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
  window.addEventListener('click', function(e) {
    if (e.target === modal) {
      closeModal();
    }
  });

  // Close modal with Escape key
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape' && modal.style.display === 'block') {
      closeModal();
    }
  });
});




// ================================== Footer KVKK KISMI ==================================
// Privacy Modal Functionality
document.addEventListener('DOMContentLoaded', function() {
  const modal = document.getElementById('privacyModal-en');
  const modalTitle = document.getElementById('modalTitle-en');
  const modalContent = document.getElementById('modalContent-en');
  const closeBtn = document.querySelector('.privacy-modal-close-en');
  const privacyLinks = document.querySelectorAll('.privacy-link-en');

  // KVKK and Privacy Policy content
  const modalContents = {
    kvkk: {
      title: 'KVKK Aydınlatma Metni',
      content: `
        <h3>EĞİNCE BİLGİLENDİRME</h3>
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
      title: 'Gizlilik Sözleşmesi',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE İNTERNET SİTESİ KULLANIM KOŞULLARI</h3>
        <p>Bu internet sitesini kullanmadan önce lütfen aşağıdaki maddeleri dikkatle okuyup inceleyiniz.</p>
        <p>https://les-expo.com.tr/ alan adlı internet sitesi ("Site" veya "Internet Sitesi") KFA Fuarcılık A.Ş
        ("KFA")'ne aittir.</p>
        <p>Bu İnternet Sitesini kullanmakla aşağıda yer alan Kullanıcı Sözleşmesini kabul etmektesiniz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret etmekle, işbu Gizlilik Politikası ve Kullanım Şartları'nı ("Sözleşme") açıklanan
        şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul etmektesiniz.</p>
        <p>KFA bu koşulları ve ayrıca yine işbu internet sitesinde yer alan Gizlilik Politikası dahil diğer her
        türlü bilgiyi zaman zaman ve gerekli gördüğü takdirde herhangi bir bildirimde bulunmaksızın
        değiştirme hakkını saklı tutmaktadır. Bu Sözleşmenin güncel haline ve Gizlilik Politikası'na her
        zaman internet sitesinden ulaşabilirsiniz. İşbu İnternet Sitesine giriş yapmanız, işbu
        Sözleşme'yi ve herhangi bir zamanda Sözleşme'de meydana gelebilecek olan değişiklikleri
        kabul ettiğiniz anlamına gelmektedir. İnternet Sitesi üzerinde her türlü kullanım ve tasarruf
        yetkisi KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetler üzerinde değişiklik yapma hakkını her
        zaman saklı tutar. İnternet sitesinin kullanımında kaynak kodunu bulmak veya elde etmek
        amacına yönelik herhangi bir başka işlemde bulunmayacağınızı, aksi halde ve 3. Kişiler
        nezdinde doğacak zararlardan sorumlu olacağını, hakkında hukuki ve cezai işlem yapılacağını
        peşinen kabul etmektesiniz.</p>

        <h3>3- Yayınlanan Bilgiye Güven</h3>
        <p>İnternet Sitesinde yer alan bilgiler tamamen genel bilgi niteliğinde olup, bu bilgilerin doğruluğu
        garanti edilmemektedir. İnternet Sitesinden edinilen bilgilere güvenden doğacak tüm
        sorumluluk tarafınıza aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>İnternet sitesine sunduğunuz kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında
        bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, şahsen sizlerin tanımlanamayacağımız bilgilerdir. Örneğin; kullanım
        saatleri, Site'nin kullanıldığı lokasyon, incelenen sayfalar vb. bilgilerdir.</p>

        <h3>6- Çerez (Cookie) Kullanımı</h3>
        <p>Çerezler, Site'yi ziyaret ettiğiniz zaman bilgisayarınıza tercihlerinizi kaydeden küçük metin
        dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlamak, web sitesine
        ve/veya mobil uygulamaya tekrar giriş yaptığınızda sitenin sizi tanıması, ilginizi çekebilecek ilan
        ve reklamların en çok giriş yaptığınız sayfalarda yayınlanması gibi işlemlerde size en kaliteli
        hizmeti sağlamak için kullanılmaktadır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan mali bilgiler satın aldığınız ürünleri size fatura etmek için kullanılmaktadır. Web
        sitesinde veya mobil uygulamada online ödemeli bir satın alma yaptığınızda size ait mali
        bilgilerin, işleminizi gerçekleştirmek için gerekli 3. şahıslara (bankalar, kredi kartı şirketleri vb.)
        verilmesini kabul etmektesiniz.</p>

        <h3>8- Diğer İnternet Sitelerine Verilen Linkler</h3>
        <p>Bu internet sitesinden üçüncü kişilere ait başka internet sitelerine link verilebilmektedir. KFA
        link verilen internet sitelerinde yer alan içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu internet sitesi ve tüm sayfaları ve bu internet uygulaması yazılımı ve içerisindeki tüm görsel
        eserler, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenlerin
        ve bunlarla sınırlı kalmaksızın her türlü hakkı münhasıran KFA, lisans veren veya diğer
        sağlayıcılara aittir.</p>

        <h3>10- Zararların Tazmini</h3>
        <p>Tarafınızca işbu hükümlerin ihlal edilmesi veya İnternet Sitesi'nin aykırı kullanılmasından
        doğacak her türlü talepten, sorumluluktan, zarardan, karardan, hükümden, kayıptan,
        masraftan veya ücretten (vekalet ücreti dahil olmak üzere) KFA'yi, iştiraklerini, lisans
        verenlerini ve bunların çalışanlarını, yöneticileri, temsilcilerini, tedarikçilerini ve haleflerini ari
        tutacağınızı ve zararları tazmin edeceğinizi kabul etmektesiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-mail adresini kullanınız.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>İşbu Sözleşmeye herhangi bir çelişkiye mahal vermeksizin Türk hukuku uygulanacak olup,
        ihtilafın karşılıklı müzakereler neticesinde çözümünün mümkün olmadığı hallerde söz konusu
        uyuşmazlık Bursa Mahkemeleri ve İcra Müdürlükleri nezdinde çözümlenecektir.</p>
      `
    },
    'user-agreement': {
      title: 'Kullanıcı Sözleşmesi',
      content: `
        <h3>GİZLİLİK POLİTİKASI VE İNTERNET SİTESİ KULLANIM KOŞULLARI</h3>
        <p>Bu internet sitesini kullanmadan önce lütfen aşağıdaki maddeleri dikkatle okuyup inceleyiniz.</p>
        <p>https://les-expo.com.tr/ alan adlı internet sitesi ("Site" veya "Internet Sitesi") KFA Fuarcılık A.Ş
        ("KFA")'ne aittir.</p>
        <p>Bu İnternet Sitesini kullanmakla aşağıda yer alan Kullanıcı Sözleşmesini kabul etmektesiniz.</p>

        <h3>1- Kabul</h3>
        <p>Bu Siteyi ziyaret etmekle, işbu Gizlilik Politikası ve Kullanım Şartları'nı ("Sözleşme") açıklanan
        şartlar kapsamında hareket etmeyi ve bu koşullara uymayı kabul etmektesiniz.</p>
        <p>KFA bu koşulları ve ayrıca yine işbu internet sitesinde yer alan Gizlilik Politikası dahil diğer her
        türlü bilgiyi zaman zaman ve gerekli gördüğü takdirde herhangi bir bildirimde bulunmaksızın
        değiştirme hakkını saklı tutmaktadır. Bu Sözleşmenin güncel haline ve Gizlilik Politikası'na her
        zaman internet sitesinden ulaşabilirsiniz. İşbu İnternet Sitesine giriş yapmanız, işbu
        Sözleşme'yi ve herhangi bir zamanda Sözleşme'de meydana gelebilecek olan değişiklikleri
        kabul ettiğiniz anlamına gelmektedir. İnternet Sitesi üzerinde her türlü kullanım ve tasarruf
        yetkisi KFA'ya aittir.</p>

        <h3>2- Sorumluluk</h3>
        <p>KFA, fiyatlar, düzenlenen fuarlar ve sunulan hizmetler üzerinde değişiklik yapma hakkını her
        zaman saklı tutar. İnternet sitesinin kullanımında kaynak kodunu bulmak veya elde etmek
        amacına yönelik herhangi bir başka işlemde bulunmayacağınızı, aksi halde ve 3. Kişiler
        nezdinde doğacak zararlardan sorumlu olacağını, hakkında hukuki ve cezai işlem yapılacağını
        peşinen kabul etmektesiniz.</p>

        <h3>3- Yayınlanan Bilgiye Güven</h3>
        <p>İnternet Sitesinde yer alan bilgiler tamamen genel bilgi niteliğinde olup, bu bilgilerin doğruluğu
        garanti edilmemektedir. İnternet Sitesinden edinilen bilgilere güvenden doğacak tüm
        sorumluluk tarafınıza aittir.</p>

        <h3>4- Kişisel Veriler</h3>
        <p>İnternet sitesine sunduğunuz kişisel bilgilerin işlenmesi, kaydedilmesi ve korunması hakkında
        bilgi edinmek için lütfen KVKK formlarımızı okuyunuz.</p>

        <h3>5- Kişisel Olmayan Bilgiler</h3>
        <p>Kişisel olmayan bilgiler, şahsen sizlerin tanımlanamayacağımız bilgilerdir. Örneğin; kullanım
        saatleri, Site'nin kullanıldığı lokasyon, incelenen sayfalar vb. bilgilerdir.</p>

        <h3>6- Çerez (Cookie) Kullanımı</h3>
        <p>Çerezler, Site'yi ziyaret ettiğiniz zaman bilgisayarınıza tercihlerinizi kaydeden küçük metin
        dosyalarıdır. Çerezler, alışveriş sepetinizde hangi ürünlerin olduğunu hatırlamak, web sitesine
        ve/veya mobil uygulamaya tekrar giriş yaptığınızda sitenin sizi tanıması, ilginizi çekebilecek ilan
        ve reklamların en çok giriş yaptığınız sayfalarda yayınlanması gibi işlemlerde size en kaliteli
        hizmeti sağlamak için kullanılmaktadır.</p>

        <h3>7- Ödeme Güvenliği</h3>
        <p>Toplanan mali bilgiler satın aldığınız ürünleri size fatura etmek için kullanılmaktadır. Web
        sitesinde veya mobil uygulamada online ödemeli bir satın alma yaptığınızda size ait mali
        bilgilerin, işleminizi gerçekleştirmek için gerekli 3. şahıslara (bankalar, kredi kartı şirketleri vb.)
        verilmesini kabul etmektesiniz.</p>

        <h3>8- Diğer İnternet Sitelerine Verilen Linkler</h3>
        <p>Bu internet sitesinden üçüncü kişilere ait başka internet sitelerine link verilebilmektedir. KFA
        link verilen internet sitelerinde yer alan içerikten sorumlu değildir.</p>

        <h3>9- Fikri Mülkiyet Hakları</h3>
        <p>Bu internet sitesi ve tüm sayfaları ve bu internet uygulaması yazılımı ve içerisindeki tüm görsel
        eserler, tasarımlar, arayüzler, kullanıcı kontrolleri, süreçler, programlar ve yazılım bileşenlerin
        ve bunlarla sınırlı kalmaksızın her türlü hakkı münhasıran KFA, lisans veren veya diğer
        sağlayıcılara aittir.</p>

        <h3>10- Zararların Tazmini</h3>
        <p>Tarafınızca işbu hükümlerin ihlal edilmesi veya İnternet Sitesi'nin aykırı kullanılmasından
        doğacak her türlü talepten, sorumluluktan, zarardan, karardan, hükümden, kayıptan,
        masraftan veya ücretten (vekalet ücreti dahil olmak üzere) KFA'yi, iştiraklerini, lisans
        verenlerini ve bunların çalışanlarını, yöneticileri, temsilcilerini, tedarikçilerini ve haleflerini ari
        tutacağınızı ve zararları tazmin edeceğinizi kabul etmektesiniz.</p>

        <h3>11- İletişim Bilgileri</h3>
        <p>Sorularınız için info@kfa.com.tr e-mail adresini kullanınız.</p>

        <h3>12- Uygulanacak Hukuk ve Uyuşmazlıkların Çözümü</h3>
        <p>İşbu Sözleşmeye herhangi bir çelişkiye mahal vermeksizin Türk hukuku uygulanacak olup,
        ihtilafın karşılıklı müzakereler neticesinde çözümünün mümkün olmadığı hallerde söz konusu
        uyuşmazlık Bursa Mahkemeleri ve İcra Müdürlükleri nezdinde çözümlenecektir.</p>
      `
    }
  };

  // Open modal
  privacyLinks.forEach(link => {
    link.addEventListener('click', function(e) {
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
  window.addEventListener('click', function(e) {
    if (e.target === modal) {
      closeModal();
    }
  });

  // Close modal with Escape key
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape' && modal.style.display === 'block') {
      closeModal();
    }
  });
});

