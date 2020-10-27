using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

    public class ServiceDtoForCreate
    {
        public string Name { get; set; }
        public string ServiceCategoryId { get; set; }
        public decimal Cost { get; set; }
    }
}
