using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Patient
{
    public class AppointmentViewModel
    {
        public class BookAppointmentViewModel
        {
            public DateTime AppointmentDate { get; set; }
            public DateTime AppointmentTime { get; set; }

            public string AppointmentTitle { get; set; }

            public string ReasonForAppointment { get; set; }

            public string DoctorId { get; set; }

            public string PatientId { get; set; }

        }
     
    }
}
