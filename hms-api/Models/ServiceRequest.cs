using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class ServiceRequest
    {
        public ServiceRequest()
        {
            Id = Guid.NewGuid().ToString();
            Status = "Not Paid";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string ServiceInvoiceId { get; set; }
        public virtual ServiceInvoice ServiceInvoice {get; set;}     
      
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }

    }
}
