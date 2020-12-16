using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorClerking
    {
        Task<IEnumerable<DoctorClerking>> GetClerkings();
        Task<DoctorClerking> GetClerking(string ClerkingId);
        Task<IEnumerable<DoctorClerking>> GetDoctorClerkingByPatient(string PatientId);
        Task<DoctorClerking> GetDoctorClerkingByAppointment(string AppointmentId);
        Task<DoctorClerking> GetDoctorClerkingByConsultation(string ConsultationId);
        Task<DoctorClerking> GetDoctorClerkingByAppointmentOrConsultation(string Id);
        Task<bool> CreateDoctorClerking(DoctorClerking clerking);
        Task<DoctorClerking> CreateDoctorClerking(string Id, string IdType, string UserId);
        Task<bool> UpdateDoctorClerking(string UserId,DoctorClerking doctorClerking, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking);
    }
}
