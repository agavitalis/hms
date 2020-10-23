using HMS.Database;
using HMS.Services.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories.Admin
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }
        public async Task<dynamic> GetDoctorsPatientAppointment()
        {
            var doctorAppointments = await _applicationDbContext.DoctorAppointments

             .Join(
                 _applicationDbContext.ApplicationUsers,
                 appointment => appointment.PatientId,
                 applicationUser => applicationUser.Id,
                 (appointment, patient) => new { appointment, patient }
             )
             .Join(
                 _applicationDbContext.ApplicationUsers,
                  appointment => appointment.appointment.DoctorId,
                 applicationUser => applicationUser.Id,
                 (appointment, doctor) => new { appointment.appointment, appointment.patient, doctor }
             )
             .ToListAsync();

            return doctorAppointments;                        
        }
    }
}
