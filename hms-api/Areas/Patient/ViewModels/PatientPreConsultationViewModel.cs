using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.ViewModels
{
    public class PatientPreConsultationViewModel
    {
        public class UpdatePatientVitalsViewModel
        {
            public string PatientId { get; set; }
            public string BloodPressure { get; set; }
            public string Respiration { get; set; }
            public string Pulse { get; set; }
            public string SPO2 { get; set; }
            public string Temperature { get; set; }
        }

        public class UpdatePatientBMIViewModel
        {
            public string PatientId { get; set; }
            public string Weight { get; set; }
            public string Height { get; set; }
            public string CalculatedBMI { get; set; }
        }

    }
}
