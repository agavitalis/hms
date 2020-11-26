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
        Task<int> GetDoctorsPendingAppointmentsCount();
    }
}

