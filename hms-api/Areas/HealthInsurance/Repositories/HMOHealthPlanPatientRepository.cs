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
    
   
    public class HMOHealthPlanPatientRepository : IHMOHealthPlanPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public HMOHealthPlanPatientRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public async Task<bool> DeleteHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan)
        {
            try
            {
                if (PatientHMOHealthPlan == null)
                {
                    return false;
                }

                _applicationDbContext.HMOHealthPlanPatients.Remove(PatientHMOHealthPlan);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HMOHealthPlanPatient> GetHMOHealthPlanPatient(string HMOHealthPlanPatientId) => await _applicationDbContext.HMOHealthPlanPatients.Include(h => h.Patient).Where(h => h.Id == HMOHealthPlanPatientId).FirstOrDefaultAsync();
       

        public PagedList<PatientDtoForView> GetHMOHealthPlanPatients(string HMOHealthPlanId, PaginationParameter paginationParameter)
        {
            var patients = _applicationDbContext.HMOHealthPlanPatients.Include(h => h.Patient).Where(h => h.HMOHealthPlanId == HMOHealthPlanId).ToList();
            var patientsToReturn = _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return PagedList<PatientDtoForView>.ToPagedList(patientsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
