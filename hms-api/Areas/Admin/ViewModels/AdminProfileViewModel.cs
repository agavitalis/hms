using HMS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.ViewModels
{
    public class EditAdminBasicInfoViewModel
    {
        [Required]
        public string AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class EditAdminContactDetailsViewModel
    {
        [Required]
        public string AdminId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class AdminProfilePictureViewModel
    {
        [Required]
        public string AdminProfileId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
    public class AccountantListViewModel
    {
        public IEnumerable<AdminProfile> Accountant;
        public string SortParameter;
    }
}
