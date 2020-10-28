using HMS.Areas.Admin.Dtos;
<<<<<<< HEAD
=======
using HMS.Areas.Doctor.Models;
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
<<<<<<< HEAD
    interface IAppointment
    {
        Task<dynamic> GetDoctorsAppointment();
        Task<bool> BookAppointment(BookAppointmentDto appointment);
=======
    public interface IAppointment
    {
        Task<dynamic> GetDoctorsAppointment();
        Task<bool> BookAppointment(DoctorAppointment appointment);
      
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
    }
}
