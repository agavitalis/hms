using Microsoft.AspNetCore.Identity;

namespace HMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }

        public string ProfileImageUrl { get; set; }

        public string UserType { get; set; }

    }
}
