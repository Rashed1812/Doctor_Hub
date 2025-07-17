using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Data.Repositories.PartnershipRepo
{
    public interface IPartnershipRepository
    {
        Task<IEnumerable<Partnership>> GetAllAsync();
        Task<Partnership?> GetByIdAsync(int id);
        Task AddAsync(Partnership partnership);
        void Update(Partnership partnership);
        void Delete(Partnership partnership);
        Task SaveAsync();
    }
}


