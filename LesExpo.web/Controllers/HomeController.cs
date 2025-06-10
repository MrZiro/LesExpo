using System.Diagnostics;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace LesExpo.web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("")]
    [HttpGet("{lang:regex(^tr|en$)}")]
    [HttpGet("{lang}/home")]
    [HttpGet("{lang}/anasayfa")]
    public IActionResult Index()
    {
        var sliders = _unitOfWork.Slider.GetAll()
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.DisplayOrder)
                    .ToList();

        // Get latest published blogs for the blog slider
        var blogs = _unitOfWork.Blog.GetAll()
                    .Where(b => b.IsPublished)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(3) // Match the 3 slides in the original design
                    .ToList();

        // Create and populate ViewModel
        var homeVM = new HomeVM
        {
            Sliders = sliders,
            Blogs = blogs
        };

        return View(model: homeVM);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

