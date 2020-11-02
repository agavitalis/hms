using HMS.Areas.Admin.Models;
using HMS.Areas.Patient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class RegistrationInvoice
    {

        public RegistrationInvoice()
        {
            Id = Guid.NewGuid().ToString();
            DateGenerated = DateTime.Now;
        }
        public string Id { get; set; }
        public string Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }
        public string ModeOfPayment { get; set; }
        public string RefrenceNumber { get; set; }

        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }

        public string HealthPlanId { get; set; }
        public virtual HealthPlan HealthPlan { get; set; }

        public string PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

    }
}
