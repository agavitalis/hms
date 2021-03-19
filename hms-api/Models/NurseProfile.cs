using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class NurseProfile
    {
        public NurseProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //personal info
        public string FullName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

        //address
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }


        ////Doctor Availablity
        public Boolean isAvailable { get; set; }

        /*------ relationships-------*/
        [ForeignKey("ApplicationUser")]
        public string NurseId { get; set; }
        public ApplicationUser Nurse { get; set; }
    }
}
