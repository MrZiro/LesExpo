        /* Form Section */
   // Ülke bilgileri (şehirler, telefon kodu ve bayrak)
        const countries = {
            turkiye: {
                cities: [
                    { value: "01", text: "Adana" },
                    { value: "02", text: "Adıyaman" },
                    { value: "03", text: "Afyonkarahisar" },
                    { value: "04", text: "Ağrı" },
                    { value: "05", text: "Amasya" },
                    { value: "06", text: "Ankara" },
                    { value: "07", text: "Antalya" },
                    { value: "08", text: "Artvin" },
                    { value: "09", text: "Aydın" },
                    { value: "10", text: "Balıkesir" },
                    { value: "11", text: "Bilecik" },
                    { value: "12", text: "Bingöl" },
                    { value: "13", text: "Bitlis" },
                    { value: "14", text: "Bolu" },
                    { value: "15", text: "Burdur" },
                    { value: "16", text: "Bursa" },
                    { value: "17", text: "Çanakkale" },
                    { value: "18", text: "Çankırı" },
                    { value: "19", text: "Çorum" },
                    { value: "20", text: "Denizli" },
                    { value: "21", text: "Diyarbakır" },
                    { value: "22", text: "Edirne" },
                    { value: "23", text: "Elazığ" },
                    { value: "24", text: "Erzincan" },
                    { value: "25", text: "Erzurum" },
                    { value: "26", text: "Eskişehir" },
                    { value: "27", text: "Gaziantep" },
                    { value: "28", text: "Giresun" },
                    { value: "29", text: "Gümüşhane" },
                    { value: "30", text: "Hakkari" },
                    { value: "31", text: "Hatay" },
                    { value: "32", text: "Isparta" },
                    { value: "33", text: "Mersin" },
                    { value: "34", text: "İstanbul" },
                    { value: "35", text: "İzmir" },
                    { value: "36", text: "Kars" },
                    { value: "37", text: "Kastamonu" },
                    { value: "38", text: "Kayseri" },
                    { value: "39", text: "Kırklareli" },
                    { value: "40", text: "Kırşehir" },
                    { value: "41", text: "Kocaeli" },
                    { value: "42", text: "Konya" },
                    { value: "43", text: "Kütahya" },
                    { value: "44", text: "Malatya" },
                    { value: "45", text: "Manisa" },
                    { value: "46", text: "Kahramanmaraş" },
                    { value: "47", text: "Mardin" },
                    { value: "48", text: "Muğla" },
                    { value: "49", text: "Muş" },
                    { value: "50", text: "Nevşehir" },
                    { value: "51", text: "Niğde" },
                    { value: "52", text: "Ordu" },
                    { value: "53", text: "Rize" },
                    { value: "54", text: "Sakarya" },
                    { value: "55", text: "Samsun" },
                    { value: "56", text: "Siirt" },
                    { value: "57", text: "Sinop" },
                    { value: "58", text: "Sivas" },
                    { value: "59", text: "Tekirdağ" },
                    { value: "60", text: "Tokat" },
                    { value: "61", text: "Trabzon" },
                    { value: "62", text: "Tunceli" },
                    { value: "63", text: "Şanlıurfa" },
                    { value: "64", text: "Uşak" },
                    { value: "65", text: "Van" },
                    { value: "66", text: "Yozgat" },
                    { value: "67", text: "Zonguldak" },
                    { value: "68", text: "Aksaray" },
                    { value: "69", text: "Bayburt" },
                    { value: "70", text: "Karaman" },
                    { value: "71", text: "Kırıkkale" },
                    { value: "72", text: "Batman" },
                    { value: "73", text: "Şırnak" },
                    { value: "74", text: "Bartın" },
                    { value: "75", text: "Ardahan" },
                    { value: "76", text: "Iğdır" },
                    { value: "77", text: "Yalova" },
                    { value: "78", text: "Karabük" },
                    { value: "79", text: "Kilis" },
                    { value: "80", text: "Osmaniye" },
                    { value: "81", text: "Düzce" }
                ],
                phoneCode: "+90",
                flagUrl: "https://upload.wikimedia.org/wikipedia/commons/b/b4/Flag_of_Turkey.svg",
                phonePlaceholder: "501 234 56 78"
            },
            almanya: {
                cities: [
                    { value: "berlin", text: "Berlin" },
                    { value: "hamburg", text: "Hamburg" },
                    { value: "munih", text: "Münih" },
                    { value: "koln", text: "Köln" },
                    { value: "frankfurt", text: "Frankfurt" },
                    { value: "stuttgart", text: "Stuttgart" },
                    { value: "dusseldorf", text: "Düsseldorf" },
                    { value: "dortmund", text: "Dortmund" },
                    { value: "essen", text: "Essen" },
                    { value: "leipzig", text: "Leipzig" },
                    { value: "bremen", text: "Bremen" },
                    { value: "dresden", text: "Dresden" },
                    { value: "hannover", text: "Hannover" },
                    { value: "nuremberg", text: "Nürnberg" },
                    { value: "duisburg", text: "Duisburg" }
                ],
                phoneCode: "+49",
                flagUrl: "https://upload.wikimedia.org/wikipedia/commons/b/ba/Flag_of_Germany.svg",
                phonePlaceholder: "170 1234567"
            },
            fransa: {
                cities: [
                    { value: "paris", text: "Paris" },
                    { value: "marseille", text: "Marsilya" },
                    { value: "lyon", text: "Lyon" },
                    { value: "toulouse", text: "Toulouse" },
                    { value: "nice", text: "Nice" },
                    { value: "nantes", text: "Nantes" },
                    { value: "strasbourg", text: "Strasbourg" },
                    { value: "montpellier", text: "Montpellier" },
                    { value: "bordeaux", text: "Bordeaux" },
                    { value: "lille", text: "Lille" },
                    { value: "rennes", text: "Rennes" },
                    { value: "reims", text: "Reims" },
                    { value: "toulon", text: "Toulon" },
                    { value: "grenoble", text: "Grenoble" },
                    { value: "dijon", text: "Dijon" }
                ],
                phoneCode: "+33",
                flagUrl: "https://upload.wikimedia.org/wikipedia/commons/c/c3/Flag_of_France.svg",
                phonePlaceholder: "6 12 34 56 78"
            },
            ingiltere: {
                cities: [
                    { value: "london", text: "Londra" },
                    { value: "manchester", text: "Manchester" },
                    { value: "birmingham", text: "Birmingham" },
                    { value: "leeds", text: "Leeds" },
                    { value: "liverpool", text: "Liverpool" },
                    { value: "newcastle", text: "Newcastle" },
                    { value: "sheffield", text: "Sheffield" },
                    { value: "bristol", text: "Bristol" },
                    { value: "nottingham", text: "Nottingham" },
                    { value: "southampton", text: "Southampton" },
                    { value: "portsmouth", text: "Portsmouth" },
                    { value: "oxford", text: "Oxford" },
                    { value: "cambridge", text: "Cambridge" },
                    { value: "york", text: "York" },
                    { value: "brighton", text: "Brighton" }
                ],
                phoneCode: "+44",
                flagUrl: "https://upload.wikimedia.org/wikipedia/commons/8/83/Flag_of_the_United_Kingdom_%283-5%29.svg",
                phonePlaceholder: "7911 123456"
            },
            rusya: {
                cities: [
                    { value: "moscow", text: "Moskova" },
                    { value: "saint-petersburg", text: "St. Petersburg" },
                    { value: "novosibirsk", text: "Novosibirsk" },
                    { value: "yekaterinburg", text: "Yekaterinburg" },
                    { value: "nizhny-novgorod", text: "Nizhny Novgorod" },
                    { value: "kazan", text: "Kazan" },
                    { value: "chelyabinsk", text: "Chelyabinsk" },
                    { value: "omsk", text: "Omsk" },
                    { value: "samara", text: "Samara" },
                    { value: "rostov-on-don", text: "Rostov-on-Don" },
                    { value: "ufa", text: "Ufa" },
                    { value: "krasnoyarsk", text: "Krasnoyarsk" },
                    { value: "voronezh", text: "Voronezh" },
                    { value: "perm", text: "Perm" },
                    { value: "volgograd", text: "Volgograd" },
                    { value: "krasnodar", text: "Krasnodar" },
                    { value: "saratov", text: "Saratov" },
                    { value: "tyumen", text: "Tyumen" },
                    { value: "tolyatti", text: "Tolyatti" },
                    { value: "izhevsk", text: "Izhevsk" }
                ],
                phoneCode: "+7",
                flagUrl: "https://upload.wikimedia.org/wikipedia/commons/f/f3/Flag_of_Russia.svg",
                phonePlaceholder: "999 123-45-67"
            }
        };

        // Ülke seçildiğinde şehirleri ve telefon kodunu güncelle
        document.getElementById('ulke').addEventListener('change', function() {
            const selectedCountry = this.value;
            const citySelect = document.getElementById('sehir');
            const phoneCodeElement = document.getElementById('phoneCode');
            const countryFlagElement = document.getElementById('countryFlag');
            const phoneInput = document.getElementById('telefon');
            
            // Şehir seçeneğini temizle
            citySelect.innerHTML = '';
            
            // Eğer bir ülke seçilmişse
            if (selectedCountry) {
                const countryData = countries[selectedCountry];
                
                // İlk seçenek olarak "Lütfen Seçiniz" ekle
                const defaultOption = document.createElement('option');
                defaultOption.value = '';
                defaultOption.text = 'Lütfen Seçiniz';
                citySelect.appendChild(defaultOption);
                
                // Seçilen ülkenin şehirlerini ekle
                countryData.cities.forEach(city => {
                    const option = document.createElement('option');
                    option.value = city.value;
                    option.text = city.text;
                    citySelect.appendChild(option);
                });
                
                // Telefon kodu ve bayrağı güncelle
                phoneCodeElement.innerHTML = `
                    <img src="${countryData.flagUrl}" alt="${selectedCountry}" width="20" id="countryFlag">
                    ${countryData.phoneCode}
                `;
                
                // Telefon placeholder'ını güncelle
                phoneInput.placeholder = countryData.phonePlaceholder;
            } else {
                // Eğer ülke seçilmemişse, varsayılan mesajı göster
                const defaultOption = document.createElement('option');
                defaultOption.value = '';
                defaultOption.text = 'Lütfen Önce Ülke Seçiniz';
                citySelect.appendChild(defaultOption);
                
                // Varsayılan telefon kodu ve bayrağı göster (Türkiye)
                phoneCodeElement.innerHTML = `
                    <img src="${countries.turkiye.flagUrl}" alt="TR" width="20" id="countryFlag">
                    ${countries.turkiye.phoneCode}
                `;
                
                // Varsayılan telefon placeholder'ı
                phoneInput.placeholder = countries.turkiye.phonePlaceholder;
            }
        });

        

        // Yeni Ekle butonları için fonksiyonlar
        document.addEventListener('DOMContentLoaded', function() {
            // Çarpı butonu oluşturma fonksiyonu
            function createRemoveButton() {
                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.className = 'remove-form-btn';
                removeBtn.innerHTML = '×';
                removeBtn.addEventListener('click', function() {
                    this.closest('.form-row-container').remove();
                });
                return removeBtn;
            }

            // Ulusal Fuarlar için
            const addNationalFairBtn = document.getElementById('addNationalFair');
            const nationalFairsContainer = document.getElementById('nationalFairsContainer');
            let nationalFairCount = 1;

            addNationalFairBtn.addEventListener('click', function() {
                const newContainer = document.createElement('div');
                newContainer.className = 'form-row-container';
                const newRow = document.querySelector('.national-fair-row').cloneNode(true);
                nationalFairCount++;
                
                // Yeni input ID'lerini güncelle
                const inputs = newRow.querySelectorAll('input');
                inputs.forEach(input => {
                    const oldId = input.id;
                    input.id = oldId + nationalFairCount;
                    input.value = ''; // Değerleri temizle
                });
                
                // Label'ları güncelle
                const labels = newRow.querySelectorAll('label');
                labels.forEach(label => {
                    const oldFor = label.getAttribute('for');
                    label.setAttribute('for', oldFor + nationalFairCount);
                });
                
                // Çarpı butonunu ekle
                newContainer.appendChild(createRemoveButton());
                newContainer.appendChild(newRow);
                nationalFairsContainer.appendChild(newContainer);
            });

            // Uluslararası Fuarlar için
            const addInternationalFairBtn = document.getElementById('addInternationalFair');
            const internationalFairsContainer = document.getElementById('internationalFairsContainer');
            let internationalFairCount = 1;

            addInternationalFairBtn.addEventListener('click', function() {
                const newContainer = document.createElement('div');
                newContainer.className = 'form-row-container';
                const newRow = document.querySelector('.international-fair-row').cloneNode(true);
                internationalFairCount++;
                
                // Yeni input ID'lerini güncelle
                const inputs = newRow.querySelectorAll('input');
                inputs.forEach(input => {
                    const oldId = input.id;
                    input.id = oldId + internationalFairCount;
                    input.value = ''; // Değerleri temizle
                });
                
                // Label'ları güncelle
                const labels = newRow.querySelectorAll('label');
                labels.forEach(label => {
                    const oldFor = label.getAttribute('for');
                    label.setAttribute('for', oldFor + internationalFairCount);
                });
                
                // Çarpı butonunu ekle
                newContainer.appendChild(createRemoveButton());
                newContainer.appendChild(newRow);
                internationalFairsContainer.appendChild(newContainer);
            });
        });
