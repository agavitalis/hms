using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class ServiceRequestResultImage
    {
        public ServiceRequestResultImage()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Image { get; set; }
        public string ServiceRequestResultId { get; set; }
        public ServiceRequestResult ServiceRequestResult { get; set; }
    }
}
