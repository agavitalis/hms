using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Patient.ViewModels;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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
        private readonly IMapper _mapper;

        public PatientAppointmentRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
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

        public async Task<bool> AssignDoctorToPatient(MyPatient patient)
        {
            try
            {
                _applicationDbContext.MyPatients.Add(patient);

                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public PagedList<AppointmentDtoForView> GetCompletedAppointments(PaginationParameter paginationParameter, string PatientId)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsCompleted == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AppointmentDtoForView> GetPendingAppointments(PaginationParameter paginationParameter, string PatientId)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsPending == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AppointmentDtoForView> GetCanceledAppointments(PaginationParameter paginationParameter, string PatientId)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsCanceled == true && a.PatientId == PatientId).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<Appointment> GetPatientAppointment(string appointmentId) => await _applicationDbContext.DoctorAppointments.Where(a => a.Id == appointmentId).Include(a => a.Patient).Include(a => a.Doctor).FirstOrDefaultAsync();
        
    }
}
