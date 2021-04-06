
namespace HMS.Areas.HealthInsurance.Dtos
{
    public class NHISHealthPlanPatientDtoForCreate
    {
        public string PatientId { get; set; }
        public string NHISHealthPlanId { get; set; }
    }

    public class NHISHealthPlanPatientDtoForUpdate
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string NHISHealthPlanId { get; set; }
        public string AuthorizationCode { get; set; }
    }

    public class NHISHealthPlanPatientDtoForDelete
    {
        public string Id { get; set; }
    }
}
