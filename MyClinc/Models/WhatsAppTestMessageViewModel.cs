using System.ComponentModel.DataAnnotations;

namespace MyClinc.Models
{
    public class WhatsAppTestMessageViewModel
    {
        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الرسالة")]
        [Display(Name = "الرسالة")]
        public string Message { get; set; }
    }
} 