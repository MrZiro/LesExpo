using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using HtmlAgilityPack;


namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetDashboardData();
            return View(model);
        }

        private async Task<DashboardVM> GetDashboardData()
        {
            // Get current counts
            var totalBlogs = _unitOfWork.Blog.GetCount();
            var totalSliders = _unitOfWork.Slider.GetCount();
            var totalContentTypes = _unitOfWork.ContentType.GetCount();
            var totalContacts = _unitOfWork.Contact.GetCount();
            var totalTickets = _unitOfWork.Ticket.GetCount();
            var totalRegistrations = _unitOfWork.Registration.GetCount();

            // Get additional statistics
            var allBlogs = _unitOfWork.Blog.GetAll();
            var publishedBlogs = allBlogs.Count(b => b.IsPublished);
            var unpublishedBlogs = allBlogs.Count(b => !b.IsPublished);
            
            var allSliders = _unitOfWork.Slider.GetAll();
            var activeSliders = allSliders.Count(s => s.IsActive);
            
            // Get tickets with API status
            var allTickets = _unitOfWork.Ticket.GetAll();
            var successfulTickets = allTickets.Count(t => t.ApiSuccess);
            var failedTickets = allTickets.Count(t => !t.ApiSuccess);
            
            var currentMonth = DateTime.Now;
            
            // Monthly statistics for current month
            var thisMonthContacts = _unitOfWork.Contact.GetAll()
                .Count(c => c.CreatedAt.Year == currentMonth.Year && c.CreatedAt.Month == currentMonth.Month);
            
            var thisMonthTickets = allTickets
                .Count(t => t.CreatedAt.Year == currentMonth.Year && t.CreatedAt.Month == currentMonth.Month);
            
            var thisMonthRegistrations = _unitOfWork.Registration.GetAll()
                .Count(r => r.CreatedAt.Year == currentMonth.Year && r.CreatedAt.Month == currentMonth.Month);
            
            var thisMonthBlogs = allBlogs
                .Count(b => b.CreatedAt.Year == currentMonth.Year && b.CreatedAt.Month == currentMonth.Month);

            // Get monthly statistics (last 6 months)
            var monthlyBlogStats = GetMonthlyBlogStats();
            var monthlyTicketStats = GetMonthlyTicketStats();
            var monthlyRegistrationStats = GetMonthlyRegistrationStats();
            var monthlyContactStats = GetMonthlyContactStats();
            
            // Get category statistics based on content types
            var categoryStats = GetCategoryStats();
            
            // Get country and sector statistics
            var countryStats = GetCountryStats();
            var sectorStats = GetSectorStats();
            
            // Get recent activities
            var recentActivities = await GetRecentActivities();

            var model = new DashboardVM
            {
                // Main statistics
                TotalBlogs = totalBlogs,
                TotalSliders = totalSliders,
                TotalPages = totalContacts, // Using contacts as pages for backward compatibility
                TotalIcerikTuru = totalContentTypes,
                TotalTickets = totalTickets,
                TotalRegistrations = totalRegistrations,
                TotalContacts = totalContacts,
                
                // Additional statistics
                PublishedBlogs = publishedBlogs,
                UnpublishedBlogs = unpublishedBlogs,
                ActiveSliders = activeSliders,
                RecentContactsCount = thisMonthContacts,
                ThisMonthBlogsCount = thisMonthBlogs,
                
                // Ticket statistics
                SuccessfulTickets = successfulTickets,
                FailedTickets = failedTickets,
                ThisMonthTicketsCount = thisMonthTickets,
                
                // Registration and Contact statistics
                ThisMonthRegistrationsCount = thisMonthRegistrations,
                ThisMonthContactsCount = thisMonthContacts,
                
                // Monthly trends
                MonthlyBlogStats = monthlyBlogStats,
                MonthlyTicketStats = monthlyTicketStats,
                MonthlyRegistrationStats = monthlyRegistrationStats,
                MonthlyContactStats = monthlyContactStats,
                
                // Category and geographic statistics
                CategoryStats = categoryStats,
                CountryStats = countryStats,
                SectorStats = sectorStats,
                RecentActivities = recentActivities
            };

            return model;
        }

        private List<MonthlyStats> GetMonthlyBlogStats()
        {
            var blogs = _unitOfWork.Blog.GetAll();
            var monthlyStats = new List<MonthlyStats>();
            
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddMonths(-i);
                var monthName = GetTurkishMonthName(targetDate.Month);
                var count = blogs.Count(b => b.CreatedAt.Year == targetDate.Year && 
                                           b.CreatedAt.Month == targetDate.Month);
                
                monthlyStats.Add(new MonthlyStats { Month = monthName, Count = count });
            }

            return monthlyStats;
        }

        private List<CategoryStats> GetCategoryStats()
        {
            var contentTypes = _unitOfWork.ContentType.GetAll().ToList();
            var blogs = _unitOfWork.Blog.GetAll("ContentType").ToList();
            
            var categoryStats = new List<CategoryStats>();
            var colors = new[] { "#334155", "#FBAD18", "#14b8a6", "#0ea5e9", "#EF4444", "#64748b", "#94a3b8" };
            
            for (int i = 0; i < contentTypes.Count && i < colors.Length; i++)
            {
                var contentType = contentTypes[i];
                var count = blogs.Count(b => b.ContentTypeId == contentType.Id);
                
                categoryStats.Add(new CategoryStats
                {
                    Category = contentType.Name,
                    Count = count,
                    Color = colors[i]
                });
            }

            // Add "Other" category for remaining items if needed
            if (contentTypes.Count == 0)
            {
                categoryStats.Add(new CategoryStats
                {
                    Category = "Henüz içerik türü yok",
                    Count = 0,
                    Color = "#e2e8f0"
                });
            }

            return categoryStats;
        }

        private List<MonthlyStats> GetMonthlyTicketStats()
        {
            var tickets = _unitOfWork.Ticket.GetAll();
            var monthlyStats = new List<MonthlyStats>();
            
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddMonths(-i);
                var monthName = GetTurkishMonthName(targetDate.Month);
                var count = tickets.Count(t => t.CreatedAt.Year == targetDate.Year && 
                                           t.CreatedAt.Month == targetDate.Month);
                
                monthlyStats.Add(new MonthlyStats { Month = monthName, Count = count });
            }

            return monthlyStats;
        }

        private List<MonthlyStats> GetMonthlyRegistrationStats()
        {
            var registrations = _unitOfWork.Registration.GetAll();
            var monthlyStats = new List<MonthlyStats>();
            
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddMonths(-i);
                var monthName = GetTurkishMonthName(targetDate.Month);
                var count = registrations.Count(r => r.CreatedAt.Year == targetDate.Year && 
                                               r.CreatedAt.Month == targetDate.Month);
                
                monthlyStats.Add(new MonthlyStats { Month = monthName, Count = count });
            }

            return monthlyStats;
        }

        private List<MonthlyStats> GetMonthlyContactStats()
        {
            var contacts = _unitOfWork.Contact.GetAll();
            var monthlyStats = new List<MonthlyStats>();
            
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddMonths(-i);
                var monthName = GetTurkishMonthName(targetDate.Month);
                var count = contacts.Count(c => c.CreatedAt.Year == targetDate.Year && 
                                           c.CreatedAt.Month == targetDate.Month);
                
                monthlyStats.Add(new MonthlyStats { Month = monthName, Count = count });
            }

            return monthlyStats;
        }

        private List<CategoryStats> GetCountryStats()
        {
            var tickets = _unitOfWork.Ticket.GetAll().ToList();
            var registrations = _unitOfWork.Registration.GetAll().ToList();
            
            var countryStats = new Dictionary<string, int>();
            
            // Count from tickets
            foreach (var ticket in tickets)
            {
                if (!string.IsNullOrEmpty(ticket.Country))
                {
                    countryStats[ticket.Country] = countryStats.GetValueOrDefault(ticket.Country, 0) + 1;
                }
            }
            
            // Count from registrations
            foreach (var registration in registrations)
            {
                if (!string.IsNullOrEmpty(registration.Ulke))
                {
                    countryStats[registration.Ulke] = countryStats.GetValueOrDefault(registration.Ulke, 0) + 1;
                }
            }
            
            var colors = new[] { "#334155", "#FBAD18", "#14b8a6", "#0ea5e9", "#EF4444", "#64748b", "#94a3b8" };
            var result = new List<CategoryStats>();
            
            var topCountries = countryStats.OrderByDescending(x => x.Value).Take(7).ToList();
            for (int i = 0; i < topCountries.Count; i++)
            {
                result.Add(new CategoryStats
                {
                    Category = topCountries[i].Key,
                    Count = topCountries[i].Value,
                    Color = colors[i % colors.Length]
                });
            }
            
            return result;
        }

        private List<CategoryStats> GetSectorStats()
        {
            var tickets = _unitOfWork.Ticket.GetAll().ToList();
            var registrations = _unitOfWork.Registration.GetAll().ToList();
            
            var sectorStats = new Dictionary<string, int>();
            
            // Count from tickets
            foreach (var ticket in tickets)
            {
                if (!string.IsNullOrEmpty(ticket.Sector))
                {
                    sectorStats[ticket.Sector] = sectorStats.GetValueOrDefault(ticket.Sector, 0) + 1;
                }
            }
            
            // Count from registrations (using FaaliyetAlani as sector)
            foreach (var registration in registrations)
            {
                if (!string.IsNullOrEmpty(registration.FaaliyetAlani))
                {
                    sectorStats[registration.FaaliyetAlani] = sectorStats.GetValueOrDefault(registration.FaaliyetAlani, 0) + 1;
                }
            }
            
            var colors = new[] { "#334155", "#FBAD18", "#14b8a6", "#0ea5e9", "#EF4444", "#64748b", "#94a3b8" };
            var result = new List<CategoryStats>();
            
            var topSectors = sectorStats.OrderByDescending(x => x.Value).Take(7).ToList();
            for (int i = 0; i < topSectors.Count; i++)
            {
                result.Add(new CategoryStats
                {
                    Category = topSectors[i].Key,
                    Count = topSectors[i].Value,
                    Color = colors[i % colors.Length]
                });
            }
            
            return result;
        }

        private async Task<List<RecentActivity>> GetRecentActivities()
        {
            var activities = new List<RecentActivity>();
            
            // Get recent blogs
            var recentBlogs = _unitOfWork.Blog.GetAll()
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToList();
            
            foreach (var blog in recentBlogs)
            {
                activities.Add(new RecentActivity
                {
                    Action = $"'{blog.Title}' blog yazısı eklendi",
                    User = blog.Author ?? "System",
                    Date = blog.CreatedAt,
                    Type = "blog"
                });
            }

            // Get recent tickets
            var recentTickets = _unitOfWork.Ticket.GetAll()
                .OrderByDescending(t => t.CreatedAt)
                .Take(2)
                .ToList();
            
            foreach (var ticket in recentTickets)
            {
                activities.Add(new RecentActivity
                {
                    Action = $"Yeni bilet başvurusu: {ticket.FirstName} {ticket.LastName}",
                    User = $"{ticket.FirstName} {ticket.LastName}",
                    Date = ticket.CreatedAt,
                    Type = "ticket"
                });
            }

            // Get recent registrations
            var recentRegistrations = _unitOfWork.Registration.GetAll()
                .OrderByDescending(r => r.CreatedAt)
                .Take(2)
                .ToList();
            
            foreach (var registration in recentRegistrations)
            {
                activities.Add(new RecentActivity
                {
                    Action = $"Yeni fuar kayıt: {registration.SirketAdi}",
                    User = registration.AdSoyad,
                    Date = registration.CreatedAt,
                    Type = "registration"
                });
            }

            // Get recent contacts
            var recentContacts = _unitOfWork.Contact.GetAll()
                .OrderByDescending(c => c.CreatedAt)
                .Take(2)
                .ToList();
            
            foreach (var contact in recentContacts)
            {
                activities.Add(new RecentActivity
                {
                    Action = $"Yeni iletişim: {contact.Subject}",
                    User = contact.Name,
                    Date = contact.CreatedAt,
                    Type = "contact"
                });
            }

            return activities.OrderByDescending(a => a.Date).Take(15).ToList();
        }

        private string GetTurkishMonthName(int month)
        {
            return month switch
            {
                1 => "Ocak",
                2 => "Şubat",
                3 => "Mart",
                4 => "Nisan",
                5 => "Mayıs",
                6 => "Haziran",
                7 => "Temmuz",
                8 => "Ağustos",
                9 => "Eylül",
                10 => "Ekim",
                11 => "Kasım",
                12 => "Aralık",
                _ => "Bilinmiyor"
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var model = await GetDashboardData();
            
            return Json(new
            {
                totalBlogs = model.TotalBlogs,
                totalSliders = model.TotalSliders,
                totalContacts = model.TotalContacts,
                totalContentTypes = model.TotalIcerikTuru,
                totalTickets = model.TotalTickets,
                totalRegistrations = model.TotalRegistrations,
                monthlyBlogStats = model.MonthlyBlogStats,
                monthlyTicketStats = model.MonthlyTicketStats,
                monthlyRegistrationStats = model.MonthlyRegistrationStats,
                monthlyContactStats = model.MonthlyContactStats,
                categoryStats = model.CategoryStats,
                countryStats = model.CountryStats,
                sectorStats = model.SectorStats,
                recentActivities = model.RecentActivities
            });
        }

        [HttpGet]
        public IActionResult GetSystemInfo()
        {
            var systemInfo = new
            {
                serverTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                uptime = Environment.TickCount64,
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                dotNetVersion = Environment.Version.ToString(),
                machineName = Environment.MachineName,
                osVersion = Environment.OSVersion.ToString(),
                processorCount = Environment.ProcessorCount
            };

            return Json(systemInfo);
        }
    }
}
