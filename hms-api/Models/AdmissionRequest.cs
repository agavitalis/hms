using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionRequest
    {
        public AdmissionRequest()
        {
            Id = Guid.NewGuid().ToString();
            PaymentStatus = "NOT PAID";
            Status = "UNDONE";

        }
        public string Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ServiceAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }

        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public int NumberOfCartons { get; set; }
        public int NumberOfContainers { get; set; }
        public int NumberOfUnits { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalCartonPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalContainerPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalUnitPrice { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal DrugPriceTotal { get; set; }

        public string DrugPriceCalculationFormular { get; set; }
    }
}
