using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOHealthPlan
    {
        Task<HMOHealthPlan> GetHMOHealthPlan(string HMOHealthPlanId);
        PagedList<HMOHealthPlanDtoForView> GetHMOHealthPlans(PaginationParameter paginationParameter);
        Task<bool> CreateHMOHealthPlan(HMOHealthPlan HMOHealthPlan);
        Task<bool> UpdateHMOHealthPlan(HMOHealthPlan HMOHealthPlan);
    }
}
