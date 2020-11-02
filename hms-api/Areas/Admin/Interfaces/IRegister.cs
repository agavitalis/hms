using HMS.Areas.Admin.Dtos;
using HMS.Services.Helpers;
using System;
using HMS.Models;


using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IRegister
    { 
        Task<File> CreateFile(string AccountId);
        Task<bool> RegisterPatient(ApplicationUser patient, File file, Account account);
     
    }
}
