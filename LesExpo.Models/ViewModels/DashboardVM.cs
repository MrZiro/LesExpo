using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models.ViewModels
{
    // View Models
    public class DashboardVM
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


}
