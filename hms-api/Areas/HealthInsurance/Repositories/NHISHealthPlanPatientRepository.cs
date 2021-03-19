using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class NHISHealthPlanPatientRepository : INHISHealthPlanPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public NHISHealthPlanPatientRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateNHISHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient)
        {
            try
            {
                if (NHISHealthPlanPatient == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanPatients.Add(NHISHealthPlanPatient);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> UpdateNHISHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient)
        {
            try
            {
                if (NHISHealthPlanPatient == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanPatients.Update(NHISHealthPlanPatient);
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
