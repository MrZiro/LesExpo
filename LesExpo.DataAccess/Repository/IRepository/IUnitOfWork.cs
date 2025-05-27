using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IContentTypeRepository ContentType { get; }
        IBlogRepository Blog { get; }

        void Save();
        Task SaveAsync();
    }
}
