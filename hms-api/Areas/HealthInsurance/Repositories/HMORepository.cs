using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Repositories
{
    public class HMORepository : IHMO
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public HMORepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateHMO(HMO HMO)
        {
            try
            {
                if (HMO == null)
                {
                    return false;
                }

                _applicationDbContext.HMOs.Add(HMO);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMO> GetHMO(string HMOId) => await _applicationDbContext.HMOs.Where(h => h.Id == HMOId).Include(h => h.HealthPlan).FirstOrDefaultAsync();

        public PagedList<HMODtoForView> GetHMOs(PaginationParameter paginationParameter)
        {
            var HMOs = _applicationDbContext.HMOs.Include(h => h.HealthPlan).ToList();
            var HMOsToReturn = _mapper.Map<IEnumerable<HMODtoForView>>(HMOs);
            return PagedList<HMODtoForView>.ToPagedList(HMOsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
