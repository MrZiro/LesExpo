using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using LesExpo.web.Models.Configuration;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LesExpo.web.Controllers
{
    [Route("{lang}")]
    public class RegistrationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IUrlLocalizationService _urlService;
        private readonly EmailTemplatesConfig _emailTemplates;
        private readonly string _adminEmail = SD.AdminEmail;
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();
        
        public RegistrationController(IUnitOfWork unitOfWork, IEmailSender emailSender, ILogger<RegistrationController> logger, IOptions<EmailTemplatesConfig> emailTemplates, IUrlLocalizationService urlService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailTemplates = emailTemplates.Value ?? throw new ArgumentNullException(nameof(emailTemplates));
            _urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
        }

        [HttpGet("registration")]
        [HttpGet("kayit")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("pre-registration-form")]
        [HttpGet("on-kayit-formu")]
        public IActionResult OnKayitFormu()
        {
            ViewData["CanonicalUrl"] = _urlService.GetCanonicalUrl("Registration", "OnKayitFormu", Lang);
            ViewData["AlternateUrls"] = _urlService.GetAlternateLanguageUrls("Registration", "OnKayitFormu", Lang);
            
            var viewModel = new RegistrationVM
            {
                Language = Lang
            };
            
            return View(viewModel);
        }

        [HttpPost("pre-registration-form")]
        [HttpPost("on-kayit-formu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnKayitFormu(RegistrationVM model)
        {
            // Get email template for current language
            var emailTemplate = _emailTemplates.GetTemplate("Registration", Lang);
            if (emailTemplate == null)
            {
                _logger.LogError("Registration email template not found for language: {Language}", Lang);
                emailTemplate = _emailTemplates.GetTemplate("Registration", "tr"); // Fallback to Turkish
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = emailTemplate?.ValidationMessage ?? "Please fill in all required fields correctly.";
                return View(model:model);
            }

            try
            {
                _logger.LogInformation("Processing registration form submission from {Email} in language {Language}", model.Registration.Email, Lang);
                
                // Set language and creation date
                model.Registration.Language = Lang;
                model.Registration.CreatedAt = DateTime.Now;
                
                // Serialize fair participation data to JSON
                if (model.UlusalFuarlar != null && model.UlusalFuarlar.Count > 0)
                {
                    model.Registration.UlusalFuarlarJson = JsonSerializer.Serialize(model.UlusalFuarlar);
                }
                
                if (model.UluslararasiFuarlar != null && model.UluslararasiFuarlar.Count > 0)
                {
                    model.Registration.UluslararasiFuarlarJson = JsonSerializer.Serialize(model.UluslararasiFuarlar);
                }
                
                // Sanitize inputs
                model.Registration.SirketAdi = model.Registration.SirketAdi?.Trim();
                model.Registration.AdSoyad = model.Registration.AdSoyad?.Trim();
                model.Registration.Gorev = model.Registration.Gorev?.Trim();
                model.Registration.Email = model.Registration.Email?.Trim();

                // Try to convert IDs to names for displaying in email
                string ulkeAdi = model.Registration.Ulke;
                string sehirAdi = model.Registration.Sehir;
                string faaliyetAlaniAdi = model.Registration.FaaliyetAlani;

                // Use localized email template
                string subject = emailTemplate.FormatSubject($"{model.Registration.SirketAdi} - {model.Registration.AdSoyad}");
                
                // Generate field labels based on language
                var labels = Lang == "en" 
                    ? new {
                        CompanyName = "Company Name", FullName = "Full Name", Position = "Position",
                        CompanyAddress = "Company Address", Country = "Country", City = "City",
                        Phone = "Phone", Email = "Email", Website = "Website", ActivityField = "Activity Field",
                        ProductGroup = "Product Group", Brands = "Brands", RequestedArea = "Requested Area",
                        EstablishmentDate = "Establishment Date", ActivityType = "Activity Type",
                        ExportTurnover = "Export Turnover", TotalTurnover = "Total Turnover",
                        FairParticipation = "Fair Participation", CompanyVisit = "Company Visit",
                        StaffCount = "Staff Count", Date = "Date", 
                        NationalFairs = "National Fairs Participated", InternationalFairs = "International Fairs Participated"
                    }
                    : new {
                        CompanyName = "Şirket Adı", FullName = "Ad Soyad", Position = "Görev",
                        CompanyAddress = "Şirket Adresi", Country = "Ülke", City = "Şehir",
                        Phone = "Telefon", Email = "E-posta", Website = "Web Sitesi", ActivityField = "Faaliyet Alanı",
                        ProductGroup = "Ürün Grubu", Brands = "Markalar", RequestedArea = "İstenen Metrekare",
                        EstablishmentDate = "Kuruluş Tarihi", ActivityType = "Aktivite Türü",
                        ExportTurnover = "İhracat Cirosu", TotalTurnover = "Toplam Ciro",
                        FairParticipation = "Fuar Katılımı", CompanyVisit = "Firma Ziyareti",
                        StaffCount = "Personel Sayısı", Date = "Tarih",
                        NationalFairs = "Katılım Sağlanan Ulusal Fuarlar", InternationalFairs = "Katılım Sağlanan Uluslararası Fuarlar"
                    };
                
                string htmlMessage = $@"
                    {emailTemplate.EmailBody}
                    <table border='0' cellpadding='5'>
                        <tr><td><b>{labels.CompanyName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.SirketAdi)}</td></tr>
                        <tr><td><b>{labels.FullName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.AdSoyad)}</td></tr>
                        <tr><td><b>{labels.Position}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.Gorev)}</td></tr>
                        <tr><td><b>{labels.CompanyAddress}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.SirketAdresi)}</td></tr>
                        <tr><td><b>{labels.Country}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(ulkeAdi)}</td></tr>
                        <tr><td><b>{labels.City}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(sehirAdi)}</td></tr>
                        <tr><td><b>{labels.Phone}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.Telefon)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.Email)}</td></tr>
                        <tr><td><b>{labels.Website}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.WebSitesi)}</td></tr>
                        <tr><td><b>{labels.ActivityField}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(faaliyetAlaniAdi)}</td></tr>
                        <tr><td><b>{labels.ProductGroup}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.UrunGrubu)}</td></tr>
                        <tr><td><b>{labels.Brands}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.Markalar)}</td></tr>
                        <tr><td><b>{labels.RequestedArea}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.IstenenMetrekare)}</td></tr>
                        <tr><td><b>{labels.EstablishmentDate}:</b></td><td>{model.Registration.KurulusTarihi:dd.MM.yyyy}</td></tr>
                        <tr><td><b>{labels.ActivityType}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.AktiviteTuru)}</td></tr>
                        <tr><td><b>{labels.ExportTurnover}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.IhracatCirosu)} USD</td></tr>
                        <tr><td><b>{labels.TotalTurnover}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.ToplamCiro)} USD</td></tr>
                        <tr><td><b>{labels.FairParticipation}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.FuarKatilim)}</td></tr>
                        <tr><td><b>{labels.CompanyVisit}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Registration.FirmaZiyareti)}</td></tr>
                        <tr><td><b>{labels.StaffCount}:</b></td><td>{model.Registration.PersonelSayisi}</td></tr>
                        <tr><td><b>{labels.Date}:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>";

                if (model.UlusalFuarlar != null && model.UlusalFuarlar.Count > 0)
                {
                    htmlMessage += $"<h3>{labels.NationalFairs}</h3><ul>";
                    foreach (var fuar in model.UlusalFuarlar)
                    {
                        htmlMessage += $"<li>{System.Web.HttpUtility.HtmlEncode(fuar.FuarAdi)} - {System.Web.HttpUtility.HtmlEncode(fuar.KatilimYili)}</li>";
                    }
                    htmlMessage += "</ul>";
                }

                if (model.UluslararasiFuarlar != null && model.UluslararasiFuarlar.Count > 0)
                {
                    htmlMessage += $"<h3>{labels.InternationalFairs}</h3><ul>";
                    foreach (var fuar in model.UluslararasiFuarlar)
                    {
                        htmlMessage += $"<li>{System.Web.HttpUtility.HtmlEncode(fuar.FuarAdi)} - {System.Web.HttpUtility.HtmlEncode(fuar.KatilimYili)}</li>";
                    }
                    htmlMessage += "</ul>";
                }
                
                // Save to database
                _unitOfWork.Registration.Add(model.Registration);
                await _unitOfWork.SaveAsync();
                
                _logger.LogInformation("Registration saved to database for {Company} - {FullName} with ID {Id}", 
                    model.Registration.SirketAdi, model.Registration.AdSoyad, model.Registration.Id);

                // Send to the admin email
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
                _logger.LogInformation("Registration form email sent to admin for {Company} - {FullName}", model.Registration.SirketAdi, model.Registration.AdSoyad);
                
                // Send confirmation to the user
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>{emailTemplate.FormatGreeting(model.Registration.AdSoyad)}</h2>
                        <p>{emailTemplate.ConfirmationText}</p>
                        <p><b>{labels.CompanyName}:</b> {System.Web.HttpUtility.HtmlEncode(model.Registration.SirketAdi)}</p>
                        <p><b>{labels.RequestedArea}:</b> {System.Web.HttpUtility.HtmlEncode(model.Registration.IstenenMetrekare)} m²</p>
                        <p><b>{labels.Date}:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>{emailTemplate.ConfirmationFooter}</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Registration.Email, emailTemplate.ConfirmationSubject, confirmationHtmlMessage);
                _logger.LogInformation("Registration confirmation email sent to user {Email} in language {Language}", model.Registration.Email, Lang);
                
                TempData["Success"] = emailTemplate.SuccessMessage;
                return RedirectToAction(nameof(OnKayitFormu));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process registration form from {Email}: {ErrorMessage}", model.Registration.Email, ex.Message);
                
                // Try to save to database even if email fails
                try
                {
                    // Set language and creation date if not already set
                    if (string.IsNullOrEmpty(model.Registration.Language))
                    {
                        model.Registration.Language = Lang;
                        model.Registration.CreatedAt = DateTime.Now;
                    }
                    
                    // Serialize fair participation data to JSON if not already serialized
                    if (string.IsNullOrEmpty(model.Registration.UlusalFuarlarJson) && model.UlusalFuarlar != null && model.UlusalFuarlar.Count > 0)
                    {
                        model.Registration.UlusalFuarlarJson = JsonSerializer.Serialize(model.UlusalFuarlar);
                    }
                    
                    if (string.IsNullOrEmpty(model.Registration.UluslararasiFuarlarJson) && model.UluslararasiFuarlar != null && model.UluslararasiFuarlar.Count > 0)
                    {
                        model.Registration.UluslararasiFuarlarJson = JsonSerializer.Serialize(model.UluslararasiFuarlar);
                    }
                    
                    _unitOfWork.Registration.Add(model.Registration);
                    await _unitOfWork.SaveAsync();
                    
                    _logger.LogInformation("Registration saved to database despite email error for {Company} - {FullName} with ID {Id}", 
                        model.Registration.SirketAdi, model.Registration.AdSoyad, model.Registration.Id);
                }
                catch (Exception dbEx)
                {
                    _logger.LogError(dbEx, "Failed to save registration to database for {Email}: {ErrorMessage}", model.Registration.Email, dbEx.Message);
                }
                
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while sending the form.";
                return View(model:model);
            }
        }
    }
}
