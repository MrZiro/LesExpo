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

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalUsers = 150,
                TotalBlogs = 25,
                TotalSliders = 8,
                TotalPages = 12,
                TotalIcerikTuru = 5,
                
                // Monthly statistics for charts
                MonthlyBlogStats = new List<MonthlyStats>
                {
                    new MonthlyStats { Month = "Ocak", Count = 5 },
                    new MonthlyStats { Month = "Şubat", Count = 8 },
                    new MonthlyStats { Month = "Mart", Count = 12 },
                    new MonthlyStats { Month = "Nisan", Count = 7 },
                    new MonthlyStats { Month = "Mayıs", Count = 15 },
                    new MonthlyStats { Month = "Haziran", Count = 10 }
                },
                
                MonthlyUserStats = new List<MonthlyStats>
                {
                    new MonthlyStats { Month = "Ocak", Count = 20 },
                    new MonthlyStats { Month = "Şubat", Count = 25 },
                    new MonthlyStats { Month = "Mart", Count = 30 },
                    new MonthlyStats { Month = "Nisan", Count = 18 },
                    new MonthlyStats { Month = "Mayıs", Count = 35 },
                    new MonthlyStats { Month = "Haziran", Count = 22 }
                },
                
                // Category distribution
                CategoryStats = new List<CategoryStats>
                {
                    new CategoryStats { Category = "Teknoloji", Count = 12, Color = "#FF6384" },
                    new CategoryStats { Category = "Sağlık", Count = 8, Color = "#36A2EB" },
                    new CategoryStats { Category = "Spor", Count = 15, Color = "#FFCE56" },
                    new CategoryStats { Category = "Eğitim", Count = 10, Color = "#4BC0C0" },
                    new CategoryStats { Category = "Diğer", Count = 5, Color = "#9966FF" }
                },
                
                // Recent activities
                RecentActivities = new List<RecentActivity>
                {
                    new RecentActivity { Action = "Yeni blog yazısı eklendi", User = "Admin", Date = DateTime.Now.AddHours(-2), Type = "blog" },
                    new RecentActivity { Action = "Slider güncellendi", User = "Editor", Date = DateTime.Now.AddHours(-5), Type = "slider" },
                    new RecentActivity { Action = "Yeni kullanıcı kaydı", User = "System", Date = DateTime.Now.AddHours(-8), Type = "user" },
                    new RecentActivity { Action = "Sayfa içeriği düzenlendi", User = "Admin", Date = DateTime.Now.AddDays(-1), Type = "page" },
                    new RecentActivity { Action = "İçerik türü eklendi", User = "Admin", Date = DateTime.Now.AddDays(-2), Type = "content" }
                }
            };
        
            return View(model);
        }
    }
}

// View Models
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalBlogs { get; set; }
        public int TotalSliders { get; set; }
        public int TotalPages { get; set; }
        public int TotalIcerikTuru { get; set; }
        
        public List<MonthlyStats> MonthlyBlogStats { get; set; } = new();
        public List<MonthlyStats> MonthlyUserStats { get; set; } = new();
        public List<CategoryStats> CategoryStats { get; set; } = new();
        public List<RecentActivity> RecentActivities { get; set; } = new();
    }

    public class MonthlyStats
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CategoryStats
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class RecentActivity
    {
        public string Action { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
    }

