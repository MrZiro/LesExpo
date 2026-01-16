using LesExpo.Models;

namespace LesExpo.DataAccess.Repository.IRepository
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        void Update(Registration obj);
        IEnumerable<Registration> GetRegistrationsByLanguage(string language);
    }
} 