using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOHealthPlanDrugPriceDtoForCreate
    {
        public string DrugId { get; set; }
        public string HMOHealthPlanId { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PricePerContainer { get; set; }
        public decimal PricePerCarton { get; set; }
    }

    public class HMOHealthPlanDrugPriceDtoForUpdate
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public string HMOHealthPlanId { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PricePerContainer { get; set; }
        public decimal PricePerCarton { get; set; }
    }

    public class HMOHealthPlanDrugPriceDtoForDelete
    {
        public string Id { get; set; }
    }
}
