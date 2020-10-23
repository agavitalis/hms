using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models.Patient
{
    public class PatientQueue
    {
        public PatientQueue()
        {
            Id = Guid.NewGuid().ToString();
            IsCanceled = false;
            IsCompleted = false;
            IsExpired = false;
            IsNewPatient = false;
            DateOfConsultation = DateTime.Now;
        }

        public string Id { get; set; }
        public DateTime DateOfConsultation { get; set; }
        public Boolean IsCanceled { get; set; }  //Did Patient cancel
        public Boolean IsCompleted { get; set; } //Appointment closed this appintment finalized
        public Boolean IsExpired { get; set; } //Day for consultation passed
        public string ConsultationTitle { get; set; }
        public string ReasonForConsultation { get; set; }
        
        public Boolean IsNewPatient { get; set; }

        /*------ composite relationships-------*/
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
