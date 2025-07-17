using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DAL.Data.Models
{
    public class DoctorJoinRequest
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }

        [Required, EmailAddress, Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }


        [Required, Display(Name = "رقم الترخيص")]
        public string LicenseNumber { get; set; }

        [Required, Display(Name = "سنوات الخبرة")]
        public int ExperienceYears { get; set; }

        [Required, Display(Name = "المؤهل التعليمي")]
        public string EducationalDegree { get; set; }

        [Display(Name = "الشهادات والدورات")]
        public string? CertificatesAndCourses { get; set; }

        [Required]
        [Display(Name = "نبذة شخصية")]
        public string Biography { get; set; }

        [Display(Name = "مسار شهادة التخرج")]
        public string? GraduationCertificatePath { get; set; }

        [Display(Name = "مسار شهادة التخصص")]
        public string? CertificateFilePath { get; set; }

        [Display(Name = "مسار رخصة المزاولة")]
        public string? LicenseFilePath { get; set; }
        
        [NotMapped]
        [Display(Name = "شهادة التخرج")]
        public IFormFile? GraduationCertificateFile { get; set; }

        [NotMapped]
        [Display(Name = "شهادة التخصص")]
        public IFormFile? CertificateFile { get; set; }

        [NotMapped]
        [Display(Name = "رخصة المزاولة")]
        public IFormFile? LicenseFile { get; set; }
        
        public ICollection<DoctorSpecialty>? DoctorSpecialties { get; set; }
        
        [Required(ErrorMessage = "يجب الموافقة على الشروط والأحكام")]
        [Display(Name = "أوافق على الشروط والأحكام")]
        public bool IsTermsAccepted { get; set; }
        
        [Required]
        [Display(Name = "حالة الطلب")]
        public JoinRequestStatus Status { get; set; } = JoinRequestStatus.Pending;
        
        [Display(Name = "تاريخ التقديم")]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        
        [Display(Name = "تاريخ الانضمام")]
        public DateTime? JoinDate { get; set; }
        
        [Display(Name = "ملاحظات الإدارة")]
        [StringLength(500)]
        public string? AdminNotes { get; set; }
    }
    
    public enum JoinRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}


