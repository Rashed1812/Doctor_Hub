using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class UpdatePaymentStatusRequest
    {
        public int ConsultationId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
