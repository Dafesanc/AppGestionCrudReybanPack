using BackEndApi.Data;
using BackEndApi.Interfaces;
using BackEndApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Pet>> GetBySpeciesAsync(string species)
        {
            return await _dbSet
                .Where(p => p.Species.ToLower() == species.ToLower())
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> SearchByNameAsync(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(p => p.Name.ToLower().Contains(lowerSearchTerm))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetByAgeRangeAsync(int minAge, int maxAge)
        {
            return await _dbSet
                .Where(p => p.Age >= minAge && p.Age <= maxAge)
                .OrderBy(p => p.Age)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Pet>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(p => p.Species)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }
    }
}
