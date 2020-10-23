using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.ViewModels.Doctor
{
    public class DoctorSpecializationViewModel
    {
        public class CreateDoctorSpecializationViewModel
        {
            public string Name { get; set; }
            public string DoctorId { get; set; }
        }

        public class EditDoctorSpecializationViewModel
        {
            [Required]
            public string Id { get; set; }
            public string Name { get; set; }

        }
    }
}
