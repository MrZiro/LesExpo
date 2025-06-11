using System.ComponentModel.DataAnnotations;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{
    public class TicketVM
    {
        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string LastName { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        [EmailAddress(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Phone { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Sector { get; set; }

        public string Website { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Country { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string City { get; set; }

        public string Position { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Gender { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public bool Terms { get; set; }
    }
} 