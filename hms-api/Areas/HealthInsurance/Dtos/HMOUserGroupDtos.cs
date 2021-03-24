using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOUserGroupDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOId { get; set; }
        public HMO HMO { get; set; }
        public HealthPlan HealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HMOUserGroupDtoForCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOId { get; set; }
    }

    public class HMOUserGroupDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOId { get; set; }
    }
}
