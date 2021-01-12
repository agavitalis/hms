using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorAppointment
    {
        Task<Appointment> GetAppointmentById(string Id);
        Task<IEnumerable<Appointment>> GetDoctorAppointments(string DoctorId);
        Task<Appointment> GetDoctorAppointment(string appointmentId);
        Task<int> AcceptAppointment(Appointment appointment);
        Task<int> RejectAppointment(Appointment appointment);
        Task<int> CancelAppointment(Appointment appointment);
        Task<int> AdmitPatientOrSendPatientHome(CompletDoctorClerkingDto clerking);
        Task<bool> UpdateAppointment(Appointment appointment);
    }
}
