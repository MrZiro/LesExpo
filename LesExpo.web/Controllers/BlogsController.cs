using LesExpo.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class BlogsController : Controller
    {
        private readonly ILogger<BlogsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public BlogsController(ILogger<BlogsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("all-news")]
        [HttpGet("tum-haberler")]
        public IActionResult Index()
        {
            var ContentTypes = _unitOfWork.ContentType.GetAll().ToList();

            return View(ContentTypes);
        }

        [HttpGet]
        [Route("Blog-detail/{slug}")]
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            // Find blog by slug with ContentType included
            var blog = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                .FirstOrDefault(b => b.Slug == slug && b.IsPublished);

            if (blog == null)
            {
                _logger.LogWarning($"Blog with slug '{slug}' not found or not published");
                return NotFound();
            }

            return View(blog);
        }



        [Route("Blogs/GetAllBlogs")]
        public IActionResult GetAllBlogs()
        {
            var blogs = _unitOfWork.Blog.GetAll(includeProperties: "ContentType")
                .Where(b => b.IsPublished && b.Language == Lang)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            var result = blogs.Select(b => new
            {
                title = b.Title,
                image = b.CardImageUrl,
                date = b.CreatedAt.ToString("dd MMMM yyyy"),
                category = b.ContentType.Name,
                slug = b.Slug,
                metaDescription = b.MetaDescription,
            });
                

            return Json(result);
        }
    }
}
