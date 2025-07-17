using DAL.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Data.Repositories.ConsultationRepo
{
    public interface IConsultationRepository
    {
        Task<IEnumerable<Consultation>> GetAllAsync();
        Task<Consultation?> GetByIdAsync(int id);
        Task AddAsync(Consultation consultation);
        void Update(Consultation consultation);
        void Delete(Consultation consultation);
        Task SaveAsync();
    }
}


