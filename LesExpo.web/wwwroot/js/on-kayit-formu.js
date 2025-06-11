/* Form Section */
document.addEventListener('DOMContentLoaded', function() {
    const countries = {};
    const ulkeSelect = document.getElementById('ulke');
    const citySelect = document.getElementById('sehir');
    const faaliyetAlaniSelect = document.getElementById('FaaliyetAlani');
    
    // Get language from URL
    const lang = getLangFromUrl();
    
    // Localized text based on language
    const texts = {
        en: {
            pleaseSelect: "Please Select"
        },
        tr: {
            pleaseSelect: "Lütfen Seçiniz"
        }
    };
    
    const currentTexts = texts[lang] || texts.tr;

    // Fetch countries and populate the dropdown
    fetch('/ExtranalData/GetStates')
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(apiResponse => {
            if (apiResponse.success && Array.isArray(apiResponse.data)) {
                console.log('Fetched countries:', apiResponse); // Debugging log
                apiResponse.data.forEach(country => {
                    countries[country.ulkeId] = {
                        ulkeAdi: country.ulkeAdi,
                        cities: [],
                    };

                    const option = document.createElement('option');
                    option.value = country.ulkeId;
                    option.textContent = country.ulkeAdi;
                    ulkeSelect.appendChild(option);
                });
            } else {
                console.error('Unexpected API response format:', apiResponse);
            }
        })
        .catch(error => {
            console.error('Error fetching countries:', error);
        });

    // Fetch cities dynamically when a country is selected
    if (ulkeSelect) {
        ulkeSelect.addEventListener('change', function() {
            const selectedCountryId = this.value;

            // Clear city dropdown
            citySelect.innerHTML = `<option value="">${currentTexts.pleaseSelect}</option>`;

            fetch(`/ExtranalData/GetCities?ulkeId=${selectedCountryId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    return response.json();
                })
                .then(apiResponse => {
                    if (apiResponse.success && Array.isArray(apiResponse.data)) {
                        console.log('Fetched cities for country:', selectedCountryId, apiResponse); // Debugging log
                        apiResponse.data.forEach(city => {
                            const option = document.createElement('option');
                            option.value = city.sehirId;
                            option.textContent = city.sehirAdi;
                            citySelect.appendChild(option);
                        });
                    } else {
                        console.error('Unexpected API response format:', apiResponse);
                    }
                })
                .catch(error => {
                    console.error('Error fetching cities:', error);
                });
        });
    }

    // Fetch sectors and populate the dropdown
    fetch('/ExtranalData/GetSector')
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(apiResponse => {
            if (apiResponse.success && Array.isArray(apiResponse.data)) {
                console.log('Fetched sectors:', apiResponse); // Debugging log
                apiResponse.data.forEach(sector => {
                    const option = document.createElement('option');
                    option.value = sector.sektorId;
                    option.textContent = sector.sektorName;
                    faaliyetAlaniSelect.appendChild(option);
                });
            } else {
                console.error('Unexpected API response format:', apiResponse);
            }
        })
        .catch(error => {
            console.error('Error fetching sectors:', error);
        });

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
    if (addNationalFairBtn) {
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
    }

    // Uluslararası Fuarlar için
    const addInternationalFairBtn = document.getElementById('addInternationalFair');
    if (addInternationalFairBtn) {
        const internationalFairsContainer = document.getElementById('internationalFairsContainer');
        let internationalFairCount = 1;

        addInternationalFairBtn.addEventListener('click', function() {
            const newContainer = document.createElement('div');
            newContainer.className = 'form-row-container';
            const newRow = document.querySelector('.international-fair-row').cloneNode(true);
            internationalFairCount++;

            // Update input IDs
            const inputs = newRow.querySelectorAll('input');
            inputs.forEach(input => {
                const oldId = input.id;
                input.id = oldId + internationalFairCount;
                input.value = ''; // Clear values
            });

            // Update labels
            const labels = newRow.querySelectorAll('label');
            labels.forEach(label => {
                const oldFor = label.getAttribute('for');
                label.setAttribute('for', oldFor + internationalFairCount);
            });

            // Add remove button
            newContainer.appendChild(createRemoveButton());
            newContainer.appendChild(newRow);
            internationalFairsContainer.appendChild(newContainer);
        });
    }
});

function getLangFromUrl() {
    const path = window.location.pathname; // Gets the path part of the URL, e.g., "/en/on-kayit-formu"
    const segments = path.split('/');      // Splits the path into an array: ["", "en", "on-kayit-formu"]

    // The 'lang' segment will be at index 1 if the URL structure is consistent
    if (segments.length > 1) {
        return segments[1];
    }

    return 'tr'; // Default to Turkish if 'lang' is not found
}

 // Privacy Policy Modal and Checkbox Functionality
        document.addEventListener('DOMContentLoaded', function() {
            const privacyCheckbox = document.getElementById('kabulEdiyorum');
            const submitButton = document.querySelector('.btn-okf-1');
            const privacyModal = new bootstrap.Modal(document.getElementById('privacyModal'));
            const acceptPrivacyButton = document.getElementById('acceptPrivacy');

            // Checkbox'a tıklandığında modal'ı aç
            privacyCheckbox.addEventListener('click', function(e) {
                e.preventDefault(); // Checkbox'ın varsayılan davranışını engelle
                privacyModal.show();
            });

            // Modal'da "Kabul Ediyorum" butonuna tıklandığında
            acceptPrivacyButton.addEventListener('click', function() {
                privacyCheckbox.checked = true;
                privacyModal.hide();
                submitButton.disabled = false;
            });

            // Form gönderilmeden önce kontrol
            submitButton.addEventListener('click', function(e) {
                if (!privacyCheckbox.checked) {
                    e.preventDefault();
                    alert('Lütfen aydınlatma metnini okuyup kabul ediniz.');
                    privacyModal.show();
                }
            });

            // Sayfa yüklendiğinde submit butonunu devre dışı bırak
            submitButton.disabled = true;

            // Checkbox işaretli değilse submit butonunu devre dışı bırak
            privacyCheckbox.addEventListener('change', function() {
                submitButton.disabled = !this.checked;
            });
        });