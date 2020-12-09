﻿using HMS.Models;
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
        Task<HealthPlan> GetHealthPlanByIdAsync(string id);
    }
}
