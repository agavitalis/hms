using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOHealthPlanDrugPrice
    {
        Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPrices();
        Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPricesByDrug(string DrugId);
        Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPricesByHealthPlan(string HealthPlanId);
        Task<HMOHealthPlanDrugPrice> GetDrugPrice(string DrugPriceId);
        Task<bool> CreateDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
        Task<bool> UpdateDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
        Task<bool> DeleteDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
    }
}
