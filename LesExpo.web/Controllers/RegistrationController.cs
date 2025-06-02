using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
