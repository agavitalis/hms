using HMS.Areas.Lab.Models;
using System;

namespace HMS.Areas.Patient.Models
{
    public class PatientLabPrescription
    {
        public PatientLabPrescription()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string LabTestId { get; set; }
        public virtual LabTest LabTest { get; set; }
        public string CommentByDoctor { get; set; }
        public Boolean isLabTestSelected { get; set; } = false;

        public Boolean isPaid { get; set; } = false;

        public Boolean isDispatched { get; set; } = false;

        public string status { get; set; } = "undone";

        public string LabTestResult { get; set; }
        public string CommentByLabTechnician { get; set; }

        /*------ relationships-------*/
        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

    }
}
