using LesExpo.Models;

namespace LesExpo.DataAccess.Repository.IRepository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void Update(Ticket obj);
        IEnumerable<Ticket> GetTicketsByLanguage(string language);
    }
} 