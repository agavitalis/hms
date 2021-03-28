using System;

namespace HMS.Models
{
    public class HMOSubUserGroup
    {
        public HMOSubUserGroup()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOUserGroupId { get; set; }
        public HMOUserGroup HMOUserGroup { get; set; }
        public string HMOHealthPlanId { get; set; }
        public HMOHealthPlan HMOHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
