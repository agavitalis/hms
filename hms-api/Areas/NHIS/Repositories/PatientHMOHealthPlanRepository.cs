using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    
   
    public class PatientHMOHealthPlanRepository : IPatientHMOHealthPlan
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PatientHMOHealthPlanRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreatePatientHMOHealthPlan(PatientHMOHealthPlan PatientHMOHealthPlan)
        {
            try
            {
                if (PatientHMOHealthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.PatientHMOHealthPlans.Add(PatientHMOHealthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
