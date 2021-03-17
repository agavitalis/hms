using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    public class HMOSubUserGroupPatientRepository : IHMOSubUserGroupPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HMOSubUserGroupPatientRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateHMOSubGroupPatient(HMOSubUserGroupPatient hMOSubUserGroupPatient)
        {
            try
            {
                if (hMOSubUserGroupPatient == null)
                {
                    return false;
                }

                _applicationDbContext.HMOSubUserGroupPatients.Add(hMOSubUserGroupPatient);
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
