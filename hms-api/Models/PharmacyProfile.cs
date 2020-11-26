  using HMS.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class PharmacyProfile
    {

        public PharmacyProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string  FullName {get; set;}
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }


        /*------ relationships-------*/
        public string PharmacyId { get; set; }
        public ApplicationUser Pharmacy { get; set; }

    }
}
