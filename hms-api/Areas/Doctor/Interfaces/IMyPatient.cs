using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IMyPatient
    {
        Task<int> GetMyPatientCountAsync(string doctorId);
        Task<int> GetMyDoctorCountAsync(string patientId);
    }
}
