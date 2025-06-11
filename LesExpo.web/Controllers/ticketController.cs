using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Models.Configuration;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class TicketController : Controller
    {
        private readonly IUrlLocalizationService _urlService;
        private readonly IExternalApiService _externalApiService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TicketController> _logger;
        private readonly EmailTemplatesConfig _emailTemplates;
        private readonly string _adminEmail = "adobe.mrziro@gmail.com";
        
        // FUAR_ID - you can provide this value
        private const int FUAR_ID = 41395; // You need to provide this value
        
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public TicketController(IUrlLocalizationService urlService, IExternalApiService externalApiService,
                               IEmailSender emailSender, ILogger<TicketController> logger, 
                               IOptions<EmailTemplatesConfig> emailTemplates)
        {
            _urlService = urlService;
            _externalApiService = externalApiService;
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailTemplates = emailTemplates.Value ?? throw new ArgumentNullException(nameof(emailTemplates));
        }

        [HttpGet("online-bilet")]
        [HttpGet("online-ticket")]
        public IActionResult Index()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("ticket", "Index", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("ticket", "Index", Lang);
            return View();
        }

        [HttpPost("online-bilet")]
        [HttpPost("online-ticket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TicketVM model)
        {
            // Get email template for current language
            var emailTemplate = _emailTemplates.GetTemplate("Ticket", Lang);
            if (emailTemplate == null)
            {
                _logger.LogError("Ticket email template not found for language: {Language}", Lang);
                emailTemplate = _emailTemplates.GetTemplate("Registration", "tr"); // Fallback to Registration template
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = emailTemplate?.ValidationMessage ?? "Please fill in all required fields correctly.";
                return View(model);
            }

            try
            {
                _logger.LogInformation("Processing ticket form submission from {Email} in language {Language}", model.Email, Lang);
                
                // Sanitize inputs
                model.FirstName = model.FirstName?.Trim();
                model.LastName = model.LastName?.Trim();
                model.Email = model.Email?.Trim();
                model.CompanyName = model.CompanyName?.Trim();

                // isYabanci: false for Turkish (tr), true for other languages
                bool isYabanci = Lang != "tr";

                // Call external API through IExternalApiService
                var apiResult = await _externalApiService.AddZiyaretciAsync(
                    model.FirstName, model.LastName, model.Email, model.Phone, model.Gender,
                    model.CompanyName, model.Position, model.Sector, int.Parse(model.Country), 
                    model.City, isYabanci, FUAR_ID);

                if (!apiResult.Success)
                {
                    _logger.LogError("API call failed: {ErrorMessage}. Response: {ResponseContent}", 
                                   apiResult.ErrorMessage, apiResult.ResponseContent);
                    TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while processing your ticket request.";
                    return View(model);
                }

                _logger.LogInformation("Successfully submitted ticket to API for {Email}", model.Email);

                // Send confirmation email to admin
                string subject = $"New Ticket Registration - {model.CompanyName} - {model.FirstName} {model.LastName}";
                
                // Generate field labels based on language
                var labels = Lang == "en" 
                    ? new {
                        FirstName = "First Name", LastName = "Last Name", Email = "Email",
                        Phone = "Phone", CompanyName = "Company Name", Sector = "Sector",
                        Website = "Website", Country = "Country", City = "City",
                        Position = "Position", Gender = "Gender", Date = "Date"
                    }
                    : new {
                        FirstName = "Ad", LastName = "Soyad", Email = "E-posta",
                        Phone = "Telefon", CompanyName = "Firma Adı", Sector = "Sektör",
                        Website = "Web Sitesi", Country = "Ülke", City = "Şehir",
                        Position = "Pozisyon", Gender = "Cinsiyet", Date = "Tarih"
                    };
                
                string htmlMessage = $@"
                    <h2>New Ticket Registration</h2>
                    <table border='0' cellpadding='5'>
                        <tr><td><b>{labels.FirstName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.FirstName)}</td></tr>
                        <tr><td><b>{labels.LastName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.LastName)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Email)}</td></tr>
                        <tr><td><b>{labels.Phone}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Phone)}</td></tr>
                        <tr><td><b>{labels.CompanyName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.CompanyName)}</td></tr>
                        <tr><td><b>{labels.Sector}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Sector)}</td></tr>
                        <tr><td><b>{labels.Website}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Website)}</td></tr>
                        <tr><td><b>{labels.Country}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Country)}</td></tr>
                        <tr><td><b>{labels.City}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.City)}</td></tr>
                        <tr><td><b>{labels.Position}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Position)}</td></tr>
                        <tr><td><b>{labels.Gender}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Gender)}</td></tr>
                        <tr><td><b>{labels.Date}:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>
                    <br>
                    <p><strong>API Response:</strong> {System.Web.HttpUtility.HtmlEncode(apiResult.ResponseContent)}</p>";

                // Send to the admin email
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
                _logger.LogInformation("Ticket form email sent to admin for {Company} - {FullName}", model.CompanyName, $"{model.FirstName} {model.LastName}");
                
                TempData["Success"] = Lang == "en" ? "Your ticket registration has been submitted successfully!" : "Bilet kaydınız başarıyla gönderildi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process ticket form from {Email}: {ErrorMessage}", model.Email, ex.Message);
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while processing your ticket request.";
                return View(model);
            }
        }
    }
}
