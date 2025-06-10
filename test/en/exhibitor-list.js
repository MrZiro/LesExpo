document.addEventListener("DOMContentLoaded", () => {
  // Sample data
  const participantsData = [
    {
      companyName: "",
      country: "Turkey",
      hall: "5",
      standNo: "5252",
      productGroup: "Crane Systems",
    },
    {
      companyName: "",
      country: "Turkey",
      hall: "5",
      standNo: "5251",
      productGroup: "Heavy Load Transport",
    },
    {
      companyName: "",
      country: "Turkey",
      hall: "5",
      standNo: "5271",
      productGroup: "Lifting Equipment",
    },
    {
      companyName: "",
      country: "Germany",
      hall: "3",
      standNo: "3120",
      productGroup: "Crane Systems",
    },
    {
      companyName: "",
      country: "Taiwan",
      hall: "2",
      standNo: "2045",
      productGroup: "Crane Systems",
    },
    {
      companyName: "",
      country: "Ukraine",
      hall: "4",
      standNo: "4189",
      productGroup: "Transport Systems",
    },
    {
      companyName: "",
      country: "Sweden",
      hall: "1",
      standNo: "1234",
      productGroup: "Lifting Equipment",
    },
  ]

  // Populate table
  populateTable(participantsData)

  // Country list - 40 countries
  const countries = [
    "Australia",
    "Austria",
    "Azerbaijan",
    "Belgium",
    "Brazil",
    "Bulgaria",
    "Canada",
    "China",
    "Czech Republic",
    "Denmark",
    "Finland",
    "France",
    "Germany",
    "India",
    "Indonesia",
    "Iran",
    "Israel",
    "Italy",
    "Japan",
    "Malaysia",
    "Mexico",
    "Netherlands",
    "Norway",
    "Poland",
    "Portugal",
    "Romania",
    "Russia",
    "Singapore",
    "South Africa",
    "South Korea",
    "Spain",
    "Sri Lanka",
    "Sweden",
    "Switzerland",
    "Taiwan",
    "Turkey",
    "Ukraine",
    "United Arab Emirates",
    "United Kingdom",
    "United States",
  ]

  // Populate country dropdown
  const countryItemsContainer = document.querySelector("#country-items .options-container-klf")
  countryItemsContainer.innerHTML = ""

  countries.forEach((country) => {
    const countryItem = document.createElement("div")
    countryItem.className = "select-item-klf"
    countryItem.dataset.value = country.toLowerCase().replace(/\s+/g, "-")
    countryItem.textContent = country
    countryItemsContainer.appendChild(countryItem)
  })

  // Get updated options after populating dropdown
  const updatedCountryOptions = document.querySelectorAll("#country-items .select-item-klf")
  const updatedProductOptions = document.querySelectorAll("#product-items .select-item-klf")

  // Country dropdown functionality
  const countrySelect = document.getElementById("selected-country")
  const countryItems = document.getElementById("country-items")
  const countrySearch = document.getElementById("country-search")

  // Product dropdown functionality
  const productSelect = document.getElementById("selected-product")
  const productItems = document.getElementById("product-items")

  // Company name filtering
  const companyNameInput = document.getElementById("company-name")
  companyNameInput.addEventListener("input", filterParticipants)

  // Country dropdown click event
  countrySelect.addEventListener("click", () => {
    countryItems.classList.toggle("select-hide-klf")
    resetSearch()
  })

  // Product dropdown click event
  productSelect.addEventListener("click", () => {
    productItems.classList.toggle("select-hide-klf")
  })

  // Click outside dropdown event
  document.addEventListener("click", (e) => {
    if (!e.target.closest(".custom-select-klf")) {
      countryItems.classList.add("select-hide-klf")
      productItems.classList.add("select-hide-klf")
    }
  })

  // Country options click event
  updatedCountryOptions.forEach((option) => {
    option.addEventListener("click", function () {
      countrySelect.textContent = this.textContent
      countryItems.classList.add("select-hide-klf")
      filterParticipants()
    })
  })

  // Product options click event
  updatedProductOptions.forEach((option) => {
    option.addEventListener("click", function () {
      productSelect.textContent = this.textContent
      productItems.classList.add("select-hide-klf")
      filterParticipants()
    })
  })

  // Country search functionality
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

  // Reset search
  function resetSearch() {
    countrySearch.value = ""
    updatedCountryOptions.forEach((option) => {
      option.style.display = ""
    })
  }

  // Filtering function
  function filterParticipants() {
    const companyNameFilter = companyNameInput.value.toLowerCase()
    const countryFilter = countrySelect.textContent
    const productFilter = productSelect.textContent

    const filteredData = participantsData.filter((participant) => {
      const companyNameMatch = participant.companyName.toLowerCase().includes(companyNameFilter)
      const countryMatch = countryFilter === "Turkey" ? true : participant.country === countryFilter

      // Product group filtering
      const productMatch = productFilter === "Crane Systems" ? true : participant.productGroup === productFilter

      return companyNameMatch && countryMatch && productMatch
    })

    populateTable(filteredData)
  }

  // Table population function
  function populateTable(data) {
    const tableBody = document.querySelector("#participants-table tbody")
    tableBody.innerHTML = ""

    if (data.length === 0) {
      const row = document.createElement("tr")
      const cell = document.createElement("td")
      cell.colSpan = 4
      cell.textContent = "No results found"
      cell.style.textAlign = "center"
      row.appendChild(cell)
      tableBody.appendChild(row)
      return
    }

    data.forEach((participant) => {
      const row = document.createElement("tr")

      const companyNameCell = document.createElement("td")
      companyNameCell.textContent = participant.companyName

      const countryCell = document.createElement("td")
      countryCell.textContent = participant.country

      const hallCell = document.createElement("td")
      hallCell.textContent = participant.hall

      const standNoCell = document.createElement("td")
      standNoCell.textContent = participant.standNo

      row.appendChild(companyNameCell)
      row.appendChild(countryCell)
      row.appendChild(hallCell)
      row.appendChild(standNoCell)

      tableBody.appendChild(row)
    })
  }
})
