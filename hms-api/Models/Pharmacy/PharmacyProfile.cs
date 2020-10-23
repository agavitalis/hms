using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Pharmacy
{
    public class PharmacyProfile
    {

        public PharmacyProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }


        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string PharmacyId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
