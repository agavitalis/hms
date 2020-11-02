using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IDoctor
    {
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);
    }
}
