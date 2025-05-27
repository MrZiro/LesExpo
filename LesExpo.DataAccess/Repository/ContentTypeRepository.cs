using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository
{
    public class ContentTypeRepository : Repository<ContentType>, IContentTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ContentTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ContentType obj)
        {
            _db.ContentTypes.Update(obj);
        }
    }
}
