using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
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

        public AccountController(IAccount account, IPatientProfile patientRepository, IMapper mapper)
        {
            _accountRepo = account;
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        [HttpPost("Account/FundAccount", Name = "PatientFundAccount")]
        public async Task<IActionResult> FundAccount(AccountDtoForPatientFunding account)
        {
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

            return Ok(new
            {
                accountToUpdate,
                message = "Account Funded successfully"
            });
        }
    }
}
