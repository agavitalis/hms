using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.ViewModels
{
    public class PatientProfileViewModel
    {
        public class CreatePatientViewModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string OtherName { get; set; }
            public string Age { get; set; }
            public string Address { get; set; }
            public string Gender { get; set; }
            public string BloodGroup { get; set; }
            public string GenoType { get; set; }
        }

        public class EditPatientBasicInfoViewModel
        {
            public string PatientId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string OtherNames { get; set; }
            public string Age { get; set; }
            public string DateOfBirth { get; set; }
            public string Gender { get; set; }
           
        }

        public class PatientHealthViewModel
        {
            public string PatientId { get; set; }
            public string BloodGroup { get; set; }
            public string GenoType { get; set; }
            public string Allergies { get; set; }
            public string Disabilities { get; set; }
            public Boolean Diabetic { get; set; }
        }

        public class PatientAddressViewModel
        {
            public string PatientId { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
        }

        public class PatientListViewModel
        {
            public IEnumerable<Models.Patient.PatientProfile> Patients;
            public string sortParameter;
        }

        public class PatientProfilePictureViewModel
        {
            [Required]
            public string PatientId { get; set; }

            [Required]
            public IFormFile ProfilePicture { get; set; }
        }
    }
}
