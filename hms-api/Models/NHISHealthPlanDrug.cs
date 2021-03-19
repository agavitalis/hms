using System;

namespace HMS.Models
{
    public class NHISHealthPlanDrug
    {
        public NHISHealthPlanDrug()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string NHISHealthPlanId { get; set; }
        public virtual NHISHealthPlan NHISHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
