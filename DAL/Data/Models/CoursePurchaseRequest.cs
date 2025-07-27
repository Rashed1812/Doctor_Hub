using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class CoursePurchaseRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public Course Course { get; set; }
        public PaymentStatuse paymentStatuse { get; set; }
    }

    public enum PaymentStatuse
    {
        Pending,
        Approved,
        Rejected
    }
}
