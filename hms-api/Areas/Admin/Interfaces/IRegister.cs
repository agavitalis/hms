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
        Task<ApplicationUser> RegisterPatient(ApplicationUser patient, File file, Account account);
        Task<object> GetPatientRegistrationInvoice(string patientId);
        Task<bool> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy, string patientId);
        Task<int> PayRegistrationFee(PatientRegistrationPaymentDto paymentDetails);
    }
}
