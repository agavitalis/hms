using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctor
    {
        Task<DoctorAppointment> GetAppointment(string Id);
    }
}
