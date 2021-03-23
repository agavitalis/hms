
using HMS.Models;
using System;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOHealthPlanDtoForCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOId { get; set; }
    }

    public class HMOHealthPlanDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class HMOHealthPlanDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HMOId { get; set; }
        public HMO HMO { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
