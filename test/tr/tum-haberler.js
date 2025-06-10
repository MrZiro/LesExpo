// Sample card data
const initialCards = [
  {
    id: 1,
    image:
      "./images/ESTAblog1.jpeg",
    date: "06",
    category: "Duyurular",
    title: "ESTA 2025 MÜKEMMELLİK ÖDÜLLERİ SAHİPLERİNİ BULDU!",
    text: "...",
    link: "/tr/ornek-blog-1.html",
  },
  {
    id: 2,
    image:
      "./images/blog2.jpg",
    date: "06",
    category: "Duyurular",
    title: "Yük Güvenliği: Aşırı Yüklenmenin Yapılar Üzerindeki Tehlikeleri",
    text: "...",
    link: "/tr/blog-single-page.html",
  },
  {
    id: 3,
    image:
      "./images/blog3.jpg",
    date: "28",
    category: "Gündem",
    title: "Yük Analizinde Yapılan Yaygın Hatalar ve Önlemler",
    text: "...",
    link: "/tr/blog-single-page.html",
  },
  {
    id: 4,
    image:
      "./images/blog4.jpg",
    date: "23",
    category: "Blog",
    title: "Sürdürülebilir Yapılar İçin Yük Tasarımında Yeni Yaklaşımlar",
    text: "...",
    link: "/tr/blog-single-page.html",
  },
  {
    id: 5,
    image:
      "./images/blog5.jpg",
    date: "23",
    category: "Haberler",
    title: 'Depreme Dayanıklı Yapılar İçin Yük Hesaplamaları Nasıl Yapılır?',
    text: "...",
    link: "/tr/blog-single-page.html",
  },
  {
    id: 6,
    image:
      "./images/blog6.jpg",
    date: "23",
    category: "Haberler",
    title: "Yük Güvenliği: Aşırı Yüklenmenin Yapılar Üzerindeki Tehlikeleri",
    text: "...",
    link: "/tr/blog-single-page.html",
  },
]

// Current active filter
let currentFilter = "all"

// Function to create a card element
function createCard(cardData) {
  const card = document.createElement("div")
  card.className = "card-blogs"
  card.setAttribute("data-id", cardData.id)
  card.setAttribute("data-category", cardData.category)

  // Use the custom link if provided, otherwise fallback to the default pattern
  const cardLink = cardData.link || `/detail/${cardData.id}`
  card.setAttribute("data-url", cardLink)

  card.innerHTML = `
        <div class="card-image-container-blogs">
            <img src="${cardData.image}" alt="${cardData.title}" class="card-image-blogs">
            <div class="card-date-blogs">${cardData.date}</div>
        </div>
        <div class="card-content-blogs">
            <div class="card-category-blogs">${cardData.category}</div>
            <h3 class="card-title-blogs">${cardData.title}</h3>   
            </div>
        </div>
    `
  // Add click event listener to the card
  card.addEventListener("click", function (e) {
    // Stop event propagation to prevent other elements from receiving the click
    e.stopPropagation()

    const url = this.getAttribute("data-url")
    // Redirect to the detail page
    window.location.href = url
  })

  return card
}

// Function to filter cards by category
function filterCards(category) {
  currentFilter = category

  // Update active button
  const buttons = document.querySelectorAll(".filter-button-blogs")
  buttons.forEach((button) => {
    if (button.getAttribute("data-category") === category) {
      button.classList.add("active-blogs")
    } else {
      button.classList.remove("active-blogs")
    }
  })

  // Filter and render cards
  if (category === "all") {
    renderCards(initialCards)
  } else {
    const filteredCards = initialCards.filter((card) => card.category === category)
    renderCards(filteredCards)
  }
}

// Function to render all cards
function renderCards(cards) {
  const cardGrid = document.getElementById("cardGrid-blogs")
  cardGrid.innerHTML = "" // Clear existing cards

  if (cards.length === 0) {
    // Display a message if no cards match the filter
    const noResults = document.createElement("div")
    noResults.className = "no-results-blogs"
    noResults.textContent = "Bu kategoride henüz içerik bulunmamaktadır."
    cardGrid.appendChild(noResults)
    return
  }

  cards.forEach((cardData) => {
    const card = createCard(cardData)
    cardGrid.appendChild(card)
  })
}

// Function to add a new card
function addCard(cardData) {
  // Generate a new ID if not provided
  if (!cardData.id) {
    cardData.id = Date.now()
  }

  initialCards.push(cardData)

  // If we're showing all cards or this card matches the current filter, re-render
  if (currentFilter === "all" || cardData.category === currentFilter) {
    filterCards(currentFilter)
  }

  return cardData.id // Return the ID of the new card
}

// Function to update an existing card
function updateCard(id, updatedData) {
  const index = initialCards.findIndex((card) => card.id === id)

  if (index !== -1) {
    initialCards[index] = { ...initialCards[index], ...updatedData }
    filterCards(currentFilter)
    return true
  }

  return false
}

// Function to delete a card
function deleteCard(id) {
  const index = initialCards.findIndex((card) => card.id === id)

  if (index !== -1) {
    initialCards.splice(index, 1)
    filterCards(currentFilter)
    return true
  }

  return false
}

// Example of how to add a new card with a custom link
function addNewCard() {
  const newCard = {
    image:
      "https://hebbkx1anhila5yf.public.blob.vercel-storage.com/Ekran%20g%C3%B6r%C3%BCnt%C3%BCs%C3%BC%202025-05-13%20132125-nP40jDkGttrnbERZLfFx5lBCwSGjW0.png",
    date: "30",
    category: "Blog",
    title: "Yeni Etkinlik: Teknoloji Fuarı 2025 İstanbul'da Gerçekleşecek!",
    text: "Teknoloji dünyasının en yeni gelişmeleri bu fuarda sergilenecek...",
    link: "/etkinlikler/teknoloji-fuari-2025",
  }

  addCard(newCard)
}

// Initialize the page with cards and add click handlers
document.addEventListener("DOMContentLoaded", () => {
  // Set up filter button click handlers
  const filterButtons = document.querySelectorAll(".filter-button-blogs")
  filterButtons.forEach((button) => {
    button.addEventListener("click", function () {
      const category = this.getAttribute("data-category")
      filterCards(category)
    })
  })

  // Initial render of all cards
  renderCards(initialCards)
})

// Make functions available globally
window.addCard = addCard
window.updateCard = updateCard
window.deleteCard = deleteCard
window.filterCards = filterCards
