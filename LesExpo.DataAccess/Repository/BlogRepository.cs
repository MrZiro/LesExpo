using LesExpo.Models;
using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {

        private readonly ApplicationDbContext _db;
        public BlogRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Blog obj)
        {
            var objFromDb = _db.Blogs.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Slug = obj.Slug;
                objFromDb.Content = obj.Content;
                objFromDb.UpdatedAt = DateTime.Now;
                objFromDb.CreatedAt = obj.CreatedAt;
                objFromDb.IsPublished = obj.IsPublished;
                if (obj.CardImageUrl != null)
                {
                    objFromDb.CardImageUrl = obj.CardImageUrl;
                }
                objFromDb.MetaDescription = obj.MetaDescription;
                objFromDb.MetaKeywords = obj.MetaKeywords;
                objFromDb.Author = obj.Author;
                objFromDb.ContentTypeId = obj.ContentTypeId;

            }
        }
    }
}
