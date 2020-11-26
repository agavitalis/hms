using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Pharmacy.ViewModels
{

    public class EditPharmacistBasicInfoViewModel
    {
        [Required]
        public string PharmacistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class EditPharmacistContactDetailsViewModel
    {
        [Required]
        public string PharmacistId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class PharmacyProfilePictureViewModel
    {
        [Required]
        public string PharmacyId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
   
}
