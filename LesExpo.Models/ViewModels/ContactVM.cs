using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{

    public class ContactVM
    {
        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Language for the form (used for validation messages and email templates)
        /// </summary>
        public string Language { get; set; } = "tr";
    }
}
