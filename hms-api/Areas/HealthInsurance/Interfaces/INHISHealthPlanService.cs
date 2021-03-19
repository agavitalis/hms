using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlanService
    {
        Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServices();
        Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServicesByService(string ServiceId);
        Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServicesByHealthPlan(string HealthPlanId);
        Task<NHISHealthPlanService> GetHealthPlanService(string HealthPlanServiceId);
        Task<bool> CreateHealthPlanService(NHISHealthPlanService HealthPlanService);
        Task<bool> UpdateHealthPlanService(NHISHealthPlanService HealthPlanService);
        Task<bool> DeleteHealthPlanService(NHISHealthPlanService HealthPlanService);
    }
}
