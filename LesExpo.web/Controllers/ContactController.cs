using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactController> _logger;
        private readonly EmailTemplatesConfig _emailTemplates;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _companyEmail = SD.AdminEmail;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public ContactController(IEmailSender emailSender, ILogger<ContactController> logger, IOptions<EmailTemplatesConfig> emailTemplates, IUnitOfWork unitOfWork)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailTemplates = emailTemplates.Value ?? throw new ArgumentNullException(nameof(emailTemplates));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
                _logger.LogInformation("Processing contact form submission from {Email} in language {Language}", model.Contact.Email, Lang);
                
                // Sanitize inputs to prevent XSS
                model.Contact.Name = model.Contact.Name?.Trim();
                model.Contact.Email = model.Contact.Email?.Trim();
                model.Contact.Subject = model.Contact.Subject?.Trim();
                model.Contact.Message = model.Contact.Message?.Trim();

                // Set language and timestamp
                model.Contact.Language = Lang;
                model.Contact.CreatedAt = DateTime.Now;

                _unitOfWork.Contact.Add(model.Contact);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Contact form saved to database with ID: {ContactId}", model.Contact.Id);

                // Use localized email template
                string subject = emailTemplate.FormatSubject(model.Contact.Subject);
                
                // Generate field labels based on language
                var labels = Lang == "en" 
                    ? new { Name = "Name", Email = "Email", Subject = "Subject", Message = "Message", Date = "Date" }
                    : new { Name = "İsim", Email = "E-posta", Subject = "Konu", Message = "Mesaj", Date = "Tarih" };
                
                string htmlMessage = $@"
                    {emailTemplate.EmailBody}
                    <table border='0' cellpadding='5'>
                        <tr><td><b>{labels.Name}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Contact.Name)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Contact.Email)}</td></tr>
                        <tr><td><b>{labels.Subject}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Contact.Subject)}</td></tr>
                        <tr><td><b>{labels.Message}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Contact.Message)}</td></tr>
                        <tr><td><b>{labels.Date}:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>";
                
                // Send to the company email
                await _emailSender.SendEmailAsync(_companyEmail, subject, htmlMessage);
                _logger.LogInformation("Contact form email sent to primary recipient");
                
                // Send confirmation to the sender
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>{emailTemplate.FormatGreeting(model.Contact.Name)}</h2>
                        <p>{emailTemplate.ConfirmationText}</p>
                        <p><b>{labels.Subject}:</b> {System.Web.HttpUtility.HtmlEncode(model.Contact.Subject)}</p>
                        <p><b>{labels.Date}:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>{emailTemplate.ConfirmationFooter}</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Contact.Email, emailTemplate.ConfirmationSubject, confirmationHtmlMessage);
                _logger.LogInformation("Confirmation email sent to user {Email} in language {Language}", model.Contact.Email, Lang);
                
                TempData["Success"] = emailTemplate.SuccessMessage;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process contact form from {Email}: {ErrorMessage}", model.Contact.Email, ex.Message);
                
                // Don't expose technical details to the user
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while sending the message.";
                return View(model);
            }
        }
    }
}
