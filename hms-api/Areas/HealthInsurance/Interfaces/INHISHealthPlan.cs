using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlan
    {
        Task<int> GetNHISHealthPlanCount();
        Task<NHISHealthPlan> GetNHISHealthPlan(string HealthPlanId);
        PagedList<NHISHealthPlanDtoForView> GetNHISHealthPlans(PaginationParameter paginationParameter);
        Task<bool> CreateNHISHealthPlan(NHISHealthPlan HealthPlan);
        Task<bool> UpdateNHISHealthPlan(NHISHealthPlan HealthPlan);
    }
}
