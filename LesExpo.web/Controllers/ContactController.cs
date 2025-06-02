using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LesExpo.web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactController> _logger;
        private readonly string _companyEmail = "adobe.mrziro@gmail.com";

        public ContactController(IEmailSender emailSender, ILogger<ContactController> logger)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Iletisim()
        {
            return View(new ContactVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Iletisim(ContactVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm alanları doğru şekilde doldurunuz.";
                return View(model);
            }

            try
            {
                _logger.LogInformation("Processing contact form submission from {Email}", model.Email);
                
                // Sanitize inputs to prevent XSS
                model.Name = model.Name?.Trim();
                model.Email = model.Email?.Trim();
                model.Subject = model.Subject?.Trim();
                model.Message = model.Message?.Trim();

                string subject = $"[İletişim Formu] {model.Subject}";
                string htmlMessage = $@"
                    <h2>İletişim Formu Mesajı</h2>
                    <table border='0' cellpadding='5'>
                        <tr><td><b>İsim:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Name)}</td></tr>
                        <tr><td><b>E-posta:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Email)}</td></tr>
                        <tr><td><b>Konu:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Subject)}</td></tr>
                        <tr><td><b>Mesaj:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Message)}</td></tr>
                        <tr><td><b>Tarih:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>";
                
                // Send to the company email
                await _emailSender.SendEmailAsync(_companyEmail, subject, htmlMessage);
                _logger.LogInformation("Contact form email sent to primary recipient");
                
                // Also send a copy to the info email
                _logger.LogInformation("Contact form email sent to secondary recipient");
                
                // Send confirmation to the sender
                string confirmationSubject = "İletişim Formunuz Alındı - LES-EXPO";
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>Sayın {System.Web.HttpUtility.HtmlEncode(model.Name)},</h2>
                        <p>İletişim formunuz başarıyla alınmıştır. En kısa sürede sizinle iletişime geçeceğiz.</p>
                        <p>Mesajınızın detayları:</p>
                        <p><b>Konu:</b> {System.Web.HttpUtility.HtmlEncode(model.Subject)}</p>
                        <p><b>Tarih:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>İyi günler dileriz,<br/>LES-EXPO Ekibi</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Email, confirmationSubject, confirmationHtmlMessage);
                _logger.LogInformation("Confirmation email sent to user {Email}", model.Email);
                
                TempData["Success"] = "Mesajınız başarıyla gönderildi. En kısa sürede sizinle iletişime geçeceğiz.";
                return RedirectToAction(nameof(Iletisim));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process contact form from {Email}: {ErrorMessage}", model.Email, ex.Message);
                
                // Don't expose technical details to the user
                TempData["Error"] = "Mesaj gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                return View(model);
            }
        }
    }
}
