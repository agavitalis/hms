using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class PatientPreConsultation
    {
        public PatientPreConsultation()
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now;
        }
        public string Id { get; set; }
        public string BloodPressure { get; set; }
        public string Pulse { get; set; }
        public string Respiration { get; set; }
        public string SPO2 { get; set; }
        public string Temperature { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string CalculatedBMI { get; set; }
        public DateTime Date { get; set; }

        /*------ relationships-------*/
        [ForeignKey("ApplicationUser")]
        public string PatientId { get; set; }

        public virtual ApplicationUser Patient { get; set; }
    }
}
