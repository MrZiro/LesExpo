using System.ComponentModel.DataAnnotations;
using LesExpo.Utility;

namespace LesExpo.Models.ViewModels
{
    public class TicketVM
    {
        public Ticket Ticket { get; set; } = new Ticket();
        
        /// <summary>
        /// Language for the form (used for validation messages and email templates)
        /// </summary>
        public string Language { get; set; } = "tr";
    }
} 