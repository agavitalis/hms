using HMS.Areas.Patient.Interfaces;
using HMS.Database;
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

        public async Task<int> GetCanceledAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCanceled == true).CountAsync();

        public async Task<int> GetCompletedAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCompleted == true).CountAsync();

        public async Task<int> GetPendingAppointmentsCount(string patientId) => await _applicationDbContext.DoctorAppointments.Where(c => c.PatientId == patientId && c.IsCompleted == false).CountAsync();
    }
}
