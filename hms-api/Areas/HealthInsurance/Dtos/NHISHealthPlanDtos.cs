using HMS.Models;
using System;


namespace HMS.Areas.HealthInsurance.Dtos
{
    public class NHISHealthPlanDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public bool RequireAuthorizationCode { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class NHISHealthPlanDtoForCreate
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool RequireAuthorizationCode { get; set; }
        public string HealthPlanId { get; set; }
    }

    public class NHISHealthPlanDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public bool RequireAuthorizationCode { get; set; }
        public decimal Amount { get; set; }
        public string HealthPlanId { get; set; }
    }
}
