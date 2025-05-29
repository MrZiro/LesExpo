using System.Collections.Generic;
using LesExpo.Models;

namespace LesExpo.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
