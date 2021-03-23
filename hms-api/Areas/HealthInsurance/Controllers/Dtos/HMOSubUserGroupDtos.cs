using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOSubUserGroupDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HMOUserGroupId { get; set; }
        public HMOUserGroup HMOUserGroup { get; set; }
        public HMO HMO { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HMOSubUserGroupDtoForCreate
    {
        public string Name { get; set; }
        public string HMOUserGroupId { get; set; }
    }
}
