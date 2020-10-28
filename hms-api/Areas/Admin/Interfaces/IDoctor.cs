using HMS.Areas.Doctor.Models;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    interface IDoctor
    {
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);
    }
}
