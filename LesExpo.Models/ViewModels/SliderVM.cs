using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; } = new Slider();
        
        // For image upload
        public IFormFile? ImageFile { get; set; }
        
        // For video upload
        public IFormFile? VideoFile { get; set; }
    }
}