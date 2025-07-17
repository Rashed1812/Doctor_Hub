using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Consultation
    {
        [Key]
        public int ConsultationId { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الاسم الكامل")]
        [Display(Name = "الاسم الكامل")]
        public string PatientName { get; set; }

        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف كاملاً مع كود الدولة")]
        [Display(Name = "رقم الهاتف الدولي")]
        public string FullPhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تاريخ الميلاد")]
        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "بلد الولادة")]
        public string BirthCountry { get; set; }

        [Required]
        [Display(Name = "بلد السكن الحالي")]
        public string CurrentCountry { get; set; }

        [Display(Name = "جهة العمل أو الدراسة")]
        public string? WorkOrStudyPlace { get; set; }

        [Display(Name = "الجنس")]
        public string? Gender { get; set; }

        [Display(Name = "الحالة الاجتماعية")]
        public string? MaritalStatus { get; set; }
        
        [Required(ErrorMessage = "يرجى اختيار نوع الاستشارة")]
        [Display(Name = "نوع الاستشارة")]
        public int MedicalSpecialtyId { get; set; }

        [ForeignKey("MedicalSpecialtyId")]
        public MedicalSpecialty? MedicalSpecialty { get; set; }

        [Display(Name = "رسوم الاستشارة")]
        public decimal ConsultationFee { get; set; }
        
        [Display(Name = "حالة الدفع")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Display(Name = "تم إرسال الوصل؟")]
        public bool ReceiptSent { get; set; } = false;
        
        public string? ReceiptToken { get; set; }
        
        [Display(Name = "تاريخ الحجز")]
        public DateTime BookingDate { get; set; } = DateTime.Now;
        
        [Display(Name = "حالة الاستشارة")]
        public string Status { get; set; } = "قيد المراجعة"; // قيد المراجعة، مؤكد، مكتمل، ملغي
        
        [Display(Name = "ملاحظات الإدارة")]
        [StringLength(500)]
        public string? AdminNotes { get; set; }

        [Display(Name = "هل تعاني من أمراض مزمنة؟")]
        public string? ChronicDiseases { get; set; }

        [Display(Name = "هل لديك حساسية تجاه أدوية معينة أو مواد غذائية؟")]
        public string? Allergies { get; set; }

        [Display(Name = "هل أجريت عمليات جراحية سابقة؟")]
        public string? PastSurgeries { get; set; }

        [Display(Name = "هل تتناول أدوية حالياً؟")]
        public string? CurrentMedications { get; set; }

        [Display(Name = "هل سبق أن أُصبت بأمراض معدية؟")]
        public string? InfectiousDiseases { get; set; }

        [Display(Name = "مشاكل في الجهاز التنفسي")]
        public bool? HasRespiratoryIssues { get; set; }

        [Display(Name = "مشاكل في الجهاز الهضمي")]
        public bool? HasDigestiveIssues { get; set; }

        [Display(Name = "مشاكل في الجهاز العصبي")]
        public bool? HasNeurologicalIssues { get; set; }

        [Display(Name = "مشاكل في الجهاز البولي أو التناسلي")]
        public bool? HasUrinaryOrReproductiveIssues { get; set; }
        // 🧬 التاريخ العائلي للأمراض الوراثية
        [Display(Name = "هل يوجد أمراض وراثية في العائلة؟")]
        public string? FamilyDiseases { get; set; } // مثال: القلب، السكري، لا يوجد

        // 🧠 الصحة النفسية
        [Display(Name = "هل تعاني أو عانيت سابقاً من مشاكل نفسية؟")]
        public string? MentalHealthIssues { get; set; } // مثال: اكتئاب، قلق، لا يوجد

        [Display(Name = "هل تأخذ أدوية نفسية؟")]
        public string? PsychiatricMedications { get; set; } // أسماء الأدوية والجرعات

        // 🧪 الفحوصات والتحاليل السابقة
        [Display(Name = "نتائج تحاليل أو أشعة أو فحوصات سابقة")]
        public string? PreviousTestsResults { get; set; }

        [Display(Name = "تاريخ آخر تطعيم")]
        [DataType(DataType.Date)]
        public DateTime? LastVaccinationDate { get; set; } // مثال: تاريخ تطعيم كورونا أو التهاب الكبد
        public string? Diagnosis { get; set; } // التشخيص الطبي إذا كان متاحًا
        public string? RecommendedTreatment { get; set; }

        // Navigation property for reviews
        public virtual ICollection<CustomerReview> Reviews { get; set; } = new List<CustomerReview>();
    }
    
    public enum PaymentStatus
    {
        Pending,
        Paid
    }
}
