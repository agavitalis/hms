using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class GovernmentHealthPlanServicePrice
    {
        public GovernmentHealthPlanServicePrice()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }

        public string GovernmentHealthPlanId { get; set; }
        public virtual GovernmentHealthPlan GovernmentHealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
