using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models
{
    public class Slider
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string? Subtitle { get; set; }
        
        public string? Description { get; set; }
        
        public string? ButtonText { get; set; }
        
        public string? ButtonUrl { get; set; }
        
        [Required]
        public int DisplayOrder { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Media Type: "image" or "video"
        [Required]
        public string MediaType { get; set; } = "image";
        
        // For images - existing field
        public string? ImageUrl { get; set; }
        
        // For videos - file path for uploaded videos
        public string? VideoUrl { get; set; }
        
        // For YouTube videos
        public string? YoutubeUrl { get; set; }
        
        // Video source type: "upload" or "youtube"
        public string? VideoSource { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedDate { get; set; }
    }
}