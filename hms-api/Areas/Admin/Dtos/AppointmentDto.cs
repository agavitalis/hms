using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
   
    public class BookAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }

        public string AppointmentTitle { get; set; }
        public string ReasonForAppointment { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }

    public class AppointmentDtoForView
    {
        public string Id { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentTitle { get; set; }
        public string ReasonForAppointment { get; set; }

        public Boolean IsNewPatient = false;
        public Boolean IsPending { get; set; }
        public Boolean IsAccepted { get; set; }
        public Boolean IsRejected { get; set; } 
        public Boolean IsCanceled { get; set; }  
        public Boolean IsCompleted { get; set; } 
        public Boolean IsExpired { get; set; } 
        public Boolean IsCanceledByDoctor { get; set; }  
        public Boolean IsPatientSentHome { get; set; }
        public Boolean IsPatientAdmitted { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual ApplicationUser Patient { get; set; }
    }

    public class ReassignAppointmentDto
    {
        public string AppointmentId { get; set; }
        public string DoctorId { get; set; }
    }

    public class DeleteAppointmentDto
    {
        public string AppointmentId { get; set; }
    }
}
