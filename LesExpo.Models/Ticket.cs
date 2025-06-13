using System;
using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Sector { get; set; } = string.Empty;

        [StringLength(200)]
        public string Website { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public bool Terms { get; set; }

        [StringLength(5)]
        public string Language { get; set; } = "tr";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // API response data
        [StringLength(1000)]
        public string ApiResponse { get; set; } = string.Empty;

        public bool ApiSuccess { get; set; } = false;
    }
}
