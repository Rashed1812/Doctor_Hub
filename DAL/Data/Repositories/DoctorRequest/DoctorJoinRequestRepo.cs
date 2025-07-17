using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories.DoctorRequest
{
    public class DoctorJoinRequestRepo(ClincDbContext _dbContext) : IDoctorJoinRequestRepo
    {
        public async Task<IEnumerable<DoctorJoinRequest>> GetAllAsync()
        {
            return await _dbContext.DoctorJoinRequests
                .Include(d => d.DoctorSpecialties)
                    .ThenInclude(ds => ds.MedicalSpecialty)
                .ToListAsync();
        }

        public async Task<DoctorJoinRequest?> GetByIdAsync(int id)
        {
            return await _dbContext.DoctorJoinRequests
                .Include(d => d.DoctorSpecialties)
                    .ThenInclude(ds => ds.MedicalSpecialty)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(DoctorJoinRequest doctor)
        {
            await _dbContext.DoctorJoinRequests.AddAsync(doctor);
        }

        public void Update(DoctorJoinRequest doctor)
        {
            _dbContext.DoctorJoinRequests.Update(doctor);
        }

        public void Delete(DoctorJoinRequest doctor)
        {
            _dbContext.DoctorJoinRequests.Remove(doctor);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
