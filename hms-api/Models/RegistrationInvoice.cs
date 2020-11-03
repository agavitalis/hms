using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class RegistrationInvoice
    {

        public RegistrationInvoice()
        {
            Id = Guid.NewGuid().ToString();
            DateGenerated = DateTime.Now;
            PaymentStatus = false;
            Description = "Registration Invoice";
        }
        public string Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public bool PaymentStatus { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
        public string ModeOfPayment { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }

        public string HealthPlanId { get; set; }
        public virtual HealthPlan HealthPlan { get; set; }

        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
    }
}
