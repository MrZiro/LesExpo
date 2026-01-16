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
        ISliderRepository Slider { get; }
        IContactRepository Contact { get; }
        ITicketRepository Ticket { get; }
        IRegistrationRepository Registration { get; }

        void Save();
        Task SaveAsync();
    }
}
