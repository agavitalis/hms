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
        Task<bool> GenerateInvoice(RegistrationInvoice invoice);
        Task<bool> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy);
        Task<object> GetPatientRegistrationInvoice(string patientId);
        Task<bool> PayRegistrationFee(DtoForPatientRegistrationPayment paymentDetails);
        Task<bool> UpdateRegistrationInvoice(RegistrationInvoice invoice, string description);
    }
}
