using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin- Manage Accounts")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccount _accountRepo;
        private readonly IMapper _mapper;
        private readonly Patient.Interfaces.IPatientProfile _patientRepository;

        public AccountController(IAccount account, Patient.Interfaces.IPatientProfile patientRepository, IMapper mapper)
        {
            _accountRepo = account;
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        [HttpGet("GetAccount/{Id}")]
        public async Task<IActionResult> GetAccountById(string Id)
        {
            if (Id == "")
            {
                return BadRequest();
            }

            var res = await _accountRepo.GetAccountByIdAsync(Id);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Account returned" });
        }

        [HttpGet("Account/GetAllAccounts")]
        public async Task<IActionResult> AllAccounts()
        {
            var accounts = await _accountRepo.GetAllAccounts();

            
            return Ok(new { accounts, message = "Accounts Fetched" });
          
        }

        [HttpPost("Account/FundAccount", Name = "AdminFundAccount")]
        public async Task<IActionResult> FundAccount(AccountDtoForAdminFunding account)
        {
            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var Account = await _accountRepo.GetAccountByIdAsync(account.AccountId);
            
            if (Account == null)
            {
                return BadRequest(new { message = "An Account with this Id was not found" });
            }

            var accountToUpdate = _mapper.Map<Account>(Account);
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

        [HttpPost("Account/CreateAccount", Name = "Account")]
        public async Task<IActionResult> CreateAccount(AccountDtoForCreate account)
        {
            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var accountToCreate = _mapper.Map<Account>(account);

            var res = await _accountRepo.CreateAccount(accountToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Account failed to create" });
            }

            return Ok(new
            {
                account,
                message = "Account created successfully"
            });
        }

        [HttpPost("Account/UpdateAccount", Name = "updateAccount")]
        public async Task<IActionResult> EditAccount(AccountDtoForUpdate account)
        {
            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var accountToUpdate = _mapper.Map<Account>(account);

            var res = await _accountRepo.UpdateAccount(accountToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Account failed to update" });
            }

            return Ok(new
            {
                account,
                message = "Account updated successfully"
            });
        }

        [HttpPost("Account/DeleteAccount", Name = "deleteAccount")]
        public async Task<IActionResult> DeleteAccount(AccountDtoForDelete account)
        {
            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var accountToDelete = _mapper.Map<Account>(account);

            var res = await _accountRepo.DeleteAccount(accountToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Account failed to delete" });
            }

            return Ok(new { account, message = "Account Deleted" });
        }
    }

}
