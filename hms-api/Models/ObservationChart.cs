using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class ObservationChart
    {
        public ObservationChart()
        {
            Date = DateTime.Now;
            Id = Guid.NewGuid().ToString();
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
        public string Remarks { get; set; }
        public DateTime Date { get; set; }

        /*------ relationships-------*/
      
        public string AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }
        [ForeignKey("ApplicationUser")]

        public string InitiatorId { get; set; }
        public virtual ApplicationUser Initiator { get; set; }
    }
}
