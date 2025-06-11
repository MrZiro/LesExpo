/* Ticket Form Section */
document.addEventListener('DOMContentLoaded', function() {
    const countries = {};
    const countrySelect = document.getElementById('country');
    const citySelect = document.getElementById('city');
    const sectorSelect = document.getElementById('sector');
    
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
                    countrySelect.appendChild(option);
                });
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
            // Add any client-side validation or UI feedback here if needed
            const submitButton = this.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
                submitButton.textContent = lang === 'en' ? 'Submitting...' : 'Gönderiliyor...';
            }
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