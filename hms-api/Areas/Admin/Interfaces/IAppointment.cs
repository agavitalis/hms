using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAppointment
    {
        Task<Appointment> GetAppointment(string AppointmentId);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<dynamic> GetDoctorsAppointment();
        Task<bool> BookAppointment(Appointment appointment);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
        Task<bool> DeleteAppointment(Appointment appointment);
        Task<int> GetDoctorsPendingAppointmentsCount();
        Task<int> GetDoctorsPendingAppointmentsCount(string doctorId);
        Task<int> GetDoctorsCompletedAppointmentsCount(string doctorId);
        Task<int> GetPatientPendingAppointmentsCount(string patientId);
        Task<int> GetPatientCompletedAppointmentsCount(string patientId);
    }
}

