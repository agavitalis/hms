using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Doctor
{
    public class DoctorSpecialization
    {
        public DoctorSpecialization()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }


        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string DoctorId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}