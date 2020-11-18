﻿using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Repositories
{
    public class PatientAppointmentRepository : IPatientAppointment
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PatientAppointmentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> BookAppointment(AppointmentViewModel.BookAppointmentViewModel appointment)
        {
            var doctorAppointment = new Appointment()
            {

                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                AppointmentTitle = appointment.AppointmentTitle,
                ReasonForAppointment = appointment.ReasonForAppointment,

                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId
            };
            _applicationDbContext.DoctorAppointments.Add(doctorAppointment);

            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCanceledAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCanceled == true).CountAsync();

        public async Task<int> GetCompletedAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCompleted == true).CountAsync();

        public async Task<IEnumerable<Appointment>> GetPatientAppointments(string patientId) => _applicationDbContext.DoctorAppointments.Where(a => a.PatientId == patientId).Include(a => a.Doctor).ToList();

        public async Task<Appointment> GetPatientAppointment(string appointmentId) => await _applicationDbContext.DoctorAppointments.Where(a => a.Id == appointmentId).Include(a => a.Doctor).FirstOrDefaultAsync();

        public async Task<IEnumerable<Appointment>> GetPendingAppointments(string patientId) => await _applicationDbContext.DoctorAppointments.Where(a => a.PatientId == patientId && a.IsPending == true).Include(a => a.Doctor).ToListAsync();
      
        public async Task<int> GetPendingAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCompleted == false).CountAsync();

        public async Task<int> CancelAppointment(string appointmentId)
        {
            //check if the patient is in queue today
            var Appointment = await _applicationDbContext.DoctorAppointments.FirstOrDefaultAsync(d => d.Id == appointmentId);


            if (Appointment == null)
            {
                return 1;
            }
            else if (Appointment.IsCanceledByDoctor == true)
            {
                return 2;
            }
            else if (Appointment.IsCompleted == true)
            {
                return 3;
            }
            else if (Appointment.IsExpired == true)
            {
                return 4;
            }
            else if (Appointment.IsRejected == true)
            {
                return 5;
            }
           
            else
            {
                Appointment.IsCanceled = true;
                Appointment.IsPending = false;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }
    }
}
