using HMS.Areas.Admin.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface IRegistrationInvoice
    {
        Task<object> GetPatientRegistrationInvoice(string patientId);
        Task<bool> GenerateRegistrationInvoice(decimal amount, string healthPlanId, string generatedBy, string patientId);
        Task<bool> PayRegistrationFee(DtoForPatientRegistrationPayment paymentDetails);
        Task<bool> UpdateRegistrationInvoice(RegistrationInvoice invoice, string description);
    }
}
