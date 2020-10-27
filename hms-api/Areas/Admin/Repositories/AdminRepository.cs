using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Doctor.Models;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Repositories
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public async Task<bool> BookAppointment(DoctorAppointment appointment)
        {
            try
            {
                _applicationDbContext.DoctorAppointments.Add(appointment);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }       
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllDoctors() =>
            await _applicationDbContext.ApplicationUsers.Where(d => d.UserType == "Doctor").ToListAsync();

        public async Task<DoctorProfile> GetDoctorsById(string Id)
        {
            return await _applicationDbContext.DoctorProfiles.Where(p => p.DoctorId == Id).FirstAsync();
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
