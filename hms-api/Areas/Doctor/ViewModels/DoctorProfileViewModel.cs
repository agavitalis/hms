using HMS.Areas.Doctor.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Doctor.ViewModels
{

    public class EditDoctorProfileViewModel
    {
        [Required]
        public string DoctorId { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
    }

    public class EditDoctorBasicInfoViewModel
    {
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

    }

    public class DoctorContactViewModel
    {
        public string DoctorId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class DoctorProfessionalDetailsViewModel
    {
        public string DoctorId { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string Specialization { get; set; }
       
    }

    public class DoctorAvaliablityViewModel
    {
        public string DoctorId { get; set; }
        public string OfficeHours { get; set; }
        public bool IsAvaliable { get; set; }
       
    }


    public class DoctorProfilePictureViewModel
    {
        [Required]
        public string DoctorId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
    public class DoctorListViewModel
    {
        public IEnumerable<DoctorProfile> Doctor;
        public string SortParameter;
    }
}
