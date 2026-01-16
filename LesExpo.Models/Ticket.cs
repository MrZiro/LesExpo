using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LesExpo.Utility;

namespace LesExpo.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string Sector { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Website { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
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
