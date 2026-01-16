using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;

namespace LesExpo.DataAccess.Repository
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        private ApplicationDbContext _db;
        public RegistrationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Registration obj)
        {
            _db.Registrations.Update(obj);
        }

        public IEnumerable<Registration> GetRegistrationsByLanguage(string language)
        {
            return _db.Registrations.Where(r => r.Language == language).OrderByDescending(r => r.CreatedAt);
        }
    }
} 