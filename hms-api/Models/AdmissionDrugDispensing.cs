using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class AdmissionDrugDispensing
    {
        public AdmissionDrugDispensing()
        {
            Id = Guid.NewGuid().ToString();
            DateDispensed = DateTime.Now;
            TimeDispensed = DateTime.Now;
        }
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
        public string AdmissionInvoiceId { get; set; }
        public AdmissionInvoice AdmissionInvoice { get; set; }

        public DateTime DateDispensed { get; set; }
        public DateTime TimeDispensed { get; set; }
        public string InitiatorId { get; set; }
    }
}
