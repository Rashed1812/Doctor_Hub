using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class DoctorSpecialty
    {
        public int DoctorJoinRequestId { get; set; }
        public DoctorJoinRequest DoctorJoinRequest { get; set; }
        public int MedicalSpecialtyId { get; set; }
        public MedicalSpecialty MedicalSpecialty { get; set; }
    }
}
