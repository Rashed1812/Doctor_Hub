using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class MedicalSpecialty
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم التخصص")]
        [Display(Name = "اسم التخصص")]
        public string Name { get; set; }

        [Display(Name = "وصف التخصص")]
        public string? Description { get; set; }

        [Display(Name = "سعر الاستشارة")]
        [Range(0, double.MaxValue, ErrorMessage = "السعر يجب أن يكون رقم موجب")]
        public decimal? Price { get; set; }

        [Display(Name = "رمز الأيقونة (اختياري)")]
        public string? IconClass { get; set; }
        public bool IsVisibleToPatient { get; set; } = false;

        public ICollection<Consultation>? Consultations { get; set; }
        public ICollection<DoctorSpecialty>? DoctorSpecialties { get; set; }
    }
}
