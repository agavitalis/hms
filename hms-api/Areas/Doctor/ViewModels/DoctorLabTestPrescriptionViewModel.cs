using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.ViewModels
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
