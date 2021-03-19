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

    public class NHISHealthPlanDrugDtoForDelete
    {
        public string Id { get; set; }
    }
}
