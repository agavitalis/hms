using System;

namespace HMS.Models
{
    public class GovernmentHealthPlan
    {
        public GovernmentHealthPlan()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public string HealthplanId { get; set; }
        public HealthPlan Healthplan { get; set; }
        public bool RequireAuthorizationCode { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
