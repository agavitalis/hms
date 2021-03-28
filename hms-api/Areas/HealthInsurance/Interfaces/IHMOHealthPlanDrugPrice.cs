using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOHealthPlanDrugPrice
    {
        Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPrices();
        Task<IEnumerable<HMOHealthPlanDrugPrice>> GetDrugPricesByDrug(string DrugId);
        PagedList<HMOHealthPlanDrugPriceDtoForView> GetDrugPricesByHealthPlan(string HealthPlanId, PaginationParameter paginationParameter);
        Task<HMOHealthPlanDrugPrice> GetDrugPrice(string DrugPriceId);
        Task<bool> CreateDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
        Task<bool> UpdateDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
        Task<bool> DeleteDrugPrice(HMOHealthPlanDrugPrice DrugPrice);
    }
}
