using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class AdmissionServiceRequestResult
    {
        public AdmissionServiceRequestResult()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Result { get; set; }
        public string AdditionalComments { get; set; }
        public string ServiceRequestId { get; set; }
        public virtual AdmissionServiceRequest ServiceRequest { get; set; }
        public virtual ICollection<AdmissionServiceRequestResultImage> ServiceRequestResultImages { get; set; }
    }
}
