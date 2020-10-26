using HMS.Models.Account;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Account.ViewModels
{
    public class AccountProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public int BloodGroup { get; set; }
        public int GenoType { get; set; }
    }

    public class EditAccountProfileViewModel
    {
        [Required]
        public string AccountantId { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
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
        public IEnumerable<AccountProfile> Accountant;
        public string SortParameter;
    }
}
