using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class WardRepository : IWard
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public WardRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public PagedList<WardDtoForView> GetWardsPagnation(PaginationParameter paginationParameter)
        {
            var wards =  _applicationDbContext.Wards.ToList();
            var wardsToReturn = _mapper.Map<IEnumerable<WardDtoForView>>(wards);
            return PagedList<WardDtoForView>.ToPagedList(wardsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<BedDtoForView> GetBedsInWardPagnation( PaginationParameter paginationParameter, string WardId)
        {
            var beds = _applicationDbContext.Beds.Where(b => b.WardId == WardId).ToList();
            var bedsToReturn = _mapper.Map<IEnumerable<BedDtoForView>>(beds);
            return PagedList<BedDtoForView>.ToPagedList(bedsToReturn.AsQueryable(), paginationParameter.PageSize, paginationParameter.PageNumber);
        }
    }
}
