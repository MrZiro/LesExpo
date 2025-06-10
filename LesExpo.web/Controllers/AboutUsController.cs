using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class AboutUsController : Controller
    {
        private readonly IUrlLocalizationService _urlService;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public AboutUsController(IUrlLocalizationService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("about-us")]
        [HttpGet("hakkimizda")]
        public IActionResult Index()
        {
            // Set SEO metadata
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "Index", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "Index", Lang);
            
            return View();
        }

        [HttpGet("exhibition-identification")]
        [HttpGet("fuar-kunyesi")]
        public IActionResult ExhibitionIdentification()
        {
            // Set SEO metadata
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "ExhibitionIdentification", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "ExhibitionIdentification", Lang);
            
            return View();
        }

        [HttpGet("exhibition-area")]
        [HttpGet("fuar-alani")]
        public IActionResult ExhibitionArea()
        {
            // Set SEO metadata
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "ExhibitionArea", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "ExhibitionArea", Lang);
            
            return View();
        }

        [HttpGet("how-to-reach")]
        [HttpGet("ulasim")]
        public IActionResult HowToReach()
        {
            // Set SEO metadata  
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "HowToReach", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "HowToReach", Lang);
            
            return View();
        }

        [HttpGet("show-report")]
        [HttpGet("fuar-sonuc-raporu")]
        public IActionResult ShowReport()
        {
            // Set SEO metadata
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "ShowReport", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "ShowReport", Lang);
            
            return View();
        }
    }
}
