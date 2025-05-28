using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesExpo.Utility;

namespace LesExpo.Models
{
    public class ContentType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Field)]
        [MaxLength(50, ErrorMessage = "İçerik türü adı 50 karakterden uzun olamaz")]
        [DisplayName("İçerik Türü")]
        public string Name { get; set; }
    }
}
