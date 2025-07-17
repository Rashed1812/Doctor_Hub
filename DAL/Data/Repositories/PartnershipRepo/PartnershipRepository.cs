using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Data.Repositories.PartnershipRepo
{
    public class PartnershipRepository(ClincDbContext _dbContext) : IPartnershipRepository
    {
        public async Task<IEnumerable<Partnership>> GetAllAsync()
        {
            return await _dbContext.Partnerships.ToListAsync();
        }

        public async Task<Partnership?> GetByIdAsync(int id)
        {
            return await _dbContext.Partnerships.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Partnership partnership)
        {
            await _dbContext.Partnerships.AddAsync(partnership);
        }

        public void Update(Partnership partnership)
        {
            _dbContext.Partnerships.Update(partnership);
        }

        public void Delete(Partnership partnership)
        {
            _dbContext.Partnerships.Remove(partnership);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}


