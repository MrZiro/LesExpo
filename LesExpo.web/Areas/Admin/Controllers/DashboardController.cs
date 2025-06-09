//using LesExpo.DataAccess.Repository.IRepository;
//using LesExpo.Models;
//using LesExpo.Models.ViewModels;
//using LesExpo.Utility;
//using LesExpo.web.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Hosting;
//using System.IO;
//using HtmlAgilityPack;

//namespace LesExpo.web.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = SD.Role_Admin)]
//    public class DashboardController : Controller
//    {
//        public ActionResult Index()
//        {
//            // Get your actual data from database/services
//            ViewBag.BlogCount = "20";
//            ViewBag.UserCount = "40";
//            ViewBag.SliderCount = "50";
//            ViewBag.PageCount = "30";

//            // Sample recent activities (replace with actual data)
//            ViewBag.RecentActivities = new List<ActivityLog> {
//            new ActivityLog { UserName = "Admin", Action = "Created", Item = "Blog Post", Timestamp = DateTime.Now.AddHours(-1) },
//            new ActivityLog { UserName = "Editor", Action = "Updated", Item = "Home Page", Timestamp = DateTime.Now.AddHours(-2) }
//            };

//            return View();
//        }

//        public class ActivityLog
//        {
//            public string UserName { get; set; }
//            public string Action { get; set; }
//            public string Item { get; set; }
//            public DateTime Timestamp { get; set; }
//        }
//    }
//}