using HMS.Models.Lab;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Lab
{
    public class EditLabProfileViewModel
    {
        [Required]
        public string LabId { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class LabProfilePictureViewModel
    {
        [Required]
        public string LabId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
   
}
