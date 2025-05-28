using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LesExpo.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [MaxLength(250, ErrorMessage = "Subtitle cannot exceed 250 characters")]
        public string Subtitle { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Display(Name = "Button Text")]
        [MaxLength(50, ErrorMessage = "Button text cannot exceed 50 characters")]
        public string ButtonText { get; set; }

        [Display(Name = "Button URL")]
        [MaxLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        public string ButtonUrl { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}
