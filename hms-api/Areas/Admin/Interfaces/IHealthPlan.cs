using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IHealthPlan
    {
        Task<bool> InsertHealthPlan(HealthPlan plan);
        Task<bool> UpdateHealthPlan(HealthPlan plan);
        Task<bool> DeleteHealthPlan(HealthPlan plan);
        Task<IEnumerable<HealthPlan>> GetAllHealthPlan();
        PagedList<HealthPlanDtoForView> GetHealthPlansPagnation(PaginationParameter paginationParameter);
        Task<HealthPlan> GetHealthPlanByIdAsync(string id);
    }
}
