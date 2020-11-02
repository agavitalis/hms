using HMS.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class DoctorProfile
    {
        public DoctorProfile()
        {
            Id = Guid.NewGuid().ToString();
            IsAvaliable = false;
        }

        public string Id { get; set; }

        //personal info
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }

        //address
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        //professional details
        public string About { get; set; }
        public string Education { get; set; }
        public string Specialization { get; set; }

        //Doctor Avaliablity
        public string OfficeHours { get; set; }
        public Boolean IsAvaliable { get; set; }

        /*------ relationships-------*/
        [ForeignKey("ApplicationUser")]
        public string DoctorId { get; set; }
        public virtual ApplicationUser Doctor { get; set; }

    }
}
