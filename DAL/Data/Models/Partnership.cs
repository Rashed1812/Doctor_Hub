using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{
    public class Partnership
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ContactPersonName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Position { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف كاملاً مع كود الدولة")]
        [Display(Name = "رقم الهاتف الدولي")]
        public string FullPhoneNumber { get; set; }
        
        [StringLength(50)]
        public string? WhatsAppNumber { get; set; }
        
        [Required]
        [StringLength(200)]
        public string CompanyAddress { get; set; }
        
        [StringLength(100)]
        public string? Website { get; set; }
        
        [Required]
        public string PartnershipType { get; set; } // نوع الشراكة
        
        [Required]
        [StringLength(1000)]
        public string PartnershipDetails { get; set; } // تفاصيل الشراكة المطلوبة
        
        [StringLength(500)]
        public string? CompanyDescription { get; set; } // وصف الشركة
        
        [StringLength(200)]
        public string? ServicesOffered { get; set; } // الخدمات المقدمة
        
        public DateTime SubmissionDate { get; set; }

        public PartnershipStatus Status { get; set; } = PartnershipStatus.Pending; // قيد المراجعة، مقبول، مرفوض
        
        [StringLength(500)]
        public string? RejectionReason { get; set; } // سبب الرفض
        
        [StringLength(500)]
        public string? AdminNotes { get; set; } // ملاحظات الإدارة
        
        public DateTime? ResponseDate { get; set; }
        
        // معلومات إضافية للشراكة
        public decimal? ProposedBudget { get; set; } // الميزانية المقترحة
        
        [StringLength(100)]
        public string? PreferredContactMethod { get; set; } // طريقة التواصل المفضلة
        
        public bool IsUrgent { get; set; } = false; // هل الطلب عاجل
        
        [StringLength(200)]
        public string? ReferenceSource { get; set; } // مصدر التعرف على الخدمة
    }
    public enum PartnershipStatus
    {
        [Display(Name = "قيد المراجعة")]
        Pending,

        [Display(Name = "مقبول")]
        Approved,

        [Display(Name = "مرفوض")]
        Rejected
    }
}

