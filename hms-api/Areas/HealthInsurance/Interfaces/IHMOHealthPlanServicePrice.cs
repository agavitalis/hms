using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOHealthPlanServicePrice
    {
        Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePrices();
        Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePricesByService(string ServiceId);
        PagedList<HMOHealthPlanServicePriceDtoForView> GetServicePricesByHealthPlan(string HealthPlanId, PaginationParameter paginationParameter);
        Task<HMOHealthPlanServicePrice> GetServicePrice(string ServicePriceId);
        Task<bool> CreateServicePrice(HMOHealthPlanServicePrice ServicePrice);
        Task<bool> UpdateServicePrice(HMOHealthPlanServicePrice ServicePrice);
        Task<bool> DeleteServicePrice(HMOHealthPlanServicePrice ServicePrice);
    }
}
