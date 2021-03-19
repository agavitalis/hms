using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class HMOAdminProfile
    {
        public HMOAdminProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //personal details
        public string FullName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }

        //contact details
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string HMOId { get; set; }
        public HMO HMO { get; set; }
        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string HMOAdminId { get; set; }
        public virtual ApplicationUser HMOAdmin { get; set; }
    }
}
