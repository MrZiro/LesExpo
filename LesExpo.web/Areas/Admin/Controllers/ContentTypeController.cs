using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ContentTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContentTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<ContentType> contentTypeList = _unitOfWork.ContentType.GetAll().ToList();
            ViewBag.Count = _unitOfWork.ContentType.GetCount();
            return View(contentTypeList);
        }

        // GET
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                // Create new content type
                return View(new ContentType());
            }

            // Edit existing content type
            ContentType contentType = _unitOfWork.ContentType.Get(u => u.Id == id);
            if (contentType == null)
            {
                return NotFound();
            }

            return View(contentType);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ContentType contentType)
        {
            if (ModelState.IsValid)
            {
                if (contentType.Id == 0)
                {
                    _unitOfWork.ContentType.Add(contentType);
                    TempData["success"] = "İçerik türü başarıyla oluşturuldu";
                }
                else
                {
                    _unitOfWork.ContentType.Update(contentType);
                    TempData["success"] = "İçerik türü başarıyla güncellendi";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(contentType);
        }

        // API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ContentType> contentTypeList = _unitOfWork.ContentType.GetAll().ToList();
            return Json(new { data = contentTypeList });
        }

        [HttpGet]
        public IActionResult GetPaged(int page = 1, int pageSize = 10, string searchTerm = "", string sortColumn = null, string sortDirection = "asc")
        {
            // Get all content types first (in a real app, you'd use Skip/Take at the repository level for efficiency)
            IEnumerable<ContentType> contentTypes = _unitOfWork.ContentType.GetAll();
            
            // Total count before filtering
            int totalRecords = contentTypes.Count();
            
            // Apply filtering if search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                contentTypes = contentTypes.Where(c => 
                    c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            // Get filtered count
            int filteredCount = contentTypes.Count();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                switch (sortColumn.ToLower())
                {
                    case "name":
                        contentTypes = sortDirection.ToLower() == "asc" 
                            ? contentTypes.OrderBy(c => c.Name)
                            : contentTypes.OrderByDescending(c => c.Name);
                        break;
                    case "id":
                        contentTypes = sortDirection.ToLower() == "asc" 
                            ? contentTypes.OrderBy(c => c.Id)
                            : contentTypes.OrderByDescending(c => c.Id);
                        break;
                    default:
                        contentTypes = contentTypes.OrderBy(c => c.Id);
                        break;
                }
            }
            else
            {
                // Default sorting
                contentTypes = contentTypes.OrderBy(c => c.Id);
            }
            
            // Apply pagination
            var pagedData = contentTypes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            // Return the data with pagination metadata
            return Json(new 
            { 
                data = pagedData,
                totalRecords = filteredCount,
                totalPages = (int)Math.Ceiling((double)filteredCount / pageSize)
            });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Json(new { success = false, message = "Geçersiz ID" });
            }

            ContentType contentTypeToDelete = _unitOfWork.ContentType.Get(u => u.Id == id);

            if (contentTypeToDelete == null)
            {
                return Json(new { success = false, message = "Hata: İçerik türü bulunamadı" });
            }

            try
            {
                _unitOfWork.ContentType.Remove(contentTypeToDelete);
                _unitOfWork.Save();

                return Json(new { success = true, message = "İçerik türü başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "İçerik türünü silerken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
