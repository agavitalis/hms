using AutoMapper;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class MedicationDispensingRepository : IMedicationDispensing
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public MedicationDispensingRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateMedicationDispensing(AdmissionMedicationDispensing admissionMedicationDispensing)
        {
            try
            {
                if (admissionMedicationDispensing == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionMedicationDispensings.Add(admissionMedicationDispensing);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AdmissionMedicationDispensing>> GetAdmissionMedicationDispensing(string AdmissionId) => await _applicationDbContext.AdmissionMedicationDispensings.Where(a => a.AdmissionId == AdmissionId).Include(a => a.Admission).Include(a => a.Initiator).ToListAsync();
      
    }
}
