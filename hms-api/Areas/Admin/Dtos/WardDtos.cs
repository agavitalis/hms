using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class WardDtoForCreate
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
    }

    public class WardDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
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
    }


}
