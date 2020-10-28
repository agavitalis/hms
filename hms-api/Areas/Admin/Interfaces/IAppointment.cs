using HMS.Areas.Admin.Dtos;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    interface IAppointment
    {
        Task<dynamic> GetDoctorsAppointment();
        Task<bool> BookAppointment(BookAppointmentDto appointment);
    }
}
