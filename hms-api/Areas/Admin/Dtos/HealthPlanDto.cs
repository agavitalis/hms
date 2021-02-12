using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class HealthPlanDtoForCreate
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Renewal { get; set; }
        public int NoOfPatients { get; set; }
        public int NoOfAccounts { get; set; }
        public bool InstantBilling { get; set; }

    }

    public class HealthPlanDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Renewal { get; set; }
        public int NoOfPatients { get; set; }
        public int NoOfAccounts { get; set; }
        public bool InstantBilling { get; set; }

    }

    public class HealthPlanDtoForDelete
    {
        public string Id { get; set; }
    }

    public class HealthPlanDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
       
        public decimal Cost { get; set; }
    
        public decimal Renewal { get; set; }

        public int NoOfPatients { get; set; }
        public int NoOfAccounts { get; set; }

        public bool InstantBilling { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public bool Status { get; set; }
    }
}
