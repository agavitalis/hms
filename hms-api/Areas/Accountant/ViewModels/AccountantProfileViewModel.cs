using HMS.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Accountant.ViewModels
{
    public class EditAccountantBasicInfoViewModel
    {
        [Required]
        public string AccountantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class EditAccountantContactDetailsViewModel
    {
        [Required]
        public string AccountantId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class AccountProfilePictureViewModel
    {
        [Required]
        public string AccountProfileId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
    public class AccountantListViewModel
    {
        public IEnumerable<AccountantProfile> Accountant;
        public string SortParameter;
    }
}
