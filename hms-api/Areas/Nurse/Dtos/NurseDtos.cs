using HMS.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Nurse.Dtos
{
    public class NurseDtoForView
    {
        public string Id { get; set; }
        public string NurseId { get; set; }
        public ApplicationUser Nurse { get; set; }
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
    }

    public class NurseBasicInfoDtoForEdit
    {
        [Required]
        public string NurseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class NurseContactDetailsDtoForEdit
    {
        [Required]
        public string NurseId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class NurseProfilePictureDtoForEdit
    {
        [Required]
        public string NurseId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }

}
