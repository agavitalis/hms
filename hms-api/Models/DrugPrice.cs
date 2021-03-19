using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class DrugPrice
    {
        public DrugPrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual  Drug Drug { get; set; }

        public string HealthPlanId { get; set; }
        public virtual HealthPlan HealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerContainer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerCarton { get; set; }
    }
}
