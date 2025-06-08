using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
     [Authorize(Roles = SD.Role_Admin)] // Temporarily commented for testing
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHtmlContentService _htmlContentService;

        public BlogController(
            IUnitOfWork unitOfWork, 
            IFileHelper fileHelper, 
            IWebHostEnvironment webHostEnvironment,
            IHtmlContentService htmlContentService)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
            _webHostEnvironment = webHostEnvironment;
            _htmlContentService = htmlContentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // DEBUG: Simple test action to verify routing works
        [HttpGet]
        public IActionResult Test()
        {
            return Content("BlogController Test Action - Routing Works!");
        }

        // GET: Admin/Blog/Create
        public IActionResult Create()
        {
            BlogVM pageVM = new BlogVM()
            {
                Blog = new Blog(),
                ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            return View(pageVM);
        }

        // POST: Admin/Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogVM pageVM)
        {
            // Populate content type list in case we need to return to the form
            pageVM.ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            // Validate card image (required for new blogs)
            _fileHelper.ValidateImageFile(pageVM.CardImage, ModelState, "CardImage", isRequired: true);

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle card image upload
                    pageVM.Blog.CardImageUrl = await _fileHelper.SaveFileAsync(
                        pageVM.CardImage, null, "Blogs");
                    
                    if (pageVM.Blog.CardImageUrl == null)
                    {
                        ModelState.AddModelError("CardImage", "Görsel yüklenirken hata oluştu. Lütfen tekrar deneyin.");
                        return View(pageVM);
                    }

                    // Process TinyMCE editor content to handle images
                    if (!string.IsNullOrEmpty(pageVM.Blog.Content))
                    {
                        // Process editor content and move images from temp to permanent storage
                        pageVM.Blog.Content = _htmlContentService.ProcessEditorContentImages(pageVM.Blog.Content);
                    }

                    _unitOfWork.Blog.Add(pageVM.Blog);
                    _unitOfWork.Save();
                    TempData["success"] = "Blog başarıyla oluşturuldu.";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hata: " + ex.Message);
                    return View(pageVM);
                }
            }
            
            return View(pageVM);
        }

        // GET: Admin/Blog/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            BlogVM pageVM = new BlogVM()
            {
                Blog = _unitOfWork.Blog.Get(u => u.Id == id, includeProperties: "ContentType"),
                ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if (pageVM.Blog == null)
            {
                return NotFound();
            }

            return View(pageVM);
        }

        // POST: Admin/Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogVM pageVM)
        {
            // Populate content type list in case we need to return to the form
            pageVM.ContentTypeList = _unitOfWork.ContentType.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            // Validate card image (not required for edits)
            _fileHelper.ValidateImageFile(pageVM.CardImage, ModelState, "CardImage", isRequired: false);

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the original blog content to compare with the edited version
                    var originalBlog = _unitOfWork.Blog.Get(u => u.Id == pageVM.Blog.Id);
                    string originalContent = originalBlog?.Content ?? string.Empty;
                    
                    // Handle card image upload if new image is provided
                    if (pageVM.CardImage != null)
                    {
                        string oldImageUrl = pageVM.Blog.CardImageUrl;
                        pageVM.Blog.CardImageUrl = await _fileHelper.SaveFileAsync(
                            pageVM.CardImage, oldImageUrl, "Blogs");
                        
                        if (pageVM.Blog.CardImageUrl == null)
                        {
                            ModelState.AddModelError("CardImage", "Görsel yüklenirken hata oluştu. Lütfen tekrar deneyin.");
                            return View(pageVM);
                        }
                    }

                    // Process TinyMCE editor content to handle images
                    if (pageVM.Blog.Content != null)
                    {
                        // Process both original and edited content to handle deleted images properly
                        pageVM.Blog.Content = _htmlContentService.ProcessEditedContent(originalContent, pageVM.Blog.Content);
                    }

                    _unitOfWork.Blog.Update(pageVM.Blog);
                    _unitOfWork.Save();
                    TempData["success"] = "Blog başarıyla güncellendi.";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hata: " + ex.Message);
                    return View(pageVM);
                }
            }
            
            return View(pageVM);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Geçersiz ID." });
            }

            Blog pageToDelete = await _unitOfWork.Blog.GetAsync(u => u.Id == id);

            if (pageToDelete == null)
            {
                return Json(new { success = false, message = "Hata: Sayfa bulunamadı." });
            }

            try
            {
                // Delete card image
                if (!string.IsNullOrEmpty(pageToDelete.CardImageUrl))
                {
                    await _fileHelper.DeleteFileAsync(pageToDelete.CardImageUrl);
                }

                // Delete content images
                if (!string.IsNullOrEmpty(pageToDelete.Content))
                {
                    _htmlContentService.DeleteContentImages(pageToDelete.Content);
                }

                _unitOfWork.Blog.Remove(pageToDelete);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, message = "Blog başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Blog silinirken bir hata oluştu: " + ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Blog.GetAll(includeProperties: "ContentType").OrderByDescending(u => u.Id);
            return Json(new { data = allObj });
        }
    }
}
