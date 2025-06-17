using BackEndApi.Models;

namespace BackEndApi.Interfaces
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        Task<IEnumerable<Pet>> GetBySpeciesAsync(string species);
        Task<IEnumerable<Pet>> SearchByNameAsync(string searchTerm);
        Task<IEnumerable<Pet>> GetByAgeRangeAsync(int minAge, int maxAge);
    }
}
