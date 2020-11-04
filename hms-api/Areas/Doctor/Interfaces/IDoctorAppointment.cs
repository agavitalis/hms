using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorAppointment
    {
        Task<Appointment> GetAppointment(string Id);
    }
}
