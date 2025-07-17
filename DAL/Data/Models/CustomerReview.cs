using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Models
{
    public class CustomerReview
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; }
        
        public DateTime ReviewDate { get; set; }
        
        public bool IsApproved { get; set; }
        
        // Foreign key to Consultation
        public int? ConsultationId { get; set; }
        public virtual Consultation? Consultation { get; set; }
        
        // Additional fields
        [StringLength(50)]
        public string? CustomerTitle { get; set; } // مثل: مريض، طبيب، إلخ
        
        [StringLength(200)]
        public string? CustomerImage { get; set; } // رابط صورة العميل (اختياري)
        
        public bool IsVisible { get; set; } = true;
    }
}

