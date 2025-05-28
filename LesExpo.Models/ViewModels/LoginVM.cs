using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "E-posta alanı gereklidir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
