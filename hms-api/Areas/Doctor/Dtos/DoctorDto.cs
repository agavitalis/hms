using HMS.Models;
using System.Collections.Generic;

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
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string UserType { get; set; }
        public virtual ICollection<DoctorSocial> Socials { get; set; }
        public virtual ICollection<DoctorEducation> Educations { get; set; }
        public virtual ICollection<DoctorExperience> Experiences { get; set; }
        public virtual ICollection<DoctorOfficeTime> OfficeTime { get; set; }
        public virtual ICollection<DoctorSpecialization> Specializations { get; set; }
    }

    public class DoctorEducationDtoForCreate
    {
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorProfileId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DoctorEducationDtoForView
    {
        public string Id { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorProfileId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorExperienceDtoForView
    { 

        public string Id { get; set; }
        public string Role { get; set; }
        public string Company { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorProfileId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorExperienceDtoForCreate
    {

        public string Role { get; set; }
        public string Company { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string DoctorProfileId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DoctorSocialDtoForView
    {
        public string Id { get; set; }
        public string Website { get; set; }
        public string Url { get; set; }
        public string DoctorProfileId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorSocialDtoForCreate
    {
        public string Website { get; set; }
        public string Url { get; set; }
        public string DoctorProfileId { get; set; }
        public string CreatedBy { get; set; }
    }
    
    public class DoctorOfficeTimeDtoForView
    {
        public string Id { get; set; }
        public string WorkDays { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DoctorProfileId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class DoctorOfficeTimeDtoForCreate
    {
        public string WorkDays { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DoctorProfileId { get; set; }
    }

    public class DoctorSpecializationsDtoForCreate
    {
        public string Specialization { get; set; }
        public string DoctorProfileId { get; set; }
    }

    public class DoctorSpecializationsDtoForView
    {
        public string Id { get; set; }
        public string Specialization { get; set; }
        public string DoctorProfileId { get; set; }
    }

}
