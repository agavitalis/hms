using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Dtos
{
    public class DoctorClerkingDtoForCreate
    {
        public string SocialHistory { get; set; }
        public string FamilyHistory { get; set; }
        public string MedicalHistory { get; set; }
        public string LastCountryVisted { get; set; }
        public string DateOfVisitation { get; set; }
        public string PresentingComplaints { get; set; }
        public string HistoryOfPresentingComplaints { get; set; }
        public string ReviewOfSystem { get; set; }
        public string PhysicalExamination { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentPlan { get; set; }
        public string ObstetricsAndGynecology { get; set; }
        public string Prescription { get; set; }
        public string ConsultationId { get; set; }
        public string AppointmentId { get; set; }
    }

   

    
    public class DoctorClerkingDtoForUpdate
    {
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
        public string Prescription { get; set; }
        public string ConsultationId { get; set; }
        public string AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }


    public class CompletDoctorClerkingDto
    {
        public string Id { get; set; }
        public bool IsAdmitted { get; set; }
        public bool IsSentHome { get; set; }
    }
}
