using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Doctor
{
    public interface IDoctor
    {
        Task<DoctorAppointment> GetAppointment(string Id);
    }
}
