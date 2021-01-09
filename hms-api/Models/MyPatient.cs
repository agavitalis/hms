using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HMS.Models
{
    public class MyPatient
    {
        public MyPatient()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }

        public string Id { get; set; }

        /*------ composite relationships-------*/
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }

        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        public DateTime DateCreated { get; set; }

    }
}

