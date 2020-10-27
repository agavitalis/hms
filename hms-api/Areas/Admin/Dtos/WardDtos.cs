using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class WardDtoForCreate
    {
        public string Name { get; set; }
        public decimal Capacity { get; set; }
    }

    public class WardDtoForView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
    }
}
