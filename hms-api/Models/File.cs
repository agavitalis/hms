using HMS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Models
{
    public class File
    {

        
        public File()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;

        }

        public string Id { get; set; }
        public string FileNumber { get; set; }
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }

        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }

        
    }
}
