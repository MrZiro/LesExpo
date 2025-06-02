using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Konu alanı zorunludur.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        public string Message { get; set; }
    }
}
