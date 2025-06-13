using System;
using System.ComponentModel.DataAnnotations;
using LesExpo.Utility;

namespace LesExpo.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(200)]
        public string SirketAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string Gorev { get; set; } = string.Empty;

        
        [StringLength(500)]
        public string SirketAdresi { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string Ulke { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string Sehir { get; set; } = string.Empty;

        
        [StringLength(20)]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [StringLength(200)]
        public string WebSitesi { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(200)]
        public string FaaliyetAlani { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(200)]
        public string UrunGrubu { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(500)]
        public string Markalar { get; set; } = string.Empty;

        [StringLength(50)]
        public string IstenenMetrekare { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        public DateTime KurulusTarihi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string AktiviteTuru { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string IhracatCirosu { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string ToplamCiro { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        public int PersonelSayisi { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string FuarKatilim { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [StringLength(100)]
        public string FirmaZiyareti { get; set; } = string.Empty;

        // Store fair participation data as JSON
        [StringLength(2000)]
        public string UlusalFuarlarJson { get; set; } = string.Empty;

        [StringLength(2000)]
        public string UluslararasiFuarlarJson { get; set; } = string.Empty;

        [StringLength(5)]
        public string Language { get; set; } = "tr";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
