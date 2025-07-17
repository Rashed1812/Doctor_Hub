using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class UpdateStatusRequest
    {
        public int ConsultationId { get; set; }
        public string Status { get; set; }
    }
}
