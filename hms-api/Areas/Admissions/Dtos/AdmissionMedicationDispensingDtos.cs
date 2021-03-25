using HMS.Models;
using System;

namespace HMS.Areas.Admissions.Dtos
{
    public class AdmissionMedicationDispensingDtoForCreate
    {
        public string DrugId { get; set; }
        public Drug Drug { get; set; }
        public DateTime DateDispensed { get; set; }
        public string AdmissionId { get; set; }
        public string InitiatorId { get; set; }
    }
}
