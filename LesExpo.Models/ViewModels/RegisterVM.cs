using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models.ViewModels
{
    public class RegisterVM
    {

        public string? Id { get; set; }
        [Required(ErrorMessage = "E-posta alanı gereklidir")]
        
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre onay alanı gereklidir")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor")]
        [Display(Name = "Şifreyi onayla")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "İsim alanı gereklidir")]
        public string Name { get; set; }

        public string? RedirectUrl { get; set; }
        public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
