using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{
    public class ContactVM
    {
        public Contact Contact { get; set; } = new Contact();
        
        /// <summary>
        /// Language for the form (used for validation messages and email templates)
        /// </summary>
        public string Language { get; set; } = "tr";
    }
}
