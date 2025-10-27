using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System;
using System.Linq;
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
        [IgnoreAntiforgeryToken]
        [RequestSizeLimit(200 * 1024 * 1024)] // 200MB limit
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

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [RequestSizeLimit(200 * 1024 * 1024)] // 200MB limit
        public IActionResult UploadEditorVideo()
        {
            try
            {
                // Check if files are present
                if (Request.Form.Files == null || Request.Form.Files.Count == 0)
                {
                    return Json(new { error = "No file uploaded" });
                }

                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                {
                    return Json(new { error = "No file uploaded" });
                }

                // Log file details for debugging
                System.Diagnostics.Debug.WriteLine($"Video upload attempt: {file.FileName}, Size: {file.Length} bytes");

                // Validate video file
                var modelState = new ModelStateDictionary();
                if (!_fileHelper.ValidateVideoFile(file, modelState, "video", isRequired: true))
                {
                    var errorMessage = modelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "Invalid video file";
                    System.Diagnostics.Debug.WriteLine($"Video validation failed: {errorMessage}");
                    return Json(new { error = errorMessage });
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
                System.Diagnostics.Debug.WriteLine($"Video uploaded successfully: {fileUrl}");

                // TinyMCE expects this specific response format
                return Json(new { location = fileUrl });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Video upload error: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
    }
}
