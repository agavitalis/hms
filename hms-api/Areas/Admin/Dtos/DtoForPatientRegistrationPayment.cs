using System;
using System.Collections.Generic;
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
        public  string HealthPlan { get; set; }
        public string PatientId { get; set; }
      
    }

    public class DtoForPatientRegistrationPayment
    {
        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public string ModeOfPayment { get; set; }
        public string ReferenceNumber { get; set; }
        public string PatientId { get; set; }

    }

}
