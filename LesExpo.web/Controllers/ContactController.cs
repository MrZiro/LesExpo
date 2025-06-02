using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {

            return View("Iletisim");
        }
    }
}
