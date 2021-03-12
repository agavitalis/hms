using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionServiceRequestResultImage
    {
        public AdmissionServiceRequestResultImage()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Image { get; set; }

        public string ImageURL { get; set; }
        public string ServiceRequestResultId { get; set; }
        public AdmissionServiceRequestResult ServiceRequestResult { get; set; }
    }
}
