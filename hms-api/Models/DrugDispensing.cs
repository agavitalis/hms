using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DrugDispensing
    {
        public DrugDispensing()
        {
            Id = Guid.NewGuid().ToString();
            PaymentStatus = "Not Paid";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public string PriceCalculationFormular { get; set; }
        public string PaymentStatus { get; set; }
   

        [ForeignKey("DrugDispensingInvoice")]
        public string DrugDispensingInvoiceId { get; set; }
        public virtual DrugDispensingInvoice DrugPrescriptionInvoice { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public string ConsultationId { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
