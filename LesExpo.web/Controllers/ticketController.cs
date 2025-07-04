using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Models.Configuration;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using LesExpo.DataAccess.Repository.IRepository;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _adminEmail = SD.AdminEmail;
        
        // FUAR_ID - you can provide this value
        private const int FUAR_ID = SD.FUAR_ID; // You need to provide this value
        
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();

        public TicketController(IUrlLocalizationService urlService, IExternalApiService externalApiService,
                               IEmailSender emailSender, ILogger<TicketController> logger, 
                               IOptions<EmailTemplatesConfig> emailTemplates, IUnitOfWork unitOfWork)
        {
            _urlService = urlService;
            _externalApiService = externalApiService;
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailTemplates = emailTemplates.Value ?? throw new ArgumentNullException(nameof(emailTemplates));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("online-bilet")]
        [HttpGet("online-ticket")]
        public IActionResult Index()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("ticket", "Index", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("ticket", "Index", Lang);
            
            var model = new TicketVM
            {
                Language = Lang
            };
            
            return View(model);
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
                _logger.LogInformation("Processing ticket form submission from {Email} in language {Language}", model.Ticket.Email, Lang);
                
                // Set language
                model.Ticket.Language = Lang;
                
                // Sanitize inputs
                model.Ticket.FirstName = model.Ticket.FirstName?.Trim();
                model.Ticket.LastName = model.Ticket.LastName?.Trim();
                model.Ticket.Email = model.Ticket.Email?.Trim();
                model.Ticket.CompanyName = model.Ticket.CompanyName?.Trim();

                // Convert IDs to names for both email display and database storage
                string countryName = model.Ticket.Country;
                string cityName = model.Ticket.City;
                string sectorName = model.Ticket.Sector;
                int countryIdForApi = 0; // Keep original ID for API call

                // Convert Country ID to Name
                if (int.TryParse(model.Ticket.Country, out int countryId))
                {
                    countryIdForApi = countryId; // Store for API call
                    var countriesResponse = await _externalApiService.GetStatesAsync();
                    var country = countriesResponse?.data?.FirstOrDefault(u => u.ulkeId == countryId);
                    if (country != null)
                    {
                        countryName = country.ulkeAdi;
                        model.Ticket.Country = country.ulkeAdi; // Update model for saving to DB
                    }
                }

                // Convert City ID to Name
                if (int.TryParse(model.Ticket.City, out int cityId) && countryId > 0)
                {
                    var citiesResponse = await _externalApiService.GetCitiesAsync(countryId);
                    var city = citiesResponse?.data?.FirstOrDefault(s => s.sehirId == cityId);
                    if (city != null)
                    {
                        cityName = city.sehirAdi;
                        model.Ticket.City = city.sehirAdi; // Update model for saving to DB
                    }
                }

                // Convert Sector ID to Name
                if (int.TryParse(model.Ticket.Sector, out int sectorId))
                {
                    var sectorsResponse = await _externalApiService.GetSectorAsync();
                    var sector = sectorsResponse?.data?.FirstOrDefault(s => s.sektorId == sectorId);
                    if (sector != null)
                    {
                        sectorName = sector.sektorName; // Use sektorName for display
                        model.Ticket.Sector = sector.sektorName; // Update model for saving to DB
                    }
                }

                // isYabanci: false for Turkish (tr), true for other languages
                bool isYabanci = Lang != "tr";

                // Call external API through IExternalApiService (use original IDs for API)
                var apiResult = await _externalApiService.AddZiyaretciAsync(
                    model.Ticket.FirstName, model.Ticket.LastName, model.Ticket.Email, model.Ticket.Phone, model.Ticket.Gender,
                    model.Ticket.CompanyName, model.Ticket.Position, sectorName, countryIdForApi, 
                    cityName, isYabanci, FUAR_ID);

                // Store API response details and parse JSON response
                model.Ticket.ApiResponse = apiResult.ResponseContent ?? "";
                // Always save the ticket to database for tracking, regardless of API response

                // Parse JSON response to extract the actual success value
                try 
                {
                    if (!string.IsNullOrEmpty(apiResult.ResponseContent))
                    {
                        using var jsonDoc = System.Text.Json.JsonDocument.Parse(apiResult.ResponseContent);
                        if (jsonDoc.RootElement.TryGetProperty("success", out var successElement))
                        {
                            model.Ticket.ApiSuccess = successElement.GetBoolean();
                            _logger.LogInformation("Parsed API success status for {Email}: {ApiSuccess}", model.Ticket.Email, model.Ticket.ApiSuccess);
                        }
                        else
                        {
                            model.Ticket.ApiSuccess = apiResult.Success;
                            _logger.LogWarning("No 'success' property found in API response for {Email}. Using HTTP success status: {Success}", model.Ticket.Email, apiResult.Success);
                        }
                    }
                    else
                    {
                        model.Ticket.ApiSuccess = apiResult.Success;
                        _logger.LogWarning("Empty API response for {Email}. Using HTTP success status: {Success}", model.Ticket.Email, apiResult.Success);
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    _logger.LogWarning("Failed to parse API response JSON for {Email}: {Error}. Using apiResult.Success instead.", model.Ticket.Email, ex.Message);
                    model.Ticket.ApiSuccess = apiResult.Success;
                }



                if (!model.Ticket.ApiSuccess)
                {
                    _logger.LogWarning("API returned success=false for ticket {Email}. Response: {ResponseContent}", 
                                      model.Ticket.Email, apiResult.ResponseContent);
                    
                    // Extract error message from API response if available
                    string apiErrorMessage = "";
                    try 
                    {
                        if (!string.IsNullOrEmpty(apiResult.ResponseContent))
                        {
                            using var jsonDoc = System.Text.Json.JsonDocument.Parse(apiResult.ResponseContent);
                            if (jsonDoc.RootElement.TryGetProperty("message", out var messageElement))
                            {
                                apiErrorMessage = messageElement.GetString() ?? "";
                            }
                        }
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        // Ignore JSON parsing errors for message extraction
                    }
                    
                    // Show appropriate error message
                    string userErrorMessage = Lang == "en" 
                        ? "Your ticket registration was saved but there was an issue with the external processing. Our team will contact you shortly."
                        : "Bilet kaydınız alındı ancak işlem sırasında bir sorun oluştu. Ekibimiz en kısa sürede sizinle iletişime geçecektir.";
                    
                    TempData["Error"] = userErrorMessage;
                    if (!string.IsNullOrEmpty(apiErrorMessage))
                    {
                        _logger.LogInformation("API error message for {Email}: {ApiError}", model.Ticket.Email, apiErrorMessage);
                    }
                    
                    // Skip sending email when API call fails
                    _logger.LogInformation("Skipping email notification due to API failure for {Email}", model.Ticket.Email);
                    return RedirectToAction(nameof(Index));
                }
                _unitOfWork.Ticket.Add(model.Ticket);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Successfully submitted ticket to API for {Email}", model.Ticket.Email);

                // Send confirmation email to admin (only when API is successful)
                string subject = $"New Ticket Registration - {model.Ticket.CompanyName} - {model.Ticket.FirstName} {model.Ticket.LastName}";
                
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
                        <tr><td><b>{labels.FirstName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.FirstName)}</td></tr>
                        <tr><td><b>{labels.LastName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.LastName)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Email)}</td></tr>
                        <tr><td><b>{labels.Phone}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Phone)}</td></tr>
                        <tr><td><b>{labels.CompanyName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.CompanyName)}</td></tr>
                        <tr><td><b>{labels.Sector}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Sector)}</td></tr>
                        <tr><td><b>{labels.Website}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Website)}</td></tr>
                        <tr><td><b>{labels.Country}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Country)}</td></tr>
                        <tr><td><b>{labels.City}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.City)}</td></tr>
                        <tr><td><b>{labels.Position}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Position)}</td></tr>
                        <tr><td><b>{labels.Gender}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Ticket.Gender)}</td></tr>
                        <tr><td><b>{labels.Date}:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>
                    <br>
                    <p><strong>API Response:</strong> {System.Web.HttpUtility.HtmlEncode(apiResult.ResponseContent)}</p>";

                // Send to the admin email
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
                _logger.LogInformation("Ticket form email sent to admin for {Company} - {FullName}", model.Ticket.CompanyName, $"{model.Ticket.FirstName} {model.Ticket.LastName}");
                
                TempData["Success"] = Lang == "en" ? "Your ticket registration has been submitted successfully!" : "Bilet kaydınız başarıyla gönderildi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process ticket form from {Email}: {ErrorMessage}", model.Ticket.Email, ex.Message);
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while processing your ticket request.";
                return View(model);
            }
        }
    }
}
