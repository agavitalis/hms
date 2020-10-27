using HMS.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Areas.Patient.Models
{
    public class PatientProfile
    {
        public PatientProfile()
        {
            Id = Guid.NewGuid().ToString();
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

        //health details
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
        public string Allergies { get; set; }
        public string Disabilities { get; set; }
        public Boolean Diabetic { get; set; }

        //consent code
        public string ConsentCode { get; set; }


        /*------ relationships-------*/
        [ForeignKey("ApplicationUser")]
        public string PatientId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
