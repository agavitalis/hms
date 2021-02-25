using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{

    public class BedRepository : IBed
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BedRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        
        }

        public async Task<bool> CreateBed(Bed bed)
        {
            try
            {
                if (bed == null)
                {
                    return false;
                }

                _applicationDbContext.Beds.Add(bed);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Bed> GetBed(string BedId) => await _applicationDbContext.Beds.FindAsync(BedId);

        public async Task<bool> UpdateBed(Bed bed)
        {
            try
            {
                if (bed == null)
                {
                    return false;
                }

                _applicationDbContext.Beds.Update(bed);
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
