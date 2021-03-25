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
        public async Task<bool> CreateMedication(AdmissionMedication admissionMedication)
        {
            try
            {
                if (admissionMedication == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionMedications.Add(admissionMedication);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<MedicationDtoForView> GetMedications(string AdmissionId, PaginationParameter paginationParameter)
        {
            var medications = _applicationDbContext.AdmissionMedications.Where(a => a.AdmissionId == AdmissionId).Include(a => a.Admission).Include(a => a.Drug).Include(a => a.Initiator).ToList();
            var medicationsToReturn = _mapper.Map<IEnumerable<MedicationDtoForView>>(medications);
            return PagedList<MedicationDtoForView>.ToPagedList(medicationsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> AdministerMedication(AdmissionDrugDispensing admissionMedication)
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

        public async Task<AdmissionMedication> GetAdmissionMedication(string AdmissionMedicationId) => await _applicationDbContext.AdmissionMedications.Where(a => a.Id == AdmissionMedicationId).Include(a => a.Admission).Include(a => a.Drug).Include(a => a.Initiator).FirstOrDefaultAsync();

        public async Task<bool> UpdateMedication(AdmissionMedication admission)
        {
            try
            {
                if (admission == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionMedications.Update(admission);
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
