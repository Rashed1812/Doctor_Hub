using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace DAL.Data.Repositories.DoctorRequest
{
    public interface IDoctorJoinRequestRepo
    {
        Task<IEnumerable<DoctorJoinRequest>> GetAllAsync();
        Task<DoctorJoinRequest?> GetByIdAsync(int id);
        Task AddAsync(DoctorJoinRequest doctor);
        void Update(DoctorJoinRequest doctor);
        void Delete(DoctorJoinRequest doctor);
        Task SaveAsync();
    }
}
