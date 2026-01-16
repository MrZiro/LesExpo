using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class SearchController : Controller
    {
        private readonly IContentIndexService _contentIndexService;
        private readonly ILogger<SearchController> _logger;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public SearchController(IContentIndexService contentIndexService, ILogger<SearchController> logger)
        {
            _contentIndexService = contentIndexService;
            _logger = logger;
        }

        [HttpGet("search")]
        [HttpGet("arama")]
        public IActionResult Index(string q = "")
        {
            ViewBag.SearchTerm = q;
            ViewBag.Language = Lang;
            ViewData["Title"] = Lang == "en" ? "Search" : "Arama";
            return View();
        }

        [HttpGet("api/search-index")]
        public async Task<IActionResult> GetSearchIndex(string q = "")
        {
            try
            {
                var searchIndex = await _contentIndexService.GetSearchIndexAsync(Lang, q);
                return Json(searchIndex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating search index for language: {Language}", Lang);
                return Json(new { error = "Failed to load search index", results = new object[0] });
            }
        }

        [HttpGet("api/search-test")]
        public IActionResult TestSearch(string q = "test")
        {
            // Test endpoint to verify search content
            try
            {
                var testContent = _contentIndexService.ExtractTextFromView($"Views/{Lang}/AboutUs/exhibitionidentification.cshtml");
                return Json(new { 
                    query = q,
                    language = Lang,
                    sampleContent = testContent.Substring(0, Math.Min(500, testContent.Length)),
                    contentLength = testContent.Length,
                    containsBaslangic = testContent.ToLower().Contains("başlangıç"),
                    containsTarihi = testContent.ToLower().Contains("tarihi")
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
} 