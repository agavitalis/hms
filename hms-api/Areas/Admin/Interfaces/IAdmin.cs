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
    public interface IAdmin
    {
        Task<dynamic> GetDoctorsPatientAppointment();
        Task<bool> BookAppointment(DoctorAppointment appointment);
        Task<IEnumerable<ApplicationUser>> GetAllDoctors();
        Task<DoctorProfile> GetDoctorsById(string Id);
        Task<Models.Account> InsertAccount(Models.Account account);
        Task<IEnumerable<Models.Account>> GetAllAccounts(PaginationParameter paginationParam);
        Task<Models.Account> GetAccountById(string Id);
        Task<IEnumerable<PatientProfile>> GetPatientsInAccount(string acctId);
        Task<File> GenerateFileNumber(FileDtoForCreate fileToCreate);
        Task<bool> InsertPatient(PatientProfile patient);
        Task<(bool, string)> AddNewPatient();
    }
}
