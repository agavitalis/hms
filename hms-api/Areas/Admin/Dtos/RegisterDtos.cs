using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
   
    public class DtoForFileCreation
    {
        public string AccountId { get; set; }
    }

    public class DtoForPatientRegistration
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string HealthPlanId { get; set; }

        //this is optional for personal accounts
        public string AccountId{ get; set; }
    }
}
