using AutoMapper;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Patient.Dtos;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class NHISHealthPlanPatientRepository : INHISHealthPlanPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public NHISHealthPlanPatientRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public async Task<bool> DeleteHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient)
        {
            try
            {
                if (NHISHealthPlanPatient == null)
                {
                    return false;
                }

                _applicationDbContext.NHISHealthPlanPatients.Remove(NHISHealthPlanPatient);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NHISHealthPlanPatient> GetNHISHealthPlanPatient(string NHISHealthPlanId) => await _applicationDbContext.NHISHealthPlanPatients.Include(h => h.Patient).Where(h => h.Id == NHISHealthPlanId).FirstOrDefaultAsync();

        public PagedList<PatientDtoForView> GetNHISHealthPlanPatients(string NHISHealthPlanId, PaginationParameter paginationParameter)
        {
            var patients = _applicationDbContext.NHISHealthPlanPatients.Include(h => h.Patient).Where(h => h.NHISHealthPlanId == NHISHealthPlanId).ToList();
            var patientsToReturn = _mapper.Map<IEnumerable<PatientDtoForView>>(patients);
            return PagedList<PatientDtoForView>.ToPagedList(patientsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
