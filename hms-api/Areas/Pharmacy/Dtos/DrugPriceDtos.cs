using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Dtos
{
    public class DrugPriceDtoForCreate
    {
        public string DrugId { get; set; }
        public string HealthPlanId { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PricePerContainer { get; set; }
        public decimal PricePerCarton { get; set; }
    }

    public class DefaultDrugPriceDtoForUpdate
    {
        public string DrugId { get; set; }
        
        public decimal DefaultPricePerUnit { get; set; }
       
        public decimal DefaultPricePerContainer { get; set; }
       
        public decimal DefaultPricePerCarton { get; set; }
    }

    

    public class DrugPriceDtoForUpdate
    {
        public string Id { get; set; }
        public string DrugId { get; set; }
        public string HealthPlanId { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PricePerContainer { get; set; }
        public decimal PricePerCarton { get; set; }
    }

    public class DrugPriceDtoForDelete
    {
        public string Id { get; set; }
    }
}
