using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class DoctorClerking
    {
        public DoctorClerking()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string PresentingComplaints { get; set; }
        public string HistroyOfPresentingComplaints { get; set; }
        public string ReviewOfSystem { get; set; }
        public string PhysicalExamination { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentPlan { get; set; }
        public string ObstetricsAndGynecology { get; set; }
        public string ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

    }
}
