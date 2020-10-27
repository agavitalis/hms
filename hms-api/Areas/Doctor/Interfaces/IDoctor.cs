using HMS.Areas.Doctor.Models;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctor
    {
        Task<DoctorAppointment> GetAppointment(string Id);
    }
}
