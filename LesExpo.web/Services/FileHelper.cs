using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LesExpo.Utility;

namespace LesExpo.web.Services
{
    // Implements the IFileHelper interface
    public class FileHelper : IFileHelper
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _uploadPath; // Path relative to wwwroot, e.g., "images/product"

        // Inject required framework services
        public FileHelper(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            // Get upload path from configuration (e.g., appsettings.json)
            _uploadPath = configuration["FileSettings:UploadPath"]?.Trim('/') ?? "uploads"; // Use a default if config missing
        }

        public async Task<string?> SaveFileAsync(IFormFile file, string? existingImageUrl, string? subDirectory = null)
        {
            if (file == null || file.Length == 0)
            {
                // If no new file, just return the existing URL (or null if you want to clear it)
                return existingImageUrl;
            }

            string wwwRootPath = _hostEnvironment.WebRootPath;
            string uploadsFolder = Path.Combine(wwwRootPath, _uploadPath);

            // Ensure the upload directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // If a subdirectory is specified, create it within the uploads folder
            if (!string.IsNullOrEmpty(subDirectory))
            {
                uploadsFolder = Path.Combine(uploadsFolder, subDirectory);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
            }

            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Combine the upload path with the unique file name and extension
            string newFilePath = Path.Combine(uploadsFolder, fileName + extension);

            // Delete the old file if it exists
            if (!string.IsNullOrEmpty(existingImageUrl))
            {
                // Construct the full path to the old file based on wwwroot and the stored URL
                var oldFilePath = Path.Combine(wwwRootPath, existingImageUrl.TrimStart('\\', '/'));
                // Use Task.Run for potentially blocking IO in async context if not sure if Exists/Delete are async friendly (they aren't always guaranteed)
                await Task.Run(() => {
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                });
            }

            // Save the new file asynchronously
            using (var fileStream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the new URL relative to wwwroot (using correct separators for URL)
            string relativePath = subDirectory != null
                ? $"/{_uploadPath.Replace('\\', '/')}/{subDirectory}/{fileName}{extension}"
                : $"/{_uploadPath.Replace('\\', '/')}/{fileName}{extension}";

            return relativePath; // Use / for URL
        }

        public async Task DeleteFileAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return; // Nothing to delete

            string wwwRootPath = _hostEnvironment.WebRootPath;
            // Construct the full path from the stored URL
            var filePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('\\', '/'));

            await Task.Run(() => {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            });
        }

        public bool ValidateImageFile(IFormFile file, ModelStateDictionary modelState, string key, bool isRequired = false)
        {
            // Check if file is required but not provided
            if (isRequired && (file == null || file.Length == 0))
            {
                modelState.AddModelError(key, "Please select an image.");
                return false;
            }
            
            // If file is not required and not provided, it's valid
            if (!isRequired && (file == null || file.Length == 0))
            {
                return true;
            }

            bool isValid = true;

            // Check file size using constant from SD
            if (file.Length > SD.MaxImageSizeInBytes)
            {
                modelState.AddModelError(key, $"Image size cannot exceed {SD.MaxImageSizeInMB}MB.");
                isValid = false;
            }

            // Check file format using allowed extensions from SD
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!SD.AllowedImageExtensions.Contains(fileExtension))
            {
                modelState.AddModelError(key, "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                isValid = false;
            }

            return isValid;
        }
    }
}
