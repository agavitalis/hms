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
        public string SocialHistory { get; set; }
        public string FamilyHistory { get; set; }
        public string MedicalHistory { get; set; }
        public string LastCountryVisited { get; set; }
        public string DateOfVisitation { get; set; }
        public string PresentingComplaints { get; set; }
        public string HistoryOfPresentingComplaints { get; set; }
        public string ReviewOfSystem { get; set; }
        public string PhysicalExamination { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentPlan { get; set; }
        public string ObstetricsAndGynecology { get; set; }
        public string ConsultationId { get; set; }
        public Consultation Consultation { get; set; }

        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

    }
}
