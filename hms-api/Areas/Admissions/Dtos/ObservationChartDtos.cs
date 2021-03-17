

namespace HMS.Areas.Admissions.Dtos
{
    public class ObservationChartDtoForUpdate
    {
        public string AdmissionId { get; set; }
        public string BloodPressure { get; set; }
        public string Respiration { get; set; }
        public string Pulse { get; set; }
        public string SPO2 { get; set; }
        public string Temperature { get; set; }
        public string Remarks { get; set; }
        public string InitiatorId { get; set; }
        
    }
}
