using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            string invoiceType = "Account";
           
            DateTime transactionDate = DateTime.Now;
            var accountInvoiceToCreate = new AccountInvoice();

            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var patient = await _patientRepository.GetPatientByIdAsync(account.InitiatorId);

            if (patient == null)
            {
                return BadRequest(new { message = "A Patient with this Id was not found" });
            }

            var accountToUpdate = _mapper.Map<Account>(patient.Account);
            var previousAccountBalance = accountToUpdate.AccountBalance;
            accountToUpdate.AccountBalance += account.Amount;

            accountInvoiceToCreate = new AccountInvoice()
            {
                Amount = account.Amount,
                GeneratedBy = account.InitiatorId,
                PaymentMethod = account.PaymentMethod,
                TransactionReference = account.TransactionReference,
                AccountId = patient.Account.Id,
            };


            var accountInvoice = await _accountRepo.CreateAccountInvoice(accountInvoiceToCreate);

            if (accountInvoice == null)
            {
                return BadRequest(new { response = "301", message = "Failed To Generate Invoice For Transaction" });
            }

            var res = await _accountRepo.UpdateAccount(accountToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Fund Account" });
            }

            await _transaction.LogAccountTransactionAsync(account.Amount, transactionType, invoiceType, accountInvoiceToCreate.Id, account.PaymentMethod, transactionDate, patient.Account.Id, previousAccountBalance, account.InitiatorId);

            return Ok(new
            {
                accountToUpdate,
                message = "Account Funded successfully"
            });
        }

        [HttpGet("Account/GetAccountBalance")]
        public async Task<IActionResult> GetPatientAccountBalance(string PatientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(PatientId);

            if (patient == null)
            {
                return BadRequest(new { message = "A Patient with this Id was not found" });
            }

            var accountBalance = patient.Account.AccountBalance;

            return Ok(new
            {
                accountBalance,
                message = "Patient Account Balance"
            });
        }

        [HttpGet("Account/GetAccount")]
        public async Task<IActionResult> GetAccount(string PatientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(PatientId);

            if (patient == null)
            {
                return BadRequest(new { message = "A Patient with this Id was not found" });
            }

            var account = patient.Account;
           
            return Ok(new
            {
                account,
                message = "Patient Account"
            });
        }

        [HttpGet("Account/GetPatientAccountTransactions")]
        public async Task<IActionResult> GetPatientAccountTransactions([FromQuery] PaginationParameter paginationParameter, string PatientId)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(PatientId);

            if (patient == null)
            {
                return BadRequest(new { message = "A Patient with this Id was not found" });
            }
            var accountTransactions = _transaction.GetAccountTransactions(patient.AccountId, paginationParameter);

            var paginationDetails = new
            {
                accountTransactions.TotalCount,
                accountTransactions.PageSize,
                accountTransactions.CurrentPage,
                accountTransactions.TotalPages,
                accountTransactions.HasNext,
                accountTransactions.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));
            return Ok(new
            {
                accountTransactions,
                message = "Patient Account Transactions"
            });
        }
    }
}
