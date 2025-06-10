document.addEventListener("DOMContentLoaded", () => {
  const currentPath = window.location.pathname;
  
  document.querySelectorAll(".sidebar .nav-link").forEach(link => {
    link.classList.toggle("active", link.getAttribute("href") === currentPath);
    
    if (link.classList.contains("active")) {
      const submenu = link.closest(".collapse");
      if (submenu) submenu.classList.add("show");
    }
  });

  // Sidebar mobile toggle functionality
  const sidebarToggleButton = document.querySelector('.sidebar-toggle-button')
  const sidebar = document.querySelector('.sidebar')

  if (sidebarToggleButton && sidebar) {
    sidebarToggleButton.addEventListener('click', () => {
      sidebar.classList.toggle('active')
    })
  }
})
