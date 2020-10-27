using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Models;
using HMS.Models.Doctor;
using HMS.Models.Patient;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HMS.Areas.Admin.Interfaces
{
    public interface IAdmin
    {
        Task<dynamic> GetDoctorsPatientAppointment();
        Task<bool> BookAppointment(DoctorAppointment appointment);
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);
        Task<Models.Account> InsertAccount(Models.Account account);
        Task<IEnumerable<Models.Account>> GetAllAccounts(PaginationParameter paginationParam);
        Task<Models.Account> GetAccountById(int Id);
        Task<IEnumerable<PatientProfile>> GetPatientsInAccount(int acctId);
        Task<File> GenerateFileNumber(FileDtoForCreate fileToCreate);
        Task<bool> InsertPatient(PatientProfile patient);
        Task<(bool, string)> AddNewPatient();
    }
}
