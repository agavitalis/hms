using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    
   
    public class HMOHealthPlanPatientRepository : IHMOHealthPlanPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HMOHealthPlanPatientRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateHMOHealthPlanPatient(HMOHealthPlanPatient HMOHealthPlanPatient)
        {
            try
            {
                if (HMOHealthPlanPatient == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanPatients.Add(HMOHealthPlanPatient);
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
