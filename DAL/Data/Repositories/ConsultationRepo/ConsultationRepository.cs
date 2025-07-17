using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Data.Repositories.ConsultationRepo
{
    public class ConsultationRepository(ClincDbContext _dbContext) : IConsultationRepository
    {
        public async Task<IEnumerable<Consultation>> GetAllAsync()
        {
            return await _dbContext.Consultations.Include(c => c.MedicalSpecialty).ToListAsync();
        }

        public async Task<Consultation?> GetByIdAsync(int id)
        {
            return await _dbContext.Consultations.Include(c => c.MedicalSpecialty).FirstOrDefaultAsync(c => c.ConsultationId == id);
        }

        public async Task AddAsync(Consultation consultation)
        {
            await _dbContext.Consultations.AddAsync(consultation);
        }

        public void Update(Consultation consultation)
        {
            _dbContext.Consultations.Update(consultation);
        }

        public void Delete(Consultation consultation)
        {
            _dbContext.Consultations.Remove(consultation);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}


