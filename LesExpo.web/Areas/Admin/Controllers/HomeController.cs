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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetDashboardData();
            return View(model);
        }

        private async Task<DashboardVM> GetDashboardData()
        {
            // Get current counts
            var totalUsers = await _userManager.Users.CountAsync();
            var totalBlogs = _unitOfWork.Blog.GetCount();
            var totalSliders = _unitOfWork.Slider.GetCount();
            var totalContentTypes = _unitOfWork.ContentType.GetCount();
            var totalContacts = _unitOfWork.Contact.GetCount();

            // Get additional statistics
            var allBlogs = _unitOfWork.Blog.GetAll();
            var publishedBlogs = allBlogs.Count(b => b.IsPublished);
            var unpublishedBlogs = allBlogs.Count(b => !b.IsPublished);
            
            var allSliders = _unitOfWork.Slider.GetAll();
            var activeSliders = allSliders.Count(s => s.IsActive);
            
            var currentMonth = DateTime.Now;
            var thisMonthContacts = _unitOfWork.Contact.GetAll()
                .Count(c => c.CreatedAt.Year == currentMonth.Year && c.CreatedAt.Month == currentMonth.Month);
            
            var thisMonthBlogs = allBlogs
                .Count(b => b.CreatedAt.Year == currentMonth.Year && b.CreatedAt.Month == currentMonth.Month);
            
            var allUsers = await _userManager.Users.ToListAsync();
            var thisMonthUsers = allUsers
                .Count(u => u.CreatedAt.Year == currentMonth.Year && u.CreatedAt.Month == currentMonth.Month);

            // Get monthly blog statistics (last 6 months)
            var monthlyBlogStats = GetMonthlyBlogStats();
            
            // Get monthly user registration statistics (last 6 months)  
            var monthlyUserStats = await GetMonthlyUserStats();
            
            // Get category statistics based on content types
            var categoryStats = GetCategoryStats();
            
            // Get recent activities
            var recentActivities = await GetRecentActivities();

            var model = new DashboardVM
            {
                TotalUsers = totalUsers,
                TotalBlogs = totalBlogs,
                TotalSliders = totalSliders,
                TotalPages = totalContacts, // Using contacts as pages for now
                TotalIcerikTuru = totalContentTypes,
                
                // Additional statistics
                PublishedBlogs = publishedBlogs,
                UnpublishedBlogs = unpublishedBlogs,
                ActiveSliders = activeSliders,
                RecentContactsCount = thisMonthContacts,
                ThisMonthBlogsCount = thisMonthBlogs,
                ThisMonthUsersCount = thisMonthUsers,
                
                MonthlyBlogStats = monthlyBlogStats,
                MonthlyUserStats = monthlyUserStats,
                CategoryStats = categoryStats,
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

        private async Task<List<MonthlyStats>> GetMonthlyUserStats()
        {
            var users = await _userManager.Users.ToListAsync();
            var monthlyStats = new List<MonthlyStats>();
            
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = DateTime.Now.AddMonths(-i);
                var monthName = GetTurkishMonthName(targetDate.Month);
                var count = users.Count(u => u.CreatedAt.Year == targetDate.Year && 
                                           u.CreatedAt.Month == targetDate.Month);
                
                monthlyStats.Add(new MonthlyStats { Month = monthName, Count = count });
            }

            return monthlyStats;
        }

        private List<CategoryStats> GetCategoryStats()
        {
            var contentTypes = _unitOfWork.ContentType.GetAll().ToList();
            var blogs = _unitOfWork.Blog.GetAll("ContentType").ToList();
            
            var categoryStats = new List<CategoryStats>();
            var colors = new[] { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF", "#FF9F40", "#FF6384", "#C9CBCF" };
            
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
                    Color = "#C9CBCF"
                });
            }

            return categoryStats;
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

            // Get recent users
            var recentUsers = await _userManager.Users
                .OrderByDescending(u => u.CreatedAt)
                .Take(2)
                .ToListAsync();
            
            foreach (var user in recentUsers)
            {
                activities.Add(new RecentActivity
                {
                    Action = $"Yeni kullanıcı: {user.Name}",
                    User = "System",
                    Date = user.CreatedAt,
                    Type = "user"
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
                    Type = "page"
                });
            }

            return activities.OrderByDescending(a => a.Date).Take(10).ToList();
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
                totalUsers = model.TotalUsers,
                totalBlogs = model.TotalBlogs,
                totalSliders = model.TotalSliders,
                totalPages = model.TotalPages,
                totalContentTypes = model.TotalIcerikTuru,
                monthlyBlogStats = model.MonthlyBlogStats,
                monthlyUserStats = model.MonthlyUserStats,
                categoryStats = model.CategoryStats,
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
