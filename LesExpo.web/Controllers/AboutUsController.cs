using AspNetCoreGeneratedDocument;
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
        [HttpGet("visitor-profile")]
        [HttpGet("ziyaretci-profili")]
        public IActionResult VisitorProfile()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "WhyVisiting", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "WhyVisiting", Lang);
            return View();
        }
        [HttpGet("why-visit")]
        [HttpGet("neden-ziyaret-etmelisiniz")]
        public IActionResult WhyVisiting()
        {
            return View();
        }
        [HttpGet("faq")]
        [HttpGet("sikca-sorulan-sorular")]
        public IActionResult faq()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "Insights", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "Insights", Lang);
            return View();
        }
        [HttpGet("b2b-toplantilari")]
        [HttpGet("b2b-meetings")]
        public IActionResult B2BMeeting()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "B2BMeeting", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "B2BMeeting", Lang);
            return View();
        }
        [HttpGet("les-insights")]
        public IActionResult Insights()
        {
            return View();
        }
        [HttpGet("les-experiences")]
        public IActionResult Experiences()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "WhyExhibit", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "WhyExhibit", Lang);
            return View();
        }
        [HttpGet("medya-logo")]
        [HttpGet("media-logo")]
        public IActionResult MediaLogo()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "MediaLogo", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "MediaLogo", Lang);
            return View();
        }
        [HttpGet("exhibitor-profile")]
        [HttpGet("katilimci-profili")]
        public IActionResult ExhibitorProfile()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "ExhibitorProfile", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "ExhibitorProfile", Lang);
            return View();
        }
        [HttpGet("why-exhibit")]
        [HttpGet("neden-katilmalisiniz")]
        public IActionResult WhyExhibit()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("AboutUs", "WhyExhibit", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("AboutUs", "WhyExhibit", Lang);
            return View();
        }
        
        
    }
}
