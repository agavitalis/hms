using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface ISurgery
    {
        Task<Surgery> GetSurgeryByAppointmentOrConsultation(string Id);
        Task<Surgery> CreateSurgery(string Id, string IdType, string DoctorId, string PatientId);
        Task<bool> UpdateSurgery(Surgery surgery);
    }
}
