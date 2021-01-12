using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class ServiceRequest
    {
        public ServiceRequest()
        {
            Id = Guid.NewGuid().ToString();
            PaymentStatus = "NOT PAID";
            Status = "UNDONE";

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

        public string AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public string ConsultationId { get; set; }
        public virtual Consultation Consultation { get; set; }

    }
}
