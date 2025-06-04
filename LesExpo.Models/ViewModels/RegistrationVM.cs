using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LesExpo.Models.ViewModels
{
    public class RegistrationVM
    {
        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        public string SirketAdi { get; set; }

        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Görev zorunludur.")]
        public string Gorev { get; set; }

        [Required(ErrorMessage = "Şirket adresi zorunludur.")]
        public string SirketAdresi { get; set; }

        [Required(ErrorMessage = "Ülke zorunludur.")]
        public string Ulke { get; set; }

        [Required(ErrorMessage = "Şehir zorunludur.")]
        public string Sehir { get; set; }

        [Required(ErrorMessage = "Telefon zorunludur.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        public string WebSitesi { get; set; }

        [Required(ErrorMessage = "Faaliyet alanı zorunludur.")]
        public string FaaliyetAlani { get; set; }

        [Required(ErrorMessage = "Ürün grubu zorunludur.")]
        public string UrunGrubu { get; set; }

        [Required(ErrorMessage = "Markalar zorunludur.")]
        public string Markalar { get; set; }

        [Required(ErrorMessage = "İstenen metrekare zorunludur.")]
        public string IstenenMetrekare { get; set; }

        [Required(ErrorMessage = "Kuruluş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime KurulusTarihi { get; set; }

        [Required(ErrorMessage = "Aktivite türü zorunludur.")]
        public string AktiviteTuru { get; set; }

        [Required(ErrorMessage = "İhracat cirosu zorunludur.")]
        public string IhracatCirosu { get; set; }

        [Required(ErrorMessage = "Toplam ciro zorunludur.")]
        public string ToplamCiro { get; set; }

        [Required(ErrorMessage = "Personel sayısı zorunludur.")]
        public int PersonelSayisi { get; set; }

        [Required(ErrorMessage = "Fuar Katılımı zorunludur.")]
        public string FuarKatilim { get; set; }

        [Required(ErrorMessage = "Firma Ziyareti zorunludur.")]
        public string FirmaZiyareti { get; set; }

        public List<FuarKatilimVM> UlusalFuarlar { get; set; }
        public List<FuarKatilimVM> UluslararasiFuarlar { get; set; }

        public RegistrationVM()
        {
            UlusalFuarlar = new List<FuarKatilimVM>();
            UluslararasiFuarlar = new List<FuarKatilimVM>();
        }
    }

    public class FuarKatilimVM
    {
        public string FuarAdi { get; set; }
        public string KatilimYili { get; set; }
    }
} 