using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HMOHealthPlanDrugPrice
    {
        public HMOHealthPlanDrugPrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }

        public string HMOHealthPlanId { get; set; }
        public virtual HMOHealthPlan HMOHealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerContainer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerCarton { get; set; }
    }
}
