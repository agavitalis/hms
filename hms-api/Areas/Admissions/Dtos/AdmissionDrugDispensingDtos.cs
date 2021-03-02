using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionDrugDispensingDtoForCreate
    {
        public string AdmissionId { get; set; }
        public List<Drugs> Drugs { get; set; }
        public string GeneratedBy { get; set; }
    }

    public class DrugDispensingPaymentDto
    {
        public string AdmissionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
        public string InitiatorId { get; set; }
    }

    public class AdmissionDrugDispensingDtoForView
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public int NumberOfCartons { get; set; }
        public int NumberOfContainers { get; set; }
        public int NumberOfUnits { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCartonPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalContainerPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalUnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DrugPriceTotal { get; set; }

        public string DrugPriceCalculationFormular { get; set; }
        public string PaymentStatus { get; set; }
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }
    }
}
