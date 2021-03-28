using System;

namespace HMS.Models
{
    public class HMO
    {
        public HMO()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public DateTime DateCreated { get; set; }        
    }
}
