using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class GovernmentHealthPlanDrugPrice
    {
        public GovernmentHealthPlanDrugPrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string HMOHealthPlanId { get; set; }
        public virtual GovernmentHealthPlan GovernmentHealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerContainer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerCarton { get; set; }
    }
}
