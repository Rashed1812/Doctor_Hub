using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace DAL.Data.Repositories.MedicalRepo
{
    public interface IMedicalSpecialtyRepository
    {
        Task<IEnumerable<MedicalSpecialty>> GetAllAsync();
        Task<MedicalSpecialty?> GetByIdAsync(int id);
        Task AddAsync(MedicalSpecialty entity);
        void Update(MedicalSpecialty entity);
        void Delete(MedicalSpecialty entity);
        Task SaveAsync();
    }
}
