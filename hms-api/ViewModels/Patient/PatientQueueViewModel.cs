using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Patient
{
    public class PatientQueueViewModel
    {
      

    }
    public class AdminAddPatientToQueueViewModel
    {
        public string ConsultationTitle { get; set; }
        public string ReasonForConsultation { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorId { get; set; }
    }
    public class AddPatientToQueueViewModel
    {
        public string ConsultationTitle { get; set; }
        public string ReasonForConsultation { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
    }

}
