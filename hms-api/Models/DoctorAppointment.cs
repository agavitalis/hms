using HMS.Models;
using System;



namespace HMS.Models
{
    public class DoctorAppointment
    {
        public DoctorAppointment()
        {
            Id = Guid.NewGuid().ToString();
            IsAccepted = false;
            IsRejected = false;
            IsCanceled = false;
            IsCompleted = false;
            IsExpired = false;
            IsCanceledByDoctor = false;

        }

        public string Id { get; set; }

        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentTitle { get; set; }
        public string ReasonForAppointment { get; set; }

        public Boolean IsNewPatient = false;

        public Boolean IsAccepted { get; set; }
        public Boolean IsRejected { get; set; }  //Did doctor reject
        public Boolean IsCanceled { get; set; }  //Did Patient cancel
        public Boolean IsCompleted { get; set; } //Did Patient complete, and doctor paid
        public Boolean IsExpired { get; set; } //Appointment closed this appintment finalized
        public Boolean IsCanceledByDoctor { get; set; }  //was initially accepted by later rejected
     
        /*------Secondary Keys-------*/
        public string DoctorId { get; set; } 
        public string PatientId { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual ApplicationUser Patient { get; set; }
       
    }
}
