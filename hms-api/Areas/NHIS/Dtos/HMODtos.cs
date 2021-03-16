

using HMS.Models;
using System;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMODtoForCreate
    {
        public string Name { get; set; }
        public string HealthPlanId { get; set; }
    }

    public class HMODtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
    
}
