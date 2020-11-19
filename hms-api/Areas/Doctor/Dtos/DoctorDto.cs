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
        //public string OfficeHours { get; set; }
    }

    public class DoctorEducationDtoForCreate
    {
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DoctorEducationDtoForView
    {
        public string Id { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorExperienceDtoForView
    { 

        public string Id { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorExperienceDtoForCreate
    {

        public string Role { get; set; }
        public string Company { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DoctorSocialDtoForView
    {
        public string Id { get; set; }
        public string HandleName { get; set; }
        public string Url { get; set; }
        public string DoctorId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorSocialDtoForCreate
    {
        public string HandleName { get; set; }
        public string Url { get; set; }
        public string DoctorId { get; set; }
        public string CreatedBy { get; set; }
    }
    
    public class DoctorOfficeTimeDtoForView
    {
        public string Id { get; set; }
        public string WorkDays { get; set; }
        public string WorkTime { get; set; }
        public string DoctorId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorOfficeTimeDtoForCreate
    {
        public string WorkDays { get; set; }
        public string WorkTime { get; set; }
        public string DoctorId { get; set; }
    }

    public class DoctorSkillsDtoForCreate
    {
        public string Skills { get; set; }
        public string DoctorId { get; set; }
    }

    public class DoctorSkillsDtoForView
    {
        public string Id { get; set; }
        public string Skills { get; set; }
        public string DoctorId { get; set; }
    }

}
