using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class DtoForPatientRegistrationInvoice
    {
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime DateGenerated { get; set; }
        public string PatientId { get; set; }
        public  HealthPlan HealthPlan { get; set; }      
    }

    public class DtoForPatientRegistrationPayment
    {
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string ModeOfPayment { get; set; }
        public string transactionReference { get; set; }
        public string Id { get; set; }
    }

}
