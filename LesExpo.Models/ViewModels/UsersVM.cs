using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesExpo.Utility;
using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        public string Name { get; set; }
        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        public string Email { get; set; }
        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
