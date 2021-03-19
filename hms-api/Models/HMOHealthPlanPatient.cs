using System;

namespace HMS.Models
{
    public class HMOHealthPlanPatient
    {
        public HMOHealthPlanPatient()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public string HMOHealthPlanId { get; set; }
        public HMOHealthPlan HMOHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
