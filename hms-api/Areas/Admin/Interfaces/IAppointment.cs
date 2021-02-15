using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAppointment
    {
        Task<Appointment> GetAppointment(string AppointmentId);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<dynamic> GetDoctorsAppointment();
        PagedList<AppointmentDtoForView> GetAppointmentsPagination(PaginationParameter paginationParameter);
        PagedList<AppointmentDtoForView> GetAppointmentsAccepted(PaginationParameter paginationParameter);
        PagedList<AppointmentDtoForView> GetAppointmentsPending(PaginationParameter paginationParameter);
        PagedList<AppointmentDtoForView> GetAppointmentsCompleted(PaginationParameter paginationParameter);
        Task<bool> BookAppointment(Appointment appointment);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
        Task<bool> DeleteAppointment(Appointment appointment);
        Task<MyPatient> CheckDoctorInMyPatients(string DoctorId, string PatientId);
        Task<int> GetDoctorsPendingAppointmentsCount();
        Task<int> GetDoctorsPendingAppointmentsCount(string doctorId);
        Task<int> GetDoctorsCompletedAppointmentsCount(string doctorId);
        Task<int> GetPatientPendingAppointmentsCount(string patientId);
        Task<int> GetPatientCompletedAppointmentsCount(string patientId);
    }
}

