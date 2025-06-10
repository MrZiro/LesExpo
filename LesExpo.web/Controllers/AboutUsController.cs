using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class AboutUsController : Controller
    {
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        [HttpGet("about-us")]
        [HttpGet("hakkimizda")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("exhibition-identification")]
        [HttpGet("fuar-kunyesi")]
        public IActionResult ExhibitionIdentification()
        {

            return View();
        }

        [HttpGet("exhibition-area")]
        [HttpGet("fuar-alani")]
        public IActionResult ExhibitionArea()
        {

            return View();
        }

        [HttpGet("how-to-reach")]
        [HttpGet("ulasim")]
        public IActionResult HowToReach()
        {

            return View();
        }

        [HttpGet("show-report")]
        [HttpGet("fuar-sonuc-raporu")]
        public IActionResult ShowReport()
        {

            return View();
        }
    }
}
