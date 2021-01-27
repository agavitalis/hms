using System.Threading.Tasks;
using HMS.Areas.Accountant.Dtos;
using HMS.Areas.Accountant.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant", Name = "Accountant - Manage Reports")]
    [ApiController]
    public class ReportsController : Controller
    {
        private readonly IReports _reports;

        public ReportsController(IReports reports)
        {
            _reports = reports;
        }
        

        [Route("GetTransactions")]
        [HttpPost]
        public async Task<IActionResult> GetTransactions(TransactionsDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(Transactions.PaymentMethod))
            {
                var transactions = await _reports.GetTransactions(Transactions.StartDate, Transactions.EndDate);
                return Ok(new { transactions, message = "Report returned" });
            }
            else
            {
                var transactions = await _reports.GetTransactions(Transactions.StartDate, Transactions.EndDate, Transactions.PaymentMethod);
                if (transactions == null)
                {
                    return NotFound();
                }

                return Ok(new { transactions, message = "Report returned" });
            }
           
        }


        [Route("GetTransactionsForDrugs")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionsForDrugs(TransactionsDtoForView Transactions)
        {
           
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(Transactions.PaymentMethod))
            {
               var drugTransactions = await _reports.GetTransactionsForDrugs(Transactions.StartDate, Transactions.EndDate);
               return Ok(new { drugTransactions, message = "Report returned" });
            }
            else
            {
                var drugTransactions = await _reports.GetTransactionsForDrugs(Transactions.StartDate, Transactions.EndDate, Transactions.PaymentMethod);
                return Ok(new { drugTransactions, message = "Report returned" });
            }
           
        }

       

        [Route("GetTransactionsForServiceRequests")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionsForServiceRequests(TransactionsDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(Transactions.PaymentMethod))
            {
                var serviceRequestTransactions = await _reports.GetTransactionsForServiceRequests(Transactions.StartDate, Transactions.EndDate);
                return Ok(new { serviceRequestTransactions, message = "Report returned" });
            }
            else
            {
                var serviceRequestTransactions = await _reports.GetTransactionsForServiceRequests(Transactions.StartDate, Transactions.EndDate, Transactions.PaymentMethod);
                return Ok(new { serviceRequestTransactions, message = "Report returned" });
            }
           
        }

        

        [Route("GetTransactionsForRegistration")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionsForRegistration(TransactionsDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }
           
            if (string.IsNullOrEmpty(Transactions.PaymentMethod))
            {
                var registrationTransactions = await _reports.GetTransactionsForRegistration(Transactions.StartDate, Transactions.EndDate);
                return Ok(new { registrationTransactions, message = "Report returned" });
            }
            else
            {
                var registrationTransactions = await _reports.GetTransactionsForRegistration(Transactions.StartDate, Transactions.EndDate, Transactions.PaymentMethod);
                return Ok(new { registrationTransactions, message = "Report returned" });
            }
            
        }
    }
}
