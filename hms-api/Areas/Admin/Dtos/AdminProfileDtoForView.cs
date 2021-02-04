using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class AdminProfileDtoForView
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
