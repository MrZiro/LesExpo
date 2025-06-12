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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IUrlLocalizationService _urlService;
        private readonly EmailTemplatesConfig _emailTemplates;
        private readonly string _adminEmail = "adobe.mrziro@gmail.com";
        protected string Lang => (RouteData.Values["lang"]?.ToString() ?? "tr").ToLower();
        
        public RegistrationController(IEmailSender emailSender, ILogger<RegistrationController> logger, IOptions<EmailTemplatesConfig> emailTemplates, IUrlLocalizationService urlService)
        {
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
            return View();
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
                _logger.LogInformation("Processing registration form submission from {Email} in language {Language}", model.Email, Lang);
                
                // Sanitize inputs
                model.SirketAdi = model.SirketAdi?.Trim();
                model.AdSoyad = model.AdSoyad?.Trim();
                model.Gorev = model.Gorev?.Trim();
                model.Email = model.Email?.Trim();

                // Try to convert IDs to names for displaying in email
                string ulkeAdi = model.Ulke;
                string sehirAdi = model.Sehir;
                string faaliyetAlaniAdi = model.FaaliyetAlani;

                // Use localized email template
                string subject = emailTemplate.FormatSubject($"{model.SirketAdi} - {model.AdSoyad}");
                
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
                        <tr><td><b>{labels.CompanyName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.SirketAdi)}</td></tr>
                        <tr><td><b>{labels.FullName}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.AdSoyad)}</td></tr>
                        <tr><td><b>{labels.Position}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Gorev)}</td></tr>
                        <tr><td><b>{labels.CompanyAddress}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.SirketAdresi)}</td></tr>
                        <tr><td><b>{labels.Country}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(ulkeAdi)}</td></tr>
                        <tr><td><b>{labels.City}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(sehirAdi)}</td></tr>
                        <tr><td><b>{labels.Phone}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Telefon)}</td></tr>
                        <tr><td><b>{labels.Email}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Email)}</td></tr>
                        <tr><td><b>{labels.Website}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.WebSitesi)}</td></tr>
                        <tr><td><b>{labels.ActivityField}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(faaliyetAlaniAdi)}</td></tr>
                        <tr><td><b>{labels.ProductGroup}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.UrunGrubu)}</td></tr>
                        <tr><td><b>{labels.Brands}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Markalar)}</td></tr>
                        <tr><td><b>{labels.RequestedArea}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.IstenenMetrekare)}</td></tr>
                        <tr><td><b>{labels.EstablishmentDate}:</b></td><td>{model.KurulusTarihi:dd.MM.yyyy}</td></tr>
                        <tr><td><b>{labels.ActivityType}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.AktiviteTuru)}</td></tr>
                        <tr><td><b>{labels.ExportTurnover}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.IhracatCirosu)} USD</td></tr>
                        <tr><td><b>{labels.TotalTurnover}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.ToplamCiro)} USD</td></tr>
                        <tr><td><b>{labels.FairParticipation}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.FuarKatilim)}</td></tr>
                        <tr><td><b>{labels.CompanyVisit}:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.FirmaZiyareti)}</td></tr>
                        <tr><td><b>{labels.StaffCount}:</b></td><td>{model.PersonelSayisi}</td></tr>
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
                
                // Send to the admin email
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
                _logger.LogInformation("Registration form email sent to admin for {Company} - {FullName}", model.SirketAdi, model.AdSoyad);
                
                // Send confirmation to the user
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>{emailTemplate.FormatGreeting(model.AdSoyad)}</h2>
                        <p>{emailTemplate.ConfirmationText}</p>
                        <p><b>{labels.CompanyName}:</b> {System.Web.HttpUtility.HtmlEncode(model.SirketAdi)}</p>
                        <p><b>{labels.RequestedArea}:</b> {System.Web.HttpUtility.HtmlEncode(model.IstenenMetrekare)} m²</p>
                        <p><b>{labels.Date}:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>{emailTemplate.ConfirmationFooter}</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Email, emailTemplate.ConfirmationSubject, confirmationHtmlMessage);
                _logger.LogInformation("Registration confirmation email sent to user {Email} in language {Language}", model.Email, Lang);
                
                TempData["Success"] = emailTemplate.SuccessMessage;
                return RedirectToAction(nameof(OnKayitFormu));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process registration form from {Email}: {ErrorMessage}", model.Email, ex.Message);
                TempData["Error"] = emailTemplate?.ErrorMessage ?? "An error occurred while sending the form.";
                return View(model:model);
            }
        }
    }
}
