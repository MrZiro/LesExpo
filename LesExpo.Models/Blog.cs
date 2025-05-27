using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [MaxLength(200)]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        //[Display(Name = "Short Description")]
        //[MaxLength(500)]
        //public string ShortDescription { get; set; }
        [ValidateNever]

        [Display(Name = "Featured Image")]
        public string CardImageUrl { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Meta Description")]
        [MaxLength(200)]
        public string MetaDescription { get; set; }

        [Display(Name = "Meta Keywords")]
        [MaxLength(200)]
        public string MetaKeywords { get; set; }

        // Foreign key
        [Display(Name = "Category")]
        public int? ContentTypeId { get; set; }

        [ForeignKey("ContentTypeId")]
        [ValidateNever]
        public ContentType ContentType { get; set; }
    }
}
