using BackEndApi.Models;

namespace BackEndApi.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<Person?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
        Task<IEnumerable<Person>> SearchByNameAsync(string searchTerm);
    }
}
