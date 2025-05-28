using LesExpo.Models;
using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;
        
        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider obj)
        {
            var objFromDb = _db.Sliders.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Subtitle = obj.Subtitle;
                objFromDb.Description = obj.Description;
                objFromDb.ButtonText = obj.ButtonText;
                objFromDb.ButtonUrl = obj.ButtonUrl;
                objFromDb.DisplayOrder = obj.DisplayOrder;
                objFromDb.IsActive = obj.IsActive;
                objFromDb.UpdatedDate = DateTime.Now;
                
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
