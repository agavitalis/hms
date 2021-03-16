using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOHealthPlanServicePrice
    {
        Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePrices();
        Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePricesByService(string ServiceId);
        Task<IEnumerable<HMOHealthPlanServicePrice>> GetServicePricesByHealthPlan(string HealthPlanId);
        Task<HMOHealthPlanServicePrice> GetServicePrice(string ServicePriceId);
        Task<bool> CreateServicePrice(HMOHealthPlanServicePrice ServicePrice);
        Task<bool> UpdateServicePrice(HMOHealthPlanServicePrice ServicePrice);
        Task<bool> DeleteServicePrice(HMOHealthPlanServicePrice ServicePrice);
    }
}
