using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class Surgery
    {
        public Surgery()
        {
            Id = Guid.NewGuid().ToString();

        }
        public string Id { get; set; }
        public bool HospitalistService { get; set; }
        public bool MedSurgService { get; set; }
        public bool ICU { get; set; }
        public bool TELE { get; set; }
        public string SurgeryAndDiagnosis { get; set; }
        public string SecondaryDiagnosis { get; set; }
        public bool Allergies { get; set; }
        public bool OnChat { get; set; }
        public bool CompletedByPCPCall { get; set; }
        public string Dietary { get; set; }
        public bool VSEveryFourHours { get; set; }
        public bool VSEveryEightHours { get; set; }
        public bool VSPerUnitProtocol { get; set; }
        public bool IAndDWeightDaily { get; set; }
        public bool BedRest { get; set; }
        public bool OOBToChain { get; set; }
        public bool AMBAsTol { get; set; }
        public bool ManagementPerPDHPolicy { get; set; }
        public bool JacksonPratt { get; set; }
        public bool Hamovac { get; set; }
        public bool Penrose { get; set; }
        public bool Dressing { get; set; }
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
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        public static implicit operator Surgery(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
