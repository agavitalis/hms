using HMS.Areas.Doctor.Models;
using HMS.Models;

namespace HMS.Areas.Doctor.Dtos
{
    public class DoctorDto
    {
        public DoctorProfile DoctorProfile { get; set; }
        public ApplicationUser Doctor { get; set; }
    }

    public class DoctorDtoForView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string OfficeHours { get; set; }

    }
}
