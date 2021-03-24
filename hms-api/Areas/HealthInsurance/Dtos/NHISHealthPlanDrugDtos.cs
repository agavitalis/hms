using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Dtos
{
    public class NHISHealthPlanDrugDtoForCreate
    {
        public string DrugId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanDrugDtoForUpdate
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanDrugDtoForView
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public string NHISHealthPlanId { get; set; }
        public virtual NHISHealthPlan NHISHealthPlan { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class NHISHealthPlanDrugDtoForDelete
    {
        public string Id { get; set; }
    }
}
