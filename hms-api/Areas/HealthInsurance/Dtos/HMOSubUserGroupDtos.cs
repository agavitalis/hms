using HMS.Models;
using System;


namespace HMS.Areas.NHIS.Dtos
{
    public class HMOSubUserGroupDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOUserGroupId { get; set; }
        public HMOUserGroup HMOUserGroup { get; set; }
        public HMO HMO { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HMOSubUserGroupDtoForCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOHealthPlanId { get; set; }
        public string HMOUserGroupId { get; set; }
    }

    public class HMOSubUserGroupDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOHealthPlanId { get; set; }
        public string HMOUserGroupId { get; set; }
    }
}
