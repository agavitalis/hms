﻿using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
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

        public async Task<bool> DeleteAppointment(Appointment appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return false;
                }

                _applicationDbContext.DoctorAppointments.Remove(appointment);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Appointment> GetAppointment(string AppointmentId) => await _applicationDbContext.DoctorAppointments.Where(a => a.Id == AppointmentId).FirstOrDefaultAsync();
      

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

        public async Task<int> GetDoctorsPendingAppointmentsCount(string doctorId)
        {
            var doctorPendingAppointmentsCount = await _applicationDbContext.DoctorAppointments.Where(a => a.IsPending == true && a.DoctorId == doctorId).CountAsync();
            return doctorPendingAppointmentsCount;
        }

        public async Task<int> GetDoctorsCompletedAppointmentsCount(string doctorId)
        {
            var doctorCompletedAppointmentsCount = await _applicationDbContext.DoctorAppointments.Where(a => a.IsCompleted == true && a.DoctorId == doctorId).CountAsync();
            return doctorCompletedAppointmentsCount;
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

        public async Task<MyPatient> CheckDoctorInMyPatients(string DoctorId, string PatientId) => await _applicationDbContext.MyPatients.Where(p => p.DoctorId == DoctorId && p.PatientId == PatientId).FirstOrDefaultAsync();

        public PagedList<AppointmentDtoForView> GetAppointmentsPagination(PaginationParameter paginationParameter)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).OrderByDescending(a => a.AppointmentTime).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AppointmentDtoForView> GetAppointmentsAccepted(PaginationParameter paginationParameter)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsAccepted == true).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).OrderByDescending(a => a.AppointmentTime).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AppointmentDtoForView> GetAppointmentsPending(PaginationParameter paginationParameter)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsPending == true).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).OrderByDescending(a => a.AppointmentTime).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<AppointmentDtoForView> GetAppointmentsCompleted(PaginationParameter paginationParameter)
        {
            var appointments = _applicationDbContext.DoctorAppointments.Where(a => a.IsCompleted == true).Include(a => a.Patient).Include(a => a.Doctor).OrderByDescending(a => a.AppointmentDate).OrderByDescending(a => a.AppointmentTime).ToList();
            var appointmentsToReturn = _mapper.Map<IEnumerable<AppointmentDtoForView>>(appointments);
            return PagedList<AppointmentDtoForView>.ToPagedList(appointmentsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

    }
}
