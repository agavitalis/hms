using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Database;
using HMS.Models;
using HMS.Areas.Doctor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Services.Helpers;
using HMS.Areas.Patient.Models;

namespace HMS.Areas.Admin.Repositories
{
    public class AppointmentRepository : IAppointment
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public AppointmentRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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


        public async Task<dynamic> GetDoctorsAppointment()
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
