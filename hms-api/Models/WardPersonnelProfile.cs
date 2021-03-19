using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class WardPersonnelProfile
    {
        public WardPersonnelProfile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        //personal details
        public string FullName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }

        //contact details
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }


        /*------ relationships-------*/

        [ForeignKey("ApplicationUser")]
        public string WardPersonnelId { get; set; }
        public virtual ApplicationUser WardPersonnel { get; set; }
    }
}
