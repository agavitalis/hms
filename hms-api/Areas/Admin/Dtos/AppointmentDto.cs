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

    public class ReassignAppointmentDto
    {
        public string AppointmentId { get; set; }
        public string DoctorId { get; set; }
    }
}
