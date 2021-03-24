using HMS.Areas.HealthInsurance.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlanDrug
    {
        Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugs();
        Task<IEnumerable<NHISHealthPlanDrug>> GetHealthPlanDrugsByDrug(string DrugId);
        PagedList<NHISHealthPlanDrugDtoForView> GetHealthPlanDrugsByHealthPlan(string HealthPlanId, PaginationParameter paginationParameter);
        Task<NHISHealthPlanDrug> GetHealthPlanDrug(string HealthPlanDrugId);
        Task<bool> CreateHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug);
        Task<bool> UpdateHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug);
        Task<bool> DeleteHealthPlanDrug(NHISHealthPlanDrug HealthPlanDrug);
    }
}
