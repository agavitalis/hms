using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class ServiceCategoryDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ServiceCategoryDtoForCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
