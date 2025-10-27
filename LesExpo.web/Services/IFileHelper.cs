using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace LesExpo.web.Services
{
    public interface IFileHelper
    {
        // Add methods required for file handling
        // You might adjust return types (e.g., return a Result object with success/error messages)
        // depending on how you want to handle errors in the controller.
        // This example uses async as the file operations are async.

        // Saves a file, deletes the old one if specified, returns the new URL relative to wwwroot
        Task<string?> SaveFileAsync(IFormFile file, string? existingImageUrl, string? subDirectory = null);

        // Deletes a file based on its URL relative to wwwroot
        Task DeleteFileAsync(string imageUrl);

        // Validates an image file and adds error messages to ModelState if validation fails
        // Returns true if valid, false if invalid
        bool ValidateImageFile(IFormFile file, ModelStateDictionary modelState, string key, bool isRequired = false);

        // Validates a video file and adds error messages to ModelState if validation fails
        // Returns true if valid, false if invalid
        bool ValidateVideoFile(IFormFile file, ModelStateDictionary modelState, string key, bool isRequired = false);

        // Optional: Add validation method if not doing it in controller
        // bool IsValidFile(IFormFile file);
    }
}
