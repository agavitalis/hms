using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionServiceRequest
    {
        public AdmissionServiceRequest()
        {
            Id = Guid.NewGuid().ToString();
            PaymentStatus = "NOT PAID";
            Status = "UNDONE";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ServiceAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }

        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        
    }
}
