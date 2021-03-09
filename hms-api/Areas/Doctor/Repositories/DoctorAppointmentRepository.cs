using HMS.Database;
using HMS.Areas.Doctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models;
using System.Collections.Generic;
using HMS.Areas.Doctor.Dtos;
using System;

namespace HMS.Areas.Doctor.Repositories
{
    public class DoctorAppointmentRepository : IDoctorAppointment
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DoctorAppointmentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AcceptAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return 1;
            }
            
            else if (appointment.IsCompleted == true)
            {
                return 2;
            }
            else if (appointment.IsExpired == true)
            {
                return 3;
            }
           

            else
            {
                appointment.IsAccepted = true;
                appointment.IsPending = false;
                appointment.IsRejected = false;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<int> CancelAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return 1;
            }

            else if (appointment.IsCompleted == true)
            {
                return 2;
            }
            else if (appointment.IsExpired == true)
            {
                return 3;
            }
            else if (appointment.IsCanceled == true)
            {
                return 4;
            }


            else
            {
                appointment.IsAccepted = false;
                appointment.IsPending = false;
                appointment.IsCanceledByDoctor = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return false;
                }

                _applicationDbContext.DoctorAppointments.Update(appointment);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> AdmitPatientOrSendPatientHome(CompletDoctorClerkingDto clerking)
        {
            var appointment = await _applicationDbContext.DoctorAppointments.FirstOrDefaultAsync(d => d.Id == clerking.Id);

            if (appointment == null)
            {
                return 1;
            }

            else if (appointment.IsRejected == true)
            {
                return 2;
            }
            else if (appointment.IsExpired == true)
            {
                return 3;
            }
            else if (appointment.IsCanceled == true)
            {
                return 4;
            }


            else
            {
                appointment.IsPending = false;
                appointment.IsPatientAdmitted = clerking.IsAdmitted;
                appointment.IsPatientSentHome = clerking.IsSentHome;
                appointment.IsCompleted = true;
               
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }

        public async Task<Appointment> GetAppointmentById(string Id) => await _applicationDbContext.DoctorAppointments.FirstOrDefaultAsync(d => d.Id == Id);
      
        public async Task<Appointment> GetDoctorAppointment(string appointmentId)
        {
            var appointment = await _applicationDbContext.DoctorAppointments.FirstOrDefaultAsync(d => d.Id == appointmentId);
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetDoctorAppointments(string DoctorId)
        { 
            return _applicationDbContext.DoctorAppointments.Where(a => a.DoctorId == DoctorId).Include(a => a.Patient).ToList();
        }

        public async Task<int> RejectAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return 1;
            }

            else if (appointment.IsCompleted == true)
            {
                return 2;
            }
            else if (appointment.IsExpired == true)
            {
                return 3;
            }


            else
            {
                appointment.IsAccepted = false;
                appointment.IsPending = false;
                appointment.IsRejected = true;
                await _applicationDbContext.SaveChangesAsync();

                return 0;
            }
        }
    }
}
