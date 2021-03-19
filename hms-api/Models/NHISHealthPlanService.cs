using System;


namespace HMS.Models
{
    public class NHISHealthPlanService
    {
        public NHISHealthPlanService()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public string NHISHealthPlanId { get; set; }
        public virtual NHISHealthPlan NHISHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
