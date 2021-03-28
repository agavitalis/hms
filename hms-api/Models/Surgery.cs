using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Surgery
    {
        public Surgery()
        {
            Id = Guid.NewGuid().ToString();
            HospitalistService = false;
            MedSurgService = false;
            ICU = false;
            TELE = false;
            Allergies = false;
            IAndDWeightDaily = false;
            BedRest = false;
            OOBToChain = false;
            AMBAsTol = false;
            ManagementPerPDHPolicy = false;
            JacksonPratt = false;
            Hamovac = false;
            Penrose = false;
          
            UnusualWoundDrainage = false;
            Pantoprazole = false;
            Famotidine = false;
        }
        public string Id { get; set; }
        public string ReferralNote { get; set; }
        public bool HospitalistService { get; set; }
        public bool MedSurgService { get; set; }
        public bool ICU { get; set; }
        public bool TELE { get; set; }
        public string SurgeryAndDiagnosis { get; set; }
        public string SecondaryDiagnosis { get; set; }
        public bool Allergies { get; set; }
        public string AdvancedDirectives { get; set; }
        public string Dietary { get; set; }
        public string VsFrequency { get; set; }
        public bool IAndDWeightDaily { get; set; }
        public bool BedRest { get; set; }
        public bool OOBToChain { get; set; }
        public bool AMBAsTol { get; set; }
        public bool ManagementPerPDHPolicy { get; set; }
        public bool JacksonPratt { get; set; }
        public bool Hamovac { get; set; }
        public bool Penrose { get; set; }
        public string Dressing { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HRLowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HRUpperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal RPLowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal RPUpperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SBPLowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SBPUpperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TemperatureLowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TemperatureUpperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DPBLowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DPBUpperLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SPO2LowerLimit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SPO2UpperLimit { get; set; }
        public bool UrineOutput { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Haemoglobin { get; set; }
        public bool UnusualWoundDrainage { get; set; }
        public bool Pantoprazole { get; set; }
        public bool Famotidine { get; set; }
        public string InfectionPrevention { get; set; }
        public string RespiratoryCare { get; set; }
        public string PostOperationMedication { get; set; }
        public string Surgeons { get; set; }
        public string Anasthetics { get; set; }
        public string Operation { get; set; }
        public string SurgeryIndication { get; set; }
        public string OperationProcedure { get; set; }
        public string AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string ConsultationId { get; set; }
        public Consultation Consultation { get; set; }
        public string InitiatorId { get; set; }
        public ApplicationUser Initiator { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public DateTime DateOfSurgery { get; set; }
        public DateTime TimeOfSurgery { get; set; }
    }
}
