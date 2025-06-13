using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;

namespace LesExpo.DataAccess.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private ApplicationDbContext _db;
        public TicketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Ticket obj)
        {
            _db.Tickets.Update(obj);
        }

        public IEnumerable<Ticket> GetTicketsByLanguage(string language)
        {
            return _db.Tickets.Where(c => c.Language == language).OrderByDescending(c => c.CreatedAt);
        }
    }
} 