/* Ticket Form Section */
document.addEventListener('DOMContentLoaded', function() {
    const countries = {};
    const countrySelect = document.getElementById('country');
    const citySelect = document.getElementById('city');
    const sectorSelect = document.getElementById('sector');
    const phoneInput = document.getElementById('phone');
    const phoneCodeSpan = document.getElementById('phoneCode');
    const countryFlagImgId = 'countryFlag';
    let phoneCodes = [];
    
    // Get language from URL
    const lang = getLangFromUrl();
    
    // Localized text based on language
    const texts = {
        en: {
            pleaseSelect: "Please Select",
            selectCountryFirst: "Select country first"
        },
        tr: {
            pleaseSelect: "Seçiniz",
            selectCountryFirst: "Önce ülke seçiniz"
        }
    };
    
    const currentTexts = texts[lang] || texts.tr;

    // Load phone codes (flags and dialing codes)
    fetch('/json/phone-codes.json')
        .then(r=>r.json())
        .then(data=>{ phoneCodes = Array.isArray(data) ? data : []; })
        .catch(()=>{ phoneCodes = []; });

    function findPhoneCodeByCountryId(ulkeId){
        const idNum = typeof ulkeId === 'string' ? parseInt(ulkeId) : ulkeId;
        return phoneCodes.find(x => x.ulkeId === idNum);
    }

    function updatePhoneUI(ulkeId){
        const info = findPhoneCodeByCountryId(ulkeId);
        if(info){
            if (phoneCodeSpan){
                phoneCodeSpan.innerHTML = `<img id="${countryFlagImgId}" src="${info.flagUrl}" width="20" alt="${info.iso2}" /> ${info.phoneCode}`;
            } else if (phoneInput){
                const code = info.phoneCode + ' ';
                if (!phoneInput.value || /^\+\d+\s*$/.test(phoneInput.value)){
                    phoneInput.value = code;
                } else {
                    phoneInput.value = phoneInput.value.replace(/^\+\d+\s*/, code);
                }
            }
        }
    }

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
                apiResponse.data
                    .filter(country => country.ulkeId !== 0)
                    .forEach(country => {
                        countries[country.ulkeId] = {
                            ulkeAdi: country.ulkeAdi,
                            cities: [],
                        };

                        const option = document.createElement('option');
                        option.value = country.ulkeId;
                        option.textContent = country.ulkeAdi;
                        countrySelect.appendChild(option);
                    });
                if (countrySelect && countrySelect.value){
                    updatePhoneUI(countrySelect.value);
                }
            } else {
                console.error('Unexpected API response format:', apiResponse);
            }
        })
        .catch(error => {
            console.error('Error fetching countries:', error);
        });

    // Fetch cities dynamically when a country is selected
    if (countrySelect) {
        countrySelect.addEventListener('change', function() {
            const selectedCountryId = this.value;

            // Clear city dropdown
            citySelect.innerHTML = `<option value="">${currentTexts.pleaseSelect}</option>`;

            if (selectedCountryId) {
                // Update phone code + flag
                updatePhoneUI(selectedCountryId);
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
            }
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
                    sectorSelect.appendChild(option);
                });
            } else {
                console.error('Unexpected API response format:', apiResponse);
            }
        })
        .catch(error => {
            console.error('Error fetching sectors:', error);
        });

    // Form submission handling
    const ticketForm = document.getElementById('ticketForm');
    if (ticketForm) {
        ticketForm.addEventListener('submit', function(e) {
            const submitButton = this.querySelector('button[type="submit"]');
            
            // Check if form is valid before disabling button
            if (ticketForm.checkValidity()) {
                // Form is valid, safe to disable button
                if (submitButton) {
                    submitButton.disabled = true;
                    submitButton.textContent = lang === 'en' ? 'Submitting...' : 'Gönderiliyor...';
                }
            }
            // If form is not valid, don't disable button - let validation errors show
        });

        // Re-enable button if user makes changes after failed submission
        const formInputs = ticketForm.querySelectorAll('input, select, textarea');
        formInputs.forEach(input => {
            input.addEventListener('input', function() {
                const submitButton = ticketForm.querySelector('button[type="submit"]');
                if (submitButton && submitButton.disabled) {
                    submitButton.disabled = false;
                    submitButton.textContent = lang === 'en' ? 'Submit' : 'Gönder';
                }
            });
        });
    }
});

function getLangFromUrl() {
    const path = window.location.pathname; // Gets the path part of the URL, e.g., "/en/online-ticket"
    const segments = path.split('/');      // Splits the path into an array: ["", "en", "online-ticket"]

    // The 'lang' segment will be at index 1 if the URL structure is consistent
    if (segments.length > 1) {
        return segments[1];
    }

    return 'tr'; // Default to Turkish if 'lang' is not found
} 