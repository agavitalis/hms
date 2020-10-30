using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
 

    public class AccountDtoForCreate
    {
        public string Name { get; set; }
        public string HealthPlanId { get; set; }
    }

    public class AccountDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public HealthPlan HealthPlan { get; set; }
    }

    public class AccountDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string  HealthPlanId { get; set; }
        public bool IsActive { get; set; }

    }

    public class AccountDtoForDelete
    {
        public string Id { get; set; }

    }

}
