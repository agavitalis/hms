using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientAppointment
    {
        Task<int> GetPendingAppointmentsCount(string patientId);
        Task<int> GetCompletedAppointmentsCount(string patientId);
        Task<int> GetCanceledAppointmentsCount(string patientId);
        Task<IEnumerable<Appointment>> GetPendingAppointments(string patientId);
    }
}
