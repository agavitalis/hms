using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class WardRepository : IWard
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public WardRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> CreateWard(Ward ward)
        {
            try
            {
                if (ward == null)
                {
                    return false;
                }

                _applicationDbContext.Wards.Add(ward);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Ward>> GetAllWards() => await _applicationDbContext.Wards.ToListAsync();
        
        public async Task<Ward> GetWardByIdAsync(string id)
        {
            try
            {
                var ward = await _applicationDbContext.Wards.FindAsync(id);

                return ward;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateWard(Ward ward)
        {
            try
            {
                if (ward == null)
                {
                    return false;
                }

                _applicationDbContext.Wards.Update(ward);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteWard(Ward ward)
        {
            try
            {
                if (ward == null)
                {
                    return false;
                }

                _applicationDbContext.Wards.Remove(ward);
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
