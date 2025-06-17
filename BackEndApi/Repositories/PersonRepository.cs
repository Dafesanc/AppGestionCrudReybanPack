using BackEndApi.Data;
using BackEndApi.Interfaces;
using BackEndApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Person?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
        {
            var query = _dbSet.Where(p => p.Email.ToLower() == email.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Person>> SearchByNameAsync(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(p => p.FirstName.ToLower().Contains(lowerSearchTerm) || 
                           p.LastName.ToLower().Contains(lowerSearchTerm))
                .ToListAsync();
        }

        public override async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName)
                .ToListAsync();
        }
    }
}
