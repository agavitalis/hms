using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Models
{
    public class File
    {
        public File()
        {
            DateCreated = DateTime.Now;
        }
        public int Id { get; set; }
        public string FileNumber { get; set; }
        public int AccountId { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public virtual Account Account { get; set; }
    }
}
