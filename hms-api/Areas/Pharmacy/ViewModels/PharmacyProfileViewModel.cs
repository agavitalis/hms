using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HMS.Models.Pharmacy;

namespace HMS.Areas.Pharmacy.ViewModels
{

    public class EditPharmacyProfileViewModel
    {
        [Required]
        public string PharmacyId { get; set; }
        public string Gender { get; set; }
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
