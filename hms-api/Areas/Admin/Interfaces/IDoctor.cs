using HMS.Areas.Doctor.Models;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
<<<<<<< HEAD
    interface IDoctor
=======
   public interface IDoctor
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6
    {
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);
    }
}
