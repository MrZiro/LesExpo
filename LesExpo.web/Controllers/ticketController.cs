using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    public class ticketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
