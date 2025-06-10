using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactController> _logger;
        private readonly EmailTemplatesConfig _emailTemplates;
        private readonly string _companyEmail = "adobe.mrziro@gmail.com";
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public ContactController(IEmailSender emailSender, ILogger<ContactController> logger, IOptions<EmailTemplatesConfig> emailTemplates)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailTemplates = emailTemplates.Value ?? throw new ArgumentNullException(nameof(emailTemplates));
        }

        [HttpGet("contact")]
        [HttpGet("iletisim")]
        public IActionResult Index()
        {
            var model = new ContactVM { Language = Lang };
            return View(model);
        }

        [HttpPost("contact")]
        [HttpPost("iletisim")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactVM model)
        {
            // Set language for validation and email templates
            model.Language = Lang;
            
            // Get email template for current language
            var emailTemplate = _emailTemplates.GetContactTemplate(Lang);
            if (emailTemplate == null)
            {
                _logger.LogError("Email template not found for language: {Language}", Lang);
                emailTemplate = _emailTemplates.GetContactTemplate("tr"); // Fallback to Turkish
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = emailTemplate?.ValidationMessage ?? "Please fill in all fields correctly.";
                return View(model);
            }

            try
            {
                _logger.LogInformation("Processing contact form submission from {Email} in language {Language}", model.Email, Lang);
                
                // Sanitize inputs to prevent XSS
                model.Name = model.Name?.Trim();
                model.Email = model.Email?.Trim();
                model.Subject = model.Subject?.Trim();
                model.Message = model.Message?.Trim();

                // Use localized email template
                string subject = emailTemplate.FormatSubject(model.Subject);
                
                // Generate field labels based on language
                var labels = Lang == "en" 
                    ? new { Name = "Name", Email = "Email", Subject = "Subject", Message = "Message", Date = "Date" }
                    : new { Name = "İsim", Email = "E-posta", Subject = "Konu", Message = "Mesaj", Date = "Tarih" };
                
                string htmlMessage = $@"
                    {emailTemplate.EmailBody}
                    <table border='0' cellpadding='5'>
                        <tr><td><b>{labels.Name}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Name)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Email)}</td></tr>
                        <tr><td><b>{labels.Subject}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Subject)}</td></tr>
                        <tr><td><b>{labels.Message}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Message)}</td></tr>
                        <tr><td><b>{labels.Date}:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>";
                
                // Send to the company email
                await _emailSender.SendEmailAsync(_companyEmail, subject, htmlMessage);
                _logger.LogInformation("Contact form email sent to primary recipient");
                
                // Send confirmation to the sender
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>{emailTemplate.FormatGreeting(model.Name)}</h2>
                        <p>{emailTemplate.ConfirmationText}</p>
                        <p><b>{labels.Subject}:</b> {System.Web.HttpUtility.HtmlEncode(model.Subject)}</p>
                        <p><b>{labels.Date}:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>{emailTemplate.ConfirmationFooter}</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Email, emailTemplate.ConfirmationSubject, confirmationHtmlMessage);
                _logger.LogInformation("Confirmation email sent to user {Email} in language {Language}", model.Email, Lang);
                
                TempData["Success"] = emailTemplate.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process contact form from {Email}: {ErrorMessage}", model.Email, ex.Message);
                
                // Don't expose technical details to the user
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while sending the message.";
                return View(model);
            }
        }
    }
}
