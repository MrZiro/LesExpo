using LesExpo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository.IRepository
{
    public interface ISliderRepository : IRepository<Slider>
    {
        public void Update(Slider obj);
    }
}
