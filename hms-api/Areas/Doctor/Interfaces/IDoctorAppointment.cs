using HMS.Areas.Admin.Dtos;
using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorAppointment
    {
        Task<Appointment> GetAppointmentById(string Id);
        Task<IEnumerable<Appointment>> GetDoctorAppointments(string DoctorId);
        Task<Appointment> GetDoctorAppointment(string appointmentId);
        PagedList<AppointmentDtoForView> GetAppointmentsPending(string DoctorId, PaginationParameter paginationParameter);
        PagedList<AppointmentDtoForView> GetAppointmentsCompleted(string DoctorId, PaginationParameter paginationParameter);
        PagedList<AppointmentDtoForView> GetAppointmentsAccepted(string DoctorId, PaginationParameter paginationParameter);
        Task<int> AcceptAppointment(Appointment appointment);
        Task<int> RejectAppointment(Appointment appointment);
        Task<int> CancelAppointment(Appointment appointment);
        Task<int> AdmitPatientOrSendPatientHome(CompletDoctorClerkingDto clerking);
        Task<bool> UpdateAppointment(Appointment appointment);
    }
}
