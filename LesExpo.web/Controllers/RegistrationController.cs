using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LesExpo.web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly string _adminEmail = "adobe.mrziro@gmail.com";
        
        public RegistrationController(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OnKayitFormu()
        {
            return View("on-kayit-formu");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnKayitFormu(RegistrationVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen tüm zorunlu alanları doğru şekilde doldurunuz.";
                return View("on-kayit-formu",model);
            }

            try
            {
                // Sanitize inputs
                model.SirketAdi = model.SirketAdi?.Trim();
                model.AdSoyad = model.AdSoyad?.Trim();
                model.Gorev = model.Gorev?.Trim();
                model.Email = model.Email?.Trim();

                // Try to convert IDs to names for displaying in email
                string ulkeAdi = model.Ulke;
                string sehirAdi = model.Sehir;
                string faaliyetAlaniAdi = model.FaaliyetAlani;

                string subject = $"[Ön Kayıt Formu] {model.SirketAdi} - {model.AdSoyad}";
                string htmlMessage = $@"
                    <h2>Ön Kayıt Formu Bilgileri</h2>
                    <table border='0' cellpadding='5'>
                        <tr><td><b>Şirket Adı:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.SirketAdi)}</td></tr>
                        <tr><td><b>Ad Soyad:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.AdSoyad)}</td></tr>
                        <tr><td><b>Görev:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Gorev)}</td></tr>
                        <tr><td><b>Şirket Adresi:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.SirketAdresi)}</td></tr>
                        <tr><td><b>Ülke:</b></td><td>{System.Web.HttpUtility.HtmlEncode(ulkeAdi)}</td></tr>
                        <tr><td><b>Şehir:</b></td><td>{System.Web.HttpUtility.HtmlEncode(sehirAdi)}</td></tr>
                        <tr><td><b>Telefon:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Telefon)}</td></tr>
                        <tr><td><b>E-posta:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Email)}</td></tr>
                        <tr><td><b>Web Sitesi:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.WebSitesi)}</td></tr>
                        <tr><td><b>Faaliyet Alanı:</b></td><td>{System.Web.HttpUtility.HtmlEncode(faaliyetAlaniAdi)}</td></tr>
                        <tr><td><b>Ürün Grubu:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.UrunGrubu)}</td></tr>
                        <tr><td><b>Markalar:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.Markalar)}</td></tr>
                        <tr><td><b>İstenen Metrekare:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.IstenenMetrekare)}</td></tr>
                        <tr><td><b>Kuruluş Tarihi:</b></td><td>{model.KurulusTarihi:dd.MM.yyyy}</td></tr>
                        <tr><td><b>Aktivite Türü:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.AktiviteTuru)}</td></tr>
                        <tr><td><b>İhracat Cirosu:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.IhracatCirosu)} USD</td></tr>
                        <tr><td><b>Toplam Ciro:</b></td><td>{System.Web.HttpUtility.HtmlEncode(model.ToplamCiro)} USD</td></tr>
                        <tr><td><b>Personel Sayısı:</b></td><td>{model.PersonelSayisi}</td></tr>
                        <tr><td><b>Tarih:</b></td><td>{DateTime.Now:dd.MM.yyyy HH:mm}</td></tr>
                    </table>";

                if (model.UlusalFuarlar != null && model.UlusalFuarlar.Count > 0)
                {
                    htmlMessage += "<h3>Katılım Sağlanan Ulusal Fuarlar</h3><ul>";
                    foreach (var fuar in model.UlusalFuarlar)
                    {
                        htmlMessage += $"<li>{System.Web.HttpUtility.HtmlEncode(fuar.FuarAdi)} - {System.Web.HttpUtility.HtmlEncode(fuar.KatilimYili)}</li>";
                    }
                    htmlMessage += "</ul>";
                }

                if (model.UluslararasiFuarlar != null && model.UluslararasiFuarlar.Count > 0)
                {
                    htmlMessage += "<h3>Katılım Sağlanan Uluslararası Fuarlar</h3><ul>";
                    foreach (var fuar in model.UluslararasiFuarlar)
                    {
                        htmlMessage += $"<li>{System.Web.HttpUtility.HtmlEncode(fuar.FuarAdi)} - {System.Web.HttpUtility.HtmlEncode(fuar.KatilimYili)}</li>";
                    }
                    htmlMessage += "</ul>";
                }
                
                // Send to the admin email
                await _emailSender.SendEmailAsync(_adminEmail, subject, htmlMessage);
                
                // Send confirmation to the user
                string confirmationSubject = "Ön Kayıt Formunuz Alındı - LES-EXPO";
                string confirmationHtmlMessage = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h2 style='color: #333;'>Sayın {System.Web.HttpUtility.HtmlEncode(model.AdSoyad)},</h2>
                        <p>Ön kayıt formunuz başarıyla alınmıştır. En kısa sürede sizinle iletişime geçeceğiz.</p>
                        <p>Firma bilgileriniz:</p>
                        <p><b>Şirket Adı:</b> {System.Web.HttpUtility.HtmlEncode(model.SirketAdi)}</p>
                        <p><b>İstenen Alan:</b> {System.Web.HttpUtility.HtmlEncode(model.IstenenMetrekare)} m²</p>
                        <p><b>Tarih:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p>İyi günler dileriz,<br/>LES-EXPO Ekibi</p>
                    </div>";
                
                await _emailSender.SendEmailAsync(model.Email, confirmationSubject, confirmationHtmlMessage);
                
                TempData["Success"] = "Ön kayıt formunuz başarıyla gönderildi. En kısa sürede sizinle iletişime geçeceğiz.";
                return RedirectToAction(nameof(OnKayitFormu));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process registration form: {ex.Message}");
                TempData["Error"] = "Form gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                return View("on-kayit-formu",model);
            }
        }
    }
}
