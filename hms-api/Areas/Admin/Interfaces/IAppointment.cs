using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IAppointment
    {
        Task<dynamic> GetDoctorsAppointment();
        Task<bool> BookAppointment(DoctorAppointment appointment);

    }
}

