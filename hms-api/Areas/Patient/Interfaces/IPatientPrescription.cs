using HMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientPrescription
    {
        Task<IEnumerable<dynamic>> GetPatientPrescriptionByAppointment(string appointmentId);
        Task<IEnumerable<dynamic>> AllPatientPrescription(string patientId);
       
        
        
    }
}
