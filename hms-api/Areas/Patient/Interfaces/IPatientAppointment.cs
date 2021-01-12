using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.AppointmentViewModel;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientAppointment
    {
        Task<int> GetPendingAppointmentsCount(string patientId);
        Task<int> GetCompletedAppointmentsCount(string patientId);
        Task<int> GetCanceledAppointmentsCount(string patientId);
        Task<bool> AssignDoctorToPatient(MyPatient patient);
        Task<bool> BookAppointment(BookAppointmentViewModel appointment);
        Task<IEnumerable<Appointment>> GetPendingAppointments(string patientId);
        Task<IEnumerable<Appointment>> GetPatientAppointments(string patientId);
        Task<Appointment> GetPatientAppointment(string appointmentId);
        Task<int> CancelAppointment(string appointmentId);
    }
}
