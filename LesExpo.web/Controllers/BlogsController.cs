using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class BlogsController : Controller
    {
        private readonly ILogger<BlogsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlLocalizationService _urlService;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public BlogsController(ILogger<BlogsController> logger, IUnitOfWork unitOfWork, IUrlLocalizationService urlService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _urlService = urlService;
        }
        
        [HttpGet("all-news")]
        [HttpGet("tum-haberler")]
        public IActionResult Index()
        {
            var ContentTypes = _unitOfWork.ContentType.GetAll().ToList();

            // Add URL data to ViewBag for SEO and navigation
            ViewBag.CanonicalUrl = _urlService.GetCanonicalUrl("Blogs", "Index", Lang);
            ViewBag.AlternateUrls = _urlService.GetAlternateLanguageUrls("Blogs", "Index", Lang);

            return View(ContentTypes);
        }

        [HttpGet]
        [Route("blog-detail/{slug}")]
        [Route("blog-detay/{slug}")]
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Find blog by slug with ContentType included
            var blog = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                .FirstOrDefault(b => b.Slug == slug && b.IsPublished && b.Language == Lang);

            if (blog == null)
            {
                _logger.LogWarning($"Blog with slug '{slug}' not found or not published");
                return NotFound();
            }

            // Add URL data to ViewBag for SEO and navigation
            ViewBag.CanonicalUrl = _urlService.GetCanonicalUrl("Blogs", "Details", Lang, new { slug });
            ViewBag.AlternateUrls = _urlService.GetAlternateLanguageUrls("Blogs", "Details", Lang, new { slug });
            ViewData["Lang"] = Lang;
            
            return View(blog);
        }



        [Route("Blogs/GetAllBlogs")]
        public IActionResult GetAllBlogs()
        {
            var blogs = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                .Where(b => b.IsPublished && b.Language == Lang)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            // Set culture based on language
            var culture = Lang == "en" ? new CultureInfo("en-US") : new CultureInfo("tr-TR");

            var result = blogs.Select(b => new
            {
                title = b.Title,
                image = b.CardImageUrl,
                date = b.CreatedAt.ToString("dd MMMM yyyy", culture),
                category = b.ContentType.Name,
                slug = b.Slug,
                metaDescription = b.MetaDescription,
            });
                

            return Json(result);
        }
    }
}
