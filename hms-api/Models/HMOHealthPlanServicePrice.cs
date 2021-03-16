using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class HMOHealthPlanServicePrice
    {
        public HMOHealthPlanServicePrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }

        public string HMOHealthPlanId { get; set; }
        public virtual HMOHealthPlan HMOHealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
