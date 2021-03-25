using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class AdmissionServiceRequest
    {
        public AdmissionServiceRequest()
        {
            Id = Guid.NewGuid().ToString();
            Status = "UNDONE";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }

        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        
    }
}
