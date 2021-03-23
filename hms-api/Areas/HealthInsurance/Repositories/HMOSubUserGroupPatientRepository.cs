using AutoMapper;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Patient.Dtos;
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
    public class HMOSubUserGroupPatientRepository : IHMOSubUserGroupPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public HMOSubUserGroupPatientRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public async Task<bool> DeleteHMOSubGroupPatient(HMOSubUserGroupPatient HMOSubUserGroupPatient)
        {
            try
            {
                if (HMOSubUserGroupPatient == null)
                {
                    return false;
                }

                _applicationDbContext.HMOSubUserGroupPatients.Remove(HMOSubUserGroupPatient);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public async Task<HMOSubUserGroupPatient> GetHMOSubUserGroupPatient(string HMOSubGroupPatientId) => await _applicationDbContext.HMOSubUserGroupPatients.Include(h => h.Patient).Where(h => h.Id == HMOSubGroupPatientId).FirstOrDefaultAsync();
       

        public PagedList<PatientDtoForView> GetHMOSubUserGroupPatients(string HMOSubGroupId, PaginationParameter paginationParameter)
        {
            var patients = _applicationDbContext.HMOSubUserGroupPatients.Include(h => h.Patient).Where(h => h.HMOSubUserGroupId == HMOSubGroupId).ToList();
            var patientsToReturn = _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return PagedList<PatientDtoForView>.ToPagedList(patientsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
