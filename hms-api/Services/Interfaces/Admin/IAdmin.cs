using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces.Admin
{
    public interface IAdmin
    {
        Task<dynamic> GetDoctorsPatientAppointment();
    }
}
