

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOHealthPlanPatientDtoForCreate
    {
        public string PatientId { get; set; }
        public string HMOHealthPlanId { get; set; }
    }

    public class HMOHealthPlanPatientDtoForDelete
    {
        public string Id { get; set; }
    }
}
