using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IContentTypeRepository ContentType { get; private set; }
        public IBlogRepository Blog { get; private set; }
        public ISliderRepository Slider { get; private set; }
        public IContactRepository Contact { get; private set; }
        public ITicketRepository Ticket { get; private set; }
        public IRegistrationRepository Registration { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ContentType = new ContentTypeRepository(_db);
            Blog = new BlogRepository(_db);
            Slider = new SliderRepository(_db);
            Contact = new ContactRepository(_db);
            Ticket = new TicketRepository(_db);
            Registration = new RegistrationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
