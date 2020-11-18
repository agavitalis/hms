﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        public async Task<bool> BookAppointment(Appointment appointment)
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
            var doctorAppointments = await _applicationDbContext.DoctorAppointments.Include(a => a.Patient).Include(a => a.Doctor).ToListAsync();
            return doctorAppointments;
        }

        public async Task<int> GetDoctorsPendingAppointmentsCount()
        {
            var appointmentsCount = await _applicationDbContext.DoctorAppointments.Where(a=> a.IsPending == true).CountAsync();
            return appointmentsCount;
        }

    }
}
