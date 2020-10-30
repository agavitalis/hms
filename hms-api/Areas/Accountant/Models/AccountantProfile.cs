﻿using HMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace HMS.Areas.Accountant.Models
{
    public class AccountantProfile
    {
        public AccountantProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
        public string Address { get; set; }


        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string AccountantId { get; set; }
        public virtual ApplicationUser Accountant { get; set; }
    }
}
