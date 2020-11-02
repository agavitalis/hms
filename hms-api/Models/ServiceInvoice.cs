using HMS.Areas.Admin.Models;
using HMS.Areas.Patient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class ServiceInvoice
    {
        public ServiceInvoice()
        {
            Id = Guid.NewGuid().ToString();
            DateGenerated = DateTime.Now;
        }
        public string Id { get; set; }
        public string AmountTotal { get; set; }
       
        public string PaymentStatus { get; set; }

        public string Description { get; set; }
        public string GeneratedBy { get; set; }

        public string ModeOfPayment { get; set; }

        public string RefrenceNumber { get; set; }

        public DateTime DatePaid { get; set; }
        public DateTime DateGenerated { get; set; }

        public string PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }


    }
}
