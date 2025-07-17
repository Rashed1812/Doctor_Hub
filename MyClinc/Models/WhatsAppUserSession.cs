using System.ComponentModel.DataAnnotations;

namespace MyClinc.Models
{
    public class WhatsAppUserSession
    {
        [Key]
        public string PhoneNumber { get; set; }
        
        public string CurrentFormType { get; set; } // "consultation", "doctor", "partnership"
        
        public int CurrentStep { get; set; } = 0;
        
        public DateTime LastActivity { get; set; } = DateTime.Now;
        
        public bool IsActive { get; set; } = true;
        
        // Form data storage
        public string? FormData { get; set; } // JSON string to store form data
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    
    public enum FormType
    {
        Consultation,
        DoctorJoin,
        Partnership
    }
} 