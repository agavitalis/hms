using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Dtos
{
    public class WardDtoForCreate
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string ChargePerNight { get; set; }
    }


    public class WardDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public decimal ChargePerNight { get; set; }
    }

    public class WardDtoForDelete
    {
        public string Id { get; set; }
       
    }

    public class WardDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string ChargePerNight { get; set; }
    }

    public class BedDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WardId { get; set; }
        public bool IsAvailable { get; set; }
        public Ward Ward { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class BedDtoForCreate
    {
        public string Name { get; set; }
        public string WardId { get; set; }
    }

}
