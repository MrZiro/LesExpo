using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using LesExpo.Utility;
using Microsoft.EntityFrameworkCore;

namespace LesExpo.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [Display(Name = "Dil")]
        public string Language { get; set; } = "tr"; // Default to Turkish

        [Required(ErrorMessage = "Başlık alanı gereklidir")]
        [MaxLength(200, ErrorMessage = "Başlık 200 karakterden uzun olamaz")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [MaxLength(200, ErrorMessage = "Slug 200 karakterden uzun olamaz")]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [Display(Name = "İçerik")]
        public string Content { get; set; }

        //[Display(Name = "Short Description")]
        //[MaxLength(500)]
        //public string ShortDescription { get; set; }
        [ValidateNever]

        [Display(Name = "Öne Çıkan Görsel")]
        public string CardImageUrl { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Yayınlandı mı")]
        public bool IsPublished { get; set; } = true;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [Display(Name = "Yazar")]
        public string Author { get; set; }

        [Display(Name = "Meta Açıklama")]
        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [MaxLength(200, ErrorMessage = "Meta açıklama 200 karakterden uzun olamaz")]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [Display(Name = "Meta Anahtar Kelimeler")]
        [MaxLength(200, ErrorMessage = "Meta anahtar kelimeler 200 karakterden uzun olamaz")]
        public string MetaKeywords { get; set; }

        // Foreign key
        [Display(Name = "Kategori")]
        public int? ContentTypeId { get; set; }

        [ForeignKey("ContentTypeId")]
        [ValidateNever]
        public ContentType ContentType { get; set; }
    }
}
