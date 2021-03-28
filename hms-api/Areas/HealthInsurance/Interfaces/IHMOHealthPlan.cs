﻿using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOHealthPlan
    {
        Task<int> GetHealthPlanCount(string HMOId);
        Task<HMOHealthPlan> GetHMOHealthPlan(string HMOHealthPlanId);
        PagedList<HMOHealthPlanDtoForView> GetHMOHealthPlans(PaginationParameter paginationParameter, string HMOId);
        Task<bool> CreateHMOHealthPlan(HMOHealthPlan HMOHealthPlan);
        Task<bool> UpdateHMOHealthPlan(HMOHealthPlan HMOHealthPlan);
    }
}
