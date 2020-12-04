using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Interfaces
{
    public interface IDrugPrice
    {
        Task<IEnumerable<DrugPrice>> GetDrugPrices();
        Task<IEnumerable<DrugPrice>> GetDrugPricesByDrug(string DrugId);
        Task<IEnumerable<DrugPrice>> GetDrugPricesByHealthPlan(string HealthPlanId);
        Task<Drug> GetDrugPrice(string DrugPriceId);
        Task<bool> CreateDrugPrice(DrugPrice DrugPrice);
        Task<bool> UpdateDrugPrice(DrugPrice DrugPrice);
        Task<bool> DeleteDrugPrice(DrugPrice DrugPrice);
    }
}
