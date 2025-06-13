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
        // Main statistics
        public int TotalBlogs { get; set; }
        public int TotalSliders { get; set; }
        public int TotalPages { get; set; }
        public int TotalIcerikTuru { get; set; }
        public int TotalTickets { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalContacts { get; set; }

        // Additional statistics
        public int PublishedBlogs { get; set; }
        public int UnpublishedBlogs { get; set; }
        public int ActiveSliders { get; set; }
        public int RecentContactsCount { get; set; }
        public int ThisMonthBlogsCount { get; set; }
        
        // Ticket statistics
        public int SuccessfulTickets { get; set; }
        public int FailedTickets { get; set; }
        public int ThisMonthTicketsCount { get; set; }
        
        // Registration statistics  
        public int ThisMonthRegistrationsCount { get; set; }
        
        // Contact statistics
        public int ThisMonthContactsCount { get; set; }

        public List<MonthlyStats> MonthlyBlogStats { get; set; } = new();
        public List<MonthlyStats> MonthlyTicketStats { get; set; } = new();
        public List<MonthlyStats> MonthlyRegistrationStats { get; set; } = new();
        public List<MonthlyStats> MonthlyContactStats { get; set; } = new();
        public List<CategoryStats> CategoryStats { get; set; } = new();
        public List<RecentActivity> RecentActivities { get; set; } = new();
        
        // Country statistics for registrations and tickets
        public List<CategoryStats> CountryStats { get; set; } = new();
        public List<CategoryStats> SectorStats { get; set; } = new();
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
