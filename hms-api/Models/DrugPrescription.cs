using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DrugPrescription
    {
        public DrugPrescription()
        {
            Id = Guid.NewGuid().ToString();
            Status = "Not Paid";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }

        [ForeignKey("DrugPrescriptionInvoice")]
        public string DrugPrescriptionInvoiceId { get; set; }
        public virtual DrugPrescriptionInvoice DrugPrescriptionInvoice { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public string ConsultationId { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
