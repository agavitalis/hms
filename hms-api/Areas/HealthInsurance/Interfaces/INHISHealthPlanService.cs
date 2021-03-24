using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlanService
    {
        Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServices();
        Task<IEnumerable<NHISHealthPlanService>> GetHealthPlanServicesByService(string ServiceId);
        PagedList<NHISHealthPlanServiceDtoForView> GetHealthPlanServicesByHealthPlan(string HealthPlanId, PaginationParameter paginationParameter);
        Task<NHISHealthPlanService> GetHealthPlanService(string HealthPlanServiceId);
        Task<bool> CreateHealthPlanService(NHISHealthPlanService HealthPlanService);
        Task<bool> UpdateHealthPlanService(NHISHealthPlanService HealthPlanService);
        Task<bool> DeleteHealthPlanService(NHISHealthPlanService HealthPlanService);
    }
}
