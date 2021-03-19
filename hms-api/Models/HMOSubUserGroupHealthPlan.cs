using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class HMOSubUserGroupHealthPlan
    {
        public HMOSubUserGroupHealthPlan()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string Id { get; set; }
        public string HMOSubUserGroupId { get; set; }
        public HMOSubUserGroup HMOSubUserGroup { get; set; }
        public string HMOHealthPlanId { get; set; }
        public HMOHealthPlan HMOHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
