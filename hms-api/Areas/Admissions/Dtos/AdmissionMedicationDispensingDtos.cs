using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionMedicationDispensingDtoForCreate
    {
        public string Medication { get; set; }
        public DateTime DateDispensed { get; set; }
        public string AdmissionId { get; set; }
        public string InitiatorId { get; set; }
    }
}
