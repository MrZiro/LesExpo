document.addEventListener("DOMContentLoaded", () => {
  const cardDataUrl = "/Blogs/GetAllBlogs";
  const cardGrid = document.getElementById("cardGrid-blogs");
  let allCards = [];
  let currentFilter = "all";

  const createCard = ({ image, date, category, title, slug }) => {
    const card = document.createElement("div");
    card.className = "card-blogs";
    card.setAttribute("data-category", category);
    card.setAttribute("data-url", `/Blog-Detay/${slug}`);

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
      cardGrid.innerHTML = `<div class="no-results-blogs">Bu kategoride henüz içerik bulunmamaktadır.</div>`;
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
