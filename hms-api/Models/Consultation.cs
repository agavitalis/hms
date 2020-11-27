using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HMS.Models
{
    public class Consultation
    {
        public Consultation()
        {
            Id = Guid.NewGuid().ToString();
            IsCanceled = false;
            IsCompleted = false;
            IsExpired = false;
            IsNewPatient = false;
            IsPatientAdmitted = false;
            IsPatientSentHome = false;
            DateOfConsultation = DateTime.Now;
        }

        public string Id { get; set; }
        public DateTime DateOfConsultation { get; set; }
        public Boolean IsCanceled { get; set; }  //Did Patient cancel
        public Boolean IsCompleted { get; set; } //Appointment closed this appintment finalized
        public Boolean IsExpired { get; set; } //Day for consultation passed
        public string ConsultationTitle { get; set; }
        public string ReasonForConsultation { get; set; }
        public Boolean IsPatientSentHome { get; set; }
        public Boolean IsPatientAdmitted { get; set; }

        public Boolean IsNewPatient { get; set; }

        /*------ composite relationships-------*/
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
