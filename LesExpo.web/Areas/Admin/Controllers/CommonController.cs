using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using LesExpo.web.Services;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileHelper _fileHelper;

        public CommonController(IWebHostEnvironment webHostEnvironment, IFileHelper fileHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileHelper = fileHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadEditorImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                {
                    return Json(new { error = "No file uploaded" });
                }

                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                
                // Save to temporary folder
                string tempDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Temp");
                
                // Ensure directory exists
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }

                string filePath = Path.Combine(tempDirectory, fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Return URL for TinyMCE
                string fileUrl = $"/uploads/Temp/{fileName}";

                // TinyMCE expects this specific response format
                return Json(new { location = fileUrl });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
