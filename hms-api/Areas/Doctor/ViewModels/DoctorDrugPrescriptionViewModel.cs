using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.ViewModels
{
    public class DrugPrescriptionViewModel
    { 
        public string Ailment { get; set; }
        public string DrugId { get; set; }
        public string Quantity { get; set; }
        public string Frequency { get; set; }
        public string Dosage { get; set; }
        public string Comment { get; set; }

        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

    }
    
}
