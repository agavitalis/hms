using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Services.Helpers;
using System;
using HMS.Areas.Doctor.Models;
using HMS.Models;


using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Areas.Patient.Models;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IRegister
    {
<<<<<<< HEAD:hms-api/Areas/Admin/Interfaces/IRegister.cs
     
     
=======
>>>>>>> e74b62fbd014d6469c1e357f886da376742c95c6:hms-api/Areas/Admin/Interfaces/IAdmin.cs
        Task<Models.Account> InsertAccount(Models.Account account);
        Task<IEnumerable<Models.Account>> GetAllAccounts(PaginationParameter paginationParam);
        Task<Models.Account> GetAccountById(string Id);
        Task<IEnumerable<PatientProfile>> GetPatientsInAccount(string acctId);
        Task<File> GenerateFileNumber(FileDtoForCreate fileToCreate);
        Task<bool> InsertPatient(PatientProfile patient);
        Task<(bool, string)> AddNewPatient();
    }
}
