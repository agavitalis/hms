﻿using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class AccountDtosForView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual HealthPlan HealthPlan { get; set; }

    }

    public class AccountDtoForCreate
    {
        public string Name { get; set; }
        public int HealthPlanId { get; set; }
    }
    
    public class FileDtoForCreate
    {
        public string FileNumber { get; set; }
        public int AccountId { get; set; }
    }
}
