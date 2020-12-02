﻿using System;
using System.ComponentModel.DataAnnotations;

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

        public string InvoiceGeneratedBy { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string AuthenticationToken { get; set; }
    }
}
