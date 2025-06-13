using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{
    public class RegistrationVM
    {
        public Registration Registration { get; set; } = new Registration();
        
        /// <summary>
        /// Language for the form (used for validation messages and email templates)
        /// </summary>
        public string Language { get; set; } = "tr";

        // Additional properties for fair participation that won't be stored directly
        public List<FuarKatilimVM> UlusalFuarlar { get; set; } = new List<FuarKatilimVM>();
        public List<FuarKatilimVM> UluslararasiFuarlar { get; set; } = new List<FuarKatilimVM>();
    }

    public class FuarKatilimVM
    {
        public string FuarAdi { get; set; } = string.Empty;
        public string KatilimYili { get; set; } = string.Empty;
    }
} 