using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientAppointment
    {
        Task<int> GetPendingAppointmentsCount(string patientId);
        Task<int> GetCompletedAppointmentsCount(string patientId);
        Task<int> GetCanceledAppointmentsCount(string patientId);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
        Task<bool> BookAppointment(BookAppointmentViewModel appointment);
        PagedList<AppointmentDtoForView> GetCompletedAppointments(PaginationParameter paginationParameter, string PatientId);
        PagedList<AppointmentDtoForView> GetPendingAppointments(PaginationParameter paginationParameter, string PatientId);
        PagedList<AppointmentDtoForView> GetCanceledAppointments(PaginationParameter paginationParameter, string PatientId);
        Task<Appointment> GetPatientAppointment(string appointmentId);
        Task<int> CancelAppointment(string appointmentId);
    }
}
