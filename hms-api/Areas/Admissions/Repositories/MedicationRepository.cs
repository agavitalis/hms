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
    public class MedicationRepository : IMedication
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public MedicationRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public MedicationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CreateDrugMedication(AdmissionDrugMedication admissionMedication)
        {
            try
            {
                if (admissionMedication == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionDrugMedications.Add(admissionMedication);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<DrugMedicationDtoForView> GetDrugMedications(string AdmissionId, PaginationParameter paginationParameter)
        {
            var medications = _applicationDbContext.AdmissionDrugMedications.Where(a => a.AdmissionId == AdmissionId).Include(a => a.Admission).Include(a => a.Drug).Include(a => a.Initiator).OrderBy(a => a.StartDate).ToList();
            var medicationsToReturn = _mapper.Map<IEnumerable<DrugMedicationDtoForView>>(medications);
            return PagedList<DrugMedicationDtoForView>.ToPagedList(medicationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> AdministerDrugMedication(AdmissionDrugDispensing admissionMedication)
        {
            try
            {
                if (admissionMedication == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionDrugDispensings.Add(admissionMedication);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AdmissionDrugMedication> GetDrugMedication(string AdmissionMedicationId) => await _applicationDbContext.AdmissionDrugMedications.Where(a => a.Id == AdmissionMedicationId).Include(a => a.Admission).Include(a => a.Drug).Include(a => a.Initiator).FirstOrDefaultAsync();

        public async Task<bool> UpdateDrugMedication(AdmissionDrugMedication admission)
        {
            try
            {
                if (admission == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionDrugMedications.Update(admission);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateServiceMedication(AdmissionServiceMedication AdmissionServiceMedication)
        {
            try
            {
                if (AdmissionServiceMedication == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionServiceMedications.Add(AdmissionServiceMedication);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public PagedList<ServiceMedicationDtoForView> GetServiceMedications(string AdmissionId, PaginationParameter paginationParameter)
        {
            var medications = _applicationDbContext.AdmissionServiceMedications.Where(a => a.AdmissionId == AdmissionId).Include(a => a.Admission).Include(a => a.Service).Include(a => a.Initiator).OrderBy(a => a.StartDate).ToList();
            var medicationsToReturn = _mapper.Map<IEnumerable<ServiceMedicationDtoForView>>(medications);
            return PagedList<ServiceMedicationDtoForView>.ToPagedList(medicationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<AdmissionServiceMedication> GetServiceMedication(string AdmissionMedicationId) => await _applicationDbContext.AdmissionServiceMedications.Where(a => a.Id == AdmissionMedicationId).Include(a => a.Admission).Include(a => a.Service).Include(a => a.Initiator).FirstOrDefaultAsync();
        

        public async Task<bool> UpdateServiceMedication(AdmissionServiceMedication admission)
        {
            try
            {
                if (admission == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionServiceMedications.Update(admission);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AdministerServiceMedication(AdmissionServiceRequest admissionMedication)
        {
            try
            {
                if (admissionMedication == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionServiceRequests.Add(admissionMedication);
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
