using LesExpo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository.IRepository
{
    public interface IBlogRepository : IRepository<Blog>
    {
        public void Update(Blog obj);

        IEnumerable<Blog> GetBlogsByLanguage(string language);
    }
}
