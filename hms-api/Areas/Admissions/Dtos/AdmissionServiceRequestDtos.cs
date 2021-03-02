using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionServiceRequestDtoForCreate
    {
        public string AdmissionId { get; set; }
        public List<string> ServiceId { get; set; }
        public string GeneratedBy { get; set; }
    }

    public class AdmissionServiceRequestDtoForView
    {
        public string Id { get; set; }
        public decimal ServiceAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }

    public class AdmissionServiceRequestPaymentDto
    {
        public string AdmissionId { get; set; }
        public List<string> ServiceRequestId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string InitiatorId { get; set; }

    }
}
