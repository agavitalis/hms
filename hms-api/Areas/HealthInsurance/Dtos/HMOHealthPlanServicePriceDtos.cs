using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOHealthPlanServicePriceDtoForCreate
    {
        public string ServiceId { get; set; }
        public string HMOHealthPlanId { get; set; }
        public decimal Price { get; set; }
    }

    public class HMOHealthPlanServicePriceDtoForUpdate
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string HMOHealthPlanId { get; set; }
        public decimal Price { get; set; }
    }

    public class HMOHealthPlanServicePriceDtoForDelete
    {
        public string Id { get; set; }
    }
}
