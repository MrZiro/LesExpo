using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Models.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }

        [ValidateNever]
        public IFormFile? CardImage { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? ContentTypeList { get; set; }
    }
}
