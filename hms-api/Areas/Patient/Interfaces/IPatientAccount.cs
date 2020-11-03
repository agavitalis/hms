using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Patient.Interfaces
{
    public interface IPatientAccount
    {
        Task<bool> UpdateAccount(Account account);
    }
}
