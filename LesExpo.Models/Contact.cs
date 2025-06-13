using System;
using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        [StringLength(5)]
        public string Language { get; set; } = "tr";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
