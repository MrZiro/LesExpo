using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{
    public class RegistrationVM
    {
        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string SirketAdi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Gorev { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string SirketAdresi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Ulke { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Sehir { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Telefon { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        [EmailAddress(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Email { get; set; }

        public string WebSitesi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string FaaliyetAlani { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string UrunGrubu { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string Markalar { get; set; }
        public string IstenenMetrekare { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        [DataType(DataType.Date)]
        public DateTime KurulusTarihi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string AktiviteTuru { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string IhracatCirosu { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string ToplamCiro { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public int PersonelSayisi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
        public string FuarKatilim { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field_Message)]
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