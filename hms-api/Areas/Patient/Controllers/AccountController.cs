using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Patient.Controllers
{
    [Route("api/Patient", Name = "Patient - Manage Accounts")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccount _accountRepo;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patientRepository;
        private readonly ITransactionLog _transaction;

        public AccountController(IAccount account, IPatientProfile patientRepository, IMapper mapper, ITransactionLog transaction)
        {
            _accountRepo = account;
            _mapper = mapper;
            _patientRepository = patientRepository;
            _transaction = transaction;
        }

        [HttpPost("Account/FundAccount", Name = "PatientFundAccount")]
        public async Task<IActionResult> FundAccount(AccountDtoForPatientFunding account)
        {
            string transactionType = "Credit";
            string invoiceType = null;
            string invoiceId = null;
            DateTime transactionDate = DateTime.Now;

            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var patient = await _patientRepository.GetPatientByIdAsync(account.PatientId);

            if (patient == null)
            {
                return BadRequest(new { message = "A Patient with this Id was not found" });
            }

            var accountToUpdate = _mapper.Map<Account>(patient.Account);
            accountToUpdate.AccountBalance += account.Amount;
            var res = await _accountRepo.UpdateAccount(accountToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Fund Accoint" });
            }

            await _transaction.LogTransaction(account.Amount, transactionType, invoiceType, invoiceId, account.paymentDescription, transactionDate);

            return Ok(new
            {
                accountToUpdate,
                message = "Account Funded successfully"
            });
        }
    }
}
