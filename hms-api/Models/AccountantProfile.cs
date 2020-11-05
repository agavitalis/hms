﻿using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace HMS.Models
{
    public class AccountantProfile
    {
        public AccountantProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
        public string Address { get; set; }


        /*------ relationships-------*/
        public string AccountantId { get; set; }
        public virtual ApplicationUser Accountant { get; set; }
    }
}
