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
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        private ApplicationDbContext _db;

        public ContactRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Contact contact)
        {
            _db.Contacts.Update(contact);
        }

        public IEnumerable<Contact> GetContactsByLanguage(string language)
        {
            return _db.Contacts.Where(c => c.Language == language).OrderByDescending(c => c.CreatedAt);
        }
    }
} 