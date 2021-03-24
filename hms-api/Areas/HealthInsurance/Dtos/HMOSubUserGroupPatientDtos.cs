

namespace HMS.Areas.NHIS.Dtos
{
    public class HMOSubUserGroupPatientDtoForCreate
    {
        public string HMOSubUserGroupId { get; set; }
        public string PatientId { get; set; }
    }

    public class HMOSubUserGroupPatientDtoForDelete
    {
        public string Id { get; set; }
    }
}
