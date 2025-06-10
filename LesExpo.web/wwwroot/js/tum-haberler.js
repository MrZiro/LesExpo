document.addEventListener("DOMContentLoaded", () => {

  const lang = getLangFromUrl();
  const cardDataUrl = `/${lang}/Blogs/GetAllBlogs`;
  const cardGrid = document.getElementById("cardGrid-blogs");
  let allCards = [];
  let currentFilter = "all";

  const createCard = ({ image, date, category, title, slug }) => {
    const card = document.createElement("div");
    card.className = "card-blogs";
    card.setAttribute("data-category", category);

    if (lang === "en") {
      card.setAttribute("data-url", `/${lang}/blog-detail/${slug}`);
    } else {
      card.setAttribute("data-url", `/${lang}/blog-detay/${slug}`);
    }

    card.innerHTML = `
      <div class="card-image-container-blogs">
        <img src="${image}" alt="${title}" class="card-image-blogs">
        <div class="card-date-blogs">${date}</div>
      </div>
      <div class="card-content-blogs">
        <div class="card-category-blogs">${category}</div>
        <h3 class="card-title-blogs">${title}</h3>
      </div>
    `;

    card.addEventListener("click", () =>
      window.location.href = card.getAttribute("data-url")
    );

    return card;
  };

  const renderCards = (cards) => {
    cardGrid.innerHTML = "";
    if (!cards.length) {
      if (lang === "en") {
        cardGrid.innerHTML = `<div class="no-results-blogs">There is no content in this category.</div>`;
      } else {
        cardGrid.innerHTML = `<div class="no-results-blogs">Bu kategoride henüz içerik bulunmamaktadır.</div>`;
      }
      return;
    }
    cards.forEach(card => cardGrid.appendChild(createCard(card)));
  };

  const filterCards = (category) => {
    currentFilter = category;
    document.querySelectorAll(".filter-button-blogs").forEach(btn => {
      btn.classList.toggle("active-blogs", btn.dataset.category === category);
    });
    const filtered = category === "all"
      ? allCards
      : allCards.filter(card => card.category === category);
    renderCards(filtered);
  };

  const fetchCards = async () => {
    try {
      const res = await fetch(cardDataUrl);
      allCards = await res.json();
      renderCards(allCards);
    } catch (err) {
      console.error("Blog verileri alınamadı:", err);
    }
  };

  // Kategori filtrelerini hazırla
  document.querySelectorAll(".filter-button-blogs").forEach(btn => {
    btn.addEventListener("click", () => filterCards(btn.dataset.category));
  });

  fetchCards();
});



function getLangFromUrl() {
  const path = window.location.pathname; // Gets the path part of the URL, e.g., "/en/home/index"
  const segments = path.split('/');      // Splits the path into an array: ["", "en", "home", "index"]

  // The 'lang' segment will be at index 1 if the URL structure is consistent
  if (segments.length > 1) {
    return segments[1];
  }

  return null; // Or a default language if 'lang' is not found
}