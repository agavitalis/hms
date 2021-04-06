using AutoMapper;

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
    public class AdmissionRepository : IAdmission
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public AdmissionRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateAdmission(Admission admission)
        {
            try
            {
                if (admission == null)
                {
                    return false;
                }

                _applicationDbContext.Admissions.Add(admission);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Admission> GetAdmission(string AdmissionId) => await _applicationDbContext.Admissions.Where(a => a.Id == AdmissionId).Include(a => a.Bed).ThenInclude(a => a.Ward).FirstOrDefaultAsync();

        public PagedList<AdmissionDtoForView> GetAdmissionsWithBed(PaginationParameter paginationParameter)
        {
            var admissions = _applicationDbContext.Admissions.Where(a => a.BedId != null).Include(a => a.Bed).ThenInclude(a => a.Ward).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfAdmission).ToList();
            var admissionsToReturn = _mapper.Map<IEnumerable<AdmissionDtoForView>>(admissions);
            return PagedList<AdmissionDtoForView>.ToPagedList(admissionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AdmissionDtoForView> GetAdmissionsWithBed(PaginationParameter paginationParameter, string WardId)
        {
            var admissions = _applicationDbContext.Admissions.Where(a => a.BedId != null && a.Bed.WardId == WardId).Include(a => a.Bed).ThenInclude(a => a.Ward).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfAdmission).ToList();
            var admissionsToReturn = _mapper.Map<IEnumerable<AdmissionDtoForView>>(admissions);
            return PagedList<AdmissionDtoForView>.ToPagedList(admissionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AdmissionDtoForView> GetAdmissionsWithoutBed(PaginationParameter paginationParameter)
        {
            var admissions = _applicationDbContext.Admissions.Where(a => a.BedId == null).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.DateOfReferral).ToList();
            var admissionsToReturn = _mapper.Map<IEnumerable<AdmissionDtoForView>>(admissions);
            return PagedList<AdmissionDtoForView>.ToPagedList(admissionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> UpdateAdmission(Admission admission)
        {
            try
            {
                if (admission == null)
                {
                    return false;
                }

                _applicationDbContext.Admissions.Update(admission);
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
