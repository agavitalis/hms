using System;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Accounts")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccount _accountRepo;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly ITransactionLog _transaction;

        public AccountController(IAccount account, IUser user, IMapper mapper, ITransactionLog transaction)
        {
            _accountRepo = account;
            _mapper = mapper;
            _user = user;
            _transaction = transaction;
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

        [HttpGet("GetAccountByAccountNumber/{AccountNumber}")]
        public async Task<IActionResult> GetAccountByAccountNumber(string AccountNumber)
        {
            if (AccountNumber == "")
            {
                return BadRequest();
            }

            var account = await _accountRepo.GetAccountByAccountNumber(AccountNumber);

            if (account == null)
            {
                return BadRequest(new { message = "An Account with this Account Number was not found" });
            }

            return Ok(new { account, mwessage = "Account returned" });
        }

        [HttpGet("Account/GetAllAccounts")]
        public async Task<IActionResult> AllAccounts()
        {
            var accounts = await _accountRepo.GetAllAccounts();

            
            return Ok(new { accounts, message = "Accounts Fetched" });
          
        }

        [HttpGet("Account/GetAccountTransactions")]
        public async Task<IActionResult> GetAccountTransactions(string AccountId)
        {
            var account = await _accountRepo.GetAccountByIdAsync(AccountId);

            if (account == null)
            {
                return BadRequest(new { message = "An Account with this Id was not found" });
            }
            var accountTransactions = await _transaction.GetAccountTransactions(AccountId);


            return Ok(new
            {
                accountTransactions,
                message = "Account Transactions"
            });
        }

        [HttpPost("Account/FundAccount", Name = "AdminFundAccount")]
        public async Task<IActionResult> FundAccount(AccountDtoForAdminFunding account)
        {
            string transactionType = "Credit";
            string invoiceType = null;
            string invoiceId = null;
            DateTime transactionDate = DateTime.Now;

            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var Account = await _accountRepo.GetAccountByIdAsync(account.AccountId);
            var user = await _user.GetUserByIdAsync(account.InitiatorId);
            if (Account == null || user == null)
            {
                return BadRequest(new { message = "An Account with this Id was not found" });
            }

            var accountToUpdate = _mapper.Map<Account>(Account);
            accountToUpdate.AccountBalance += account.Amount;
            var res = await _accountRepo.UpdateAccount(accountToUpdate);
      
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Fund Account" });
            }

            await _transaction.LogAccountTransactionAsync(account.Amount, transactionType, invoiceType, invoiceId, account.PaymentMethod, transactionDate, Account.Id, account.InitiatorId);
        
            return Ok(new
            {
                accountToUpdate,
                message = "Account Funded successfully"
            });
        }

        [HttpPost("Account/FundAccountByLink", Name = "FundAccountByLink")]
        public async Task<IActionResult> FundAccountByLink(AccountDtoForLinkFunding account)
        {
            string transactionType = "Credit";
            string invoiceType = null;
            string invoiceId = null;
            DateTime transactionDate = DateTime.Now;

            if (account == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var Account = await _accountRepo.GetAccountByAccountNumber(account.AccountNumber);
            //var user = await _user.GetUserByIdAsync(account.UserId);
            if (Account == null)
            {
                return BadRequest(new { message = "An Account With This Account Number Was Not Found" });
            }

            var accountToUpdate = _mapper.Map<Account>(Account);
            accountToUpdate.AccountBalance += account.Amount;
            var res = await _accountRepo.UpdateAccount(accountToUpdate);

            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Fund Account" });
            }

            await _transaction.LogLinkPaymentTransaction(account.Amount, transactionType, invoiceType, invoiceId, account.paymentDescription, transactionDate, Account.Id, account.Initiator);

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
