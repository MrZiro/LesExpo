using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models.ViewModels
{
    public class SliderVM
    {
        public Slider Slider { get; set; }
        
        [ValidateNever]
        [Display(Name = "Slider Image")]
        public IFormFile ImageFile { get; set; }
    }
}
