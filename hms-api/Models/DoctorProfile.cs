using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class DoctorProfile
    {
        public DoctorProfile()
        {
            Id = Guid.NewGuid().ToString();
            //IsAvaliable = false;
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
        public string DoctorId { get; set; }
        public virtual ApplicationUser Doctor { get; set; }
        public virtual ICollection<DoctorSocial> Socials  { get; set; }
        public virtual ICollection<DoctorEducation> Educations { get; set; }
        public virtual ICollection<DoctorExperience> Experiences { get; set; }
        public virtual ICollection<DoctorOfficeTime> OfficeTime { get; set; }
        public virtual ICollection<DoctorSpecialization> Specializations { get; set; }

    }
}
