document.addEventListener("DOMContentLoaded", () => {
  // Örnek veri
  const participantsData = [
    {
      firmaAdi: "",
      adresUlke: "Türkiye",
      salon: "5",
      stantNo: "5252",
      urunGrubu: "Vinç Sistemleri",
    },
    {
      firmaAdi: "",
      adresUlke: "Türkiye",
      salon: "5",
      stantNo: "5251",
      urunGrubu: "Ağır Yük Taşıma",
    },
    {
      firmaAdi: "",
      adresUlke: "Türkiye",
      salon: "5",
      stantNo: "5271",
      urunGrubu: "Kaldırma Ekipmanları",
    },
    {
      firmaAdi: "",
      adresUlke: "Almanya",
      salon: "3",
      stantNo: "3120",
      urunGrubu: "Vinç Sistemleri",
    },
    {
      firmaAdi: "",
      adresUlke: "Tayvan",
      salon: "2",
      stantNo: "2045",
      urunGrubu: "Vinç Sistemleri",
    },
    {
      firmaAdi: "",
      adresUlke: "Ukrayna",
      salon: "4",
      stantNo: "4189",
      urunGrubu: "Taşıma Sistemleri",
    },
    {
      firmaAdi: "",
      adresUlke: "İsveç",
      salon: "1",
      stantNo: "1234",
      urunGrubu: "Kaldırma Ekipmanları",
    },
  ]

  // Tabloyu doldur
  populateTable(participantsData)

  // Ülke listesi - 40 ülke
  const countries = [
    "Almanya",
    "Amerika Birleşik Devletleri",
    "Avustralya",
    "Avusturya",
    "Azerbaycan",
    "Belçika",
    "Birleşik Arap Emirlikleri",
    "Birleşik Krallık",
    "Brezilya",
    "Bulgaristan",
    "Çek Cumhuriyeti",
    "Çin",
    "Danimarka",
    "Endonezya",
    "Finlandiya",
    "Fransa",
    "Güney Afrika",
    "Güney Kore",
    "Hindistan",
    "Hollanda",
    "İran",
    "İspanya",
    "İsrail",
    "İsveç",
    "İsviçre",
    "İtalya",
    "Japonya",
    "Kanada",
    "Malezya",
    "Meksika",
    "Norveç",
    "Polonya",
    "Portekiz",
    "Romanya",
    "Rusya",
    "Singapur",
    "Sri Lanka",
    "Tayvan",
    "Türkiye",
    "Ukrayna",
  ]

  // Ülke dropdown'ını doldur
  const countryItemsContainer = document.querySelector("#country-items .options-container-klf")
  countryItemsContainer.innerHTML = ""

  countries.forEach((country) => {
    const countryItem = document.createElement("div")
    countryItem.className = "select-item-klf"
    countryItem.dataset.value = country.toLowerCase().replace(/\s+/g, "-")
    countryItem.textContent = country
    countryItemsContainer.appendChild(countryItem)
  })

  // Ülke dropdown'ını doldur kısmından sonra bu satırları ekle:
  const updatedCountryOptions = document.querySelectorAll("#country-items .select-item-klf")
  const updatedProductOptions = document.querySelectorAll("#product-items .select-item-klf")

  // Ülke dropdown işlevselliği
  const countrySelect = document.getElementById("selected-country")
  const countryItems = document.getElementById("country-items")
  const countryOptions = document.querySelectorAll("#country-items .select-item")
  const productOptions = document.querySelectorAll("#product-items .select-item")
  const countrySearch = document.getElementById("country-search")

  // Ürün dropdown işlevselliği
  const productSelect = document.getElementById("selected-product")
  const productItems = document.getElementById("product-items")

  // Firma adı filtreleme
  const firmaAdiInput = document.getElementById("firma-adi")
  firmaAdiInput.addEventListener("input", filterParticipants)

  // Ülke dropdown tıklama olayı
  countrySelect.addEventListener("click", () => {
    countryItems.classList.toggle("select-hide-klf")
    resetSearch()
  })

  // Ürün dropdown tıklama olayı
  productSelect.addEventListener("click", () => {
    productItems.classList.toggle("select-hide-klf")
  })

  // Dropdown dışına tıklama olayı
  document.addEventListener("click", (e) => {
    if (!e.target.closest(".custom-select-klf")) {
      countryItems.classList.add("select-hide-klf")
      productItems.classList.add("select-hide-klf")
    }
  })

  // Ülke seçenekleri tıklama olayı
  updatedCountryOptions.forEach((option) => {
    option.addEventListener("click", function () {
      countrySelect.textContent = this.textContent
      countryItems.classList.add("select-hide-klf")
      filterParticipants()
    })
  })

  // Ürün seçenekleri tıklama olayı
  updatedProductOptions.forEach((option) => {
    option.addEventListener("click", function () {
      productSelect.textContent = this.textContent
      productItems.classList.add("select-hide-klf")
      filterParticipants()
    })
  })

  // Ülke arama işlevselliği
  countrySearch.addEventListener("input", function () {
    const searchValue = this.value.toLowerCase()

    updatedCountryOptions.forEach((option) => {
      const text = option.textContent.toLowerCase()
      if (text.includes(searchValue)) {
        option.style.display = ""
      } else {
        option.style.display = "none"
      }
    })
  })

  // Arama kutusunu sıfırla
  function resetSearch() {
    countrySearch.value = ""
    updatedCountryOptions.forEach((option) => {
      option.style.display = ""
    })
  }

  // Filtreleme fonksiyonu
  function filterParticipants() {
    const firmaAdiFilter = firmaAdiInput.value.toLowerCase()
    const ulkeFilter = countrySelect.textContent
    const urunFilter = productSelect.textContent

    const filteredData = participantsData.filter((participant) => {
      const firmaAdiMatch = participant.firmaAdi.toLowerCase().includes(firmaAdiFilter)
      const ulkeMatch = ulkeFilter === "Türkiye" ? true : participant.adresUlke === ulkeFilter

      // Ürün grupları filtrelemesi
      const urunMatch = urunFilter === "Vinç Sistemleri" ? true : participant.urunGrubu === urunFilter

      return firmaAdiMatch && ulkeMatch && urunMatch
    })

    populateTable(filteredData)
  }

  // Tabloyu doldurma fonksiyonu
  function populateTable(data) {
    const tableBody = document.querySelector("#participants-table tbody")
    tableBody.innerHTML = ""

    if (data.length === 0) {
      const row = document.createElement("tr")
      const cell = document.createElement("td")
      cell.colSpan = 4
      cell.textContent = "Sonuç bulunamadı"
      cell.style.textAlign = "center"
      row.appendChild(cell)
      tableBody.appendChild(row)
      return
    }

    data.forEach((participant) => {
      const row = document.createElement("tr")

      const firmaAdiCell = document.createElement("td")
      firmaAdiCell.textContent = participant.firmaAdi

      const ulkeCell = document.createElement("td")
      ulkeCell.textContent = participant.adresUlke

      const salonCell = document.createElement("td")
      salonCell.textContent = participant.salon

      const stantNoCell = document.createElement("td")
      stantNoCell.textContent = participant.stantNo

      row.appendChild(firmaAdiCell)
      row.appendChild(ulkeCell)
      row.appendChild(salonCell)
      row.appendChild(stantNoCell)

      tableBody.appendChild(row)
    })
  }
})
