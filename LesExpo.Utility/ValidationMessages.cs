using System;

namespace LesExpo.Utility
{
    public static class ValidationMessages
    {
        // Required field messages
        public const string Required_Field = "{0} alanı gereklidir";
        
        // Length validation messages
        public const string MaxLength = "{0} {1} karakterden uzun olamaz";
        
        // Comparison validation messages
        public const string ComparePassword = "Şifreler eşleşmiyor";
        
        // Custom validation messages
        public const string InvalidLogin = "Geçersiz giriş denemesi";
        public const string ImageUploadFailed = "Görsel yüklenirken hata oluştu. Lütfen tekrar deneyin";
        public const string SelectImage = "Lütfen bir görsel seçin";
        public const string ImageSizeExceeded = "Görsel boyutu {0}MB'ı geçemez";
        public const string InvalidImageFormat = "Yalnızca görsel dosyaları (jpg, jpeg, png, gif, webp) kabul edilir";
    }
}