using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class NHISHealthPlan
    {
        public NHISHealthPlan()
        {
            Id = Guid.NewGuid().ToString();
            RequireAuthorizationCode = false;
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public bool RequireAuthorizationCode { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
