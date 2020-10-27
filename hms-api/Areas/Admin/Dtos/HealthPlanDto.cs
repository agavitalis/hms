using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class HealthPlanDtoForCreate
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Renewal { get; set; }
        public int AccountPerPlan { get; set; }
        public bool InstantBilling { get; set; }
    }

    public class HealthPlanDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}
