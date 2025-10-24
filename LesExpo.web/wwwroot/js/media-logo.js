
// Logo verileri - GERÇEK LOGO DOSYALARINIZIN URL'LERİNİ BURAYA EKLEYİN
const logos = [
    {
        id: 1,
        title: "KFA White Logo",
        format: "PNG",
        imageUrl: "/images/kfa-logo.png", // Gerçek logo URL'si
        theme: "light" // Beyaz logo, koyu arka plan
    },
    {
        id: 2,
        title: "KFA Black Logo",
        format: "PNG",
        imageUrl: "/images/kfa-siyah.png", // Gerçek logo URL'si
        theme: "dark" // Siyah logo, açık arka plan
    },
    {
        id: 3,
        title: "İFM White Logo",
        format: "PNG",
        imageUrl: "/images/ifmlogo_300x130.png", // Gerçek logo URL'si
        theme: "light"
    },
    {
        id: 4,
        title: "İFM Black Logo",
        format: "PNG",
        imageUrl: "/images/ifm-siyah.png", // Gerçek logo URL'si
        theme: "dark"
    },
    {
        id: 5,
        title: "Les-Expo Logo",
        format: "PNG",
        imageUrl: "/images/Les-Expo-Logo-TR-Y.png", // Gerçek logo URL'si
        theme: "dark"
    },
    {
        id: 6,
        title: "Les-Expo Logo",
        format: "PNG",
        imageUrl: "/images/Les-Expo-Logo-TR-YT.png", // Gerçek logo URL'si
        theme: "dark"
    },
    {
        id: 7,
        title: "Les-Expo Logo",
        format: "PNG",
        imageUrl: "/images/Les-Expo-Logo-TR.png", // Gerçek logo URL'si
        theme: "dark"
    },
    {
        id: 8,
        title: "Les-Expo Logo",
        format: "PNG",
        imageUrl: "/images/Les-Expo-Logo.png", // Gerçek logo URL'si
        theme: "dark"
    }
];

// Logoları ekrana yazdırma fonksiyonu
function renderLogos() {
    const container = document.getElementById('logoContainer');

    logos.forEach(logo => {
        const logoCard = document.createElement('div');
        logoCard.className = 'logo-card-2';

        // Arka plan rengini theme'e göre ayarla
        const bgColor = logo.theme === 'dark' ? '#fff' : '#000';
        const imgContainerStyle = `background-color: ${bgColor};`;

        logoCard.innerHTML = `
                    <div class="logo-img-container-2" style="${imgContainerStyle}">
                        <img src="${logo.imageUrl}" alt="${logo.title}" class="logo-img-2">
                    </div>
                    <div class="logo-info-2">
                        <div class="logo-title-2">${logo.title}</div>
                        <span class="logo-format-2">${logo.format}</span>
                    </div>
                    <button class="download-btn-2" onclick="downloadLogo(${logo.id})">Download Logo</button>
                `;

        container.appendChild(logoCard);
    });
}

// GERÇEK LOGO İNDİRME FONKSİYONU
function downloadLogo(logoId) {
    const logo = logos.find(l => l.id === logoId);
    if (logo) {
        // Yeni bir anchor elementi oluştur
        const link = document.createElement('a');

        // İndirilecek dosyanın URL'sini ayarla
        link.href = logo.imageUrl;

        // İndirilen dosyanın adını ayarla (boşlukları alt çizgiyle değiştir)
        link.download = `${logo.title.replace(/\s+/g, '_')}.${logo.format.toLowerCase()}`;

        // Elementi sayfaya ekle (görünmez)
        link.style.display = 'none';
        document.body.appendChild(link);

        // Tıklama simülasyonu yap
        link.click();

        // Temizlik yap
        document.body.removeChild(link);
    }
}

// Sayfa yüklendiğinde logoları render et
document.addEventListener('DOMContentLoaded', renderLogos);

/*====================================================LOGO END=================================================================*/