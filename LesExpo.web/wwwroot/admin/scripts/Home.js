// Initialize page
document.addEventListener('DOMContentLoaded', function() {
    initializeCounters();
    loadSystemInfo();
    // startRealTimeUpdates(); // Automatic refresh disabled
    addInteractiveEffects();
});

// Counter Animation
function animateCounter(element, target) {
    let current = 0;
    const increment = target / 50;
    const timer = setInterval(() => {
        current += increment;
        if (current >= target) {
            current = target;
            clearInterval(timer);
        }
        element.textContent = Math.floor(current);
    }, 30);
}

// Initialize counters
function initializeCounters() {
    const counters = document.querySelectorAll('.stat-number');
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const target = parseInt(entry.target.getAttribute('data-target'));
                animateCounter(entry.target, target);
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    counters.forEach(counter => {
        observer.observe(counter);
    });
}

// Load system information
async function loadSystemInfo() {
    if (!window.getSystemInfoUrl) {
        console.error('System info URL is not defined.');
        return;
    }
    try {
        const response = await fetch(window.getSystemInfoUrl);
        const data = await response.json();
        
        document.getElementById('serverTime').textContent = data.serverTime;
        
        const systemInfoGrid = document.getElementById('systemInfoGrid');
        // Use the info-item class from the main style.css
        systemInfoGrid.innerHTML = `
            <div class="info-item">
                <span class="info-label">Ortam</span>
                <span class="info-value">${data.environment}</span>
            </div>
            <div class="info-item">
                <span class="info-label">.NET Sürümü</span>
                <span class="info-value">${data.dotNetVersion}</span>
            </div>
            <div class="info-item">
                <span class="info-label">İşlemci Sayısı</span>
                <span class="info-value">${data.processorCount}</span>
            </div>
            <div class="info-item">
                <span class="info-label">Makine Adı</span>
                <span class="info-value">${data.machineName}</span>
            </div>
        `;
        
    } catch (error) {
        console.error('Sistem bilgileri yüklenirken hata:', error);
    }
}

// Refresh dashboard data
async function refreshDashboard() {
    if (!window.getDashboardStatsUrl) {
        console.error('Dashboard stats URL is not defined.');
        return;
    }
    const refreshBtn = document.querySelector('.refresh-btn');
    refreshBtn.style.transform = 'rotate(360deg)';
    
    try {
        const response = await fetch(window.getDashboardStatsUrl);
        const data = await response.json();
        
        // Update counters
        document.querySelector('.blogs .stat-number').setAttribute('data-target', data.totalBlogs);
        document.querySelector('.sliders .stat-number').setAttribute('data-target', data.totalSliders);
        document.querySelector('.pages .stat-number').setAttribute('data-target', data.totalContacts);
        document.querySelector('.content .stat-number').setAttribute('data-target', data.totalContentTypes);
        document.querySelector('.tickets .stat-number').setAttribute('data-target', data.totalTickets);
        document.querySelector('.registrations .stat-number').setAttribute('data-target', data.totalRegistrations);
        
        // Re-animate counters
        initializeCounters();
        
        // Update system info
        await loadSystemInfo();
        
        // Show success message
        showNotification('Dashboard başarıyla güncellendi!', 'success');
        
    } catch (error) {
        console.error('Dashboard yenilenirken hata:', error);
        showNotification('Güncelleme sırasında hata oluştu!', 'error');
    }
    
    setTimeout(() => {
        refreshBtn.style.transform = 'rotate(0deg)';
    }, 300);
}

// Real-time updates
function startRealTimeUpdates() {
    // Update server time every minute
    setInterval(() => {
        loadSystemInfo();
    }, 60000);
}

// Interactive effects
function addInteractiveEffects() {
    document.querySelectorAll('.stat-card').forEach(card => {
        card.addEventListener('click', createRipple);
    });
}

// Ripple effect
function createRipple(event) {
    const button = event.currentTarget;
    const circle = document.createElement("span");
    const diameter = Math.max(button.clientWidth, button.clientHeight);
    const radius = diameter / 2;

    circle.style.width = circle.style.height = `${diameter}px`;
    circle.style.left = `${event.clientX - button.offsetLeft - radius}px`;
    circle.style.top = `${event.clientY - button.offsetTop - radius}px`;
    circle.classList.add("ripple");

    const ripple = button.getElementsByClassName("ripple")[0];
    if (ripple) {
        ripple.remove();
    }

    button.appendChild(circle);
}

// Show notification
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    
    let bgColor = type === 'success' ? 'var(--ca-success)' : 'var(--ca-danger)';
    let icon = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-triangle';
    
    notification.innerHTML = `
        <div style="
            position: fixed;
            top: 20px;
            right: 20px;
            background: ${bgColor};
            color: white;
            padding: 1rem;
            border-radius: var(--ca-radius-lg);
            box-shadow: var(--ca-shadow-md);
            z-index: 9999;
            display: flex;
            align-items: center;
            gap: 0.75rem;
            animation: slideInRight 0.3s ease-out;
            font-weight: 500;
        ">
            <i class="fas ${icon}"></i>
            <span>${message}</span>
        </div>
    `;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'fadeOutRight 0.3s ease-in';
        notification.addEventListener('animationend', () => notification.remove());
    }, 3000);
}

// Add ripple CSS and other dynamic styles
const dynamicStyles = document.createElement('style');
dynamicStyles.textContent = `
    .ripple {
        position: absolute;
        border-radius: 50%;
        transform: scale(0);
        animation: ripple 600ms linear;
        background-color: rgba(0, 0, 0, 0.1);
        pointer-events: none;
    }
    
    @keyframes ripple {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    @keyframes slideInRight {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    
    @keyframes fadeOutRight {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(dynamicStyles);