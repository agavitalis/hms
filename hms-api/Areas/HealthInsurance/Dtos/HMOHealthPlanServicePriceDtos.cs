using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

    public class HMOHealthPlanServicePriceDtoForView
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public string ServiceCategoryId { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public string HMOHealthPlanId { get; set; }
        public virtual HMOHealthPlan HMOHealthPlan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HMOHealthPlanServicePriceDtoForDelete
    {
        public string Id { get; set; }
    }
}
