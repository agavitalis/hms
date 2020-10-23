using HMS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Database.Seeders
{
    interface IDBInitializer
    {
        public void RunMigration(ApplicationDbContext db);
    }
}
