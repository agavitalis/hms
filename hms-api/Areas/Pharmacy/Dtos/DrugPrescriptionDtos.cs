using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Dtos
{
    public class DrugPrescriptionInvoiceDtoForCreate
    {
        public string PatientId { get; set; }
        public List<string> DrugId { get; set; }
        public string Description { get; set; }
        public string GeneratedBy { get; set; }

        //this Id can either be Appointment or Consultation Id
        public string Id { get; set; }
        public string IdType { get; set; }
    }

    public class DrugPrescriptionInvoiceDtoForView
    {
        public string Id { get; set; }
        public string Fullname { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        public int NoofPrescriptions { get; set; }
        public string PaymentStatus { get; set; }
        public string ClerkingId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime DateGenerated { get; set; }

    }

    public class DrugPrescriptionPaymentDto
    {
        public string PatientId { get; set; }
        public List<string> DrugPrescriptionId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public string ModeOfPayment { get; set; }
        public string PaymentReference { get; set; }

    }
}

