using HMS.Areas.Doctor.Interfaces;
using HMS.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Repositories
{
    public class MyPatientRepository: IMyPatient
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MyPatientRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> GetMyPatientCountAsync(string doctorId)
        {
            var myPatientCount = await _applicationDbContext.MyPatients.Where(p => p.DoctorId == doctorId).CountAsync();
            return myPatientCount;
        }

        public async Task<int> GetMyDoctorCountAsync(string patientId)
        {
            var myPatientCount = await _applicationDbContext.MyPatients.Where(p => p.PatientId == patientId).CountAsync();
            return myPatientCount;
        }


    }
}
