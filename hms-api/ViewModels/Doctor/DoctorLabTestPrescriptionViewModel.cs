using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Doctor
{
    public class LabTestPrescriptionViewModel
    {
        public string LabTestId { get; set; }
        public string CommentByDoctor { get; set; }

        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

    }
    
}
