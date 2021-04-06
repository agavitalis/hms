using System;
using System.Threading.Tasks;
using HMS.Areas.Accountant.Dtos;
using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Accountant.Controllers
{
    [Route("api/Accountant", Name = "Accountant - Manage Reports")]
    [ApiController]
    public class ReportsController : Controller
    {
        private readonly IReports _reports;
        private readonly IDrug _drug;
        public ReportsController(IReports reports, IDrug drug)
        {
            _reports = reports;
            _drug = drug;
        }
        

        [Route("GetTransactions")]
        [HttpPost]
        public async Task<IActionResult> GetTransactions(TransactionsDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (Transactions.PaymentMethod.ToLower() == "all")
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

        [Route("GetTransactionsForAccount")]
        [HttpPost]
        public async Task<IActionResult> GetTransactionsForAccount(TransactionTypeDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (Transactions.TransactionType.ToLower() == "all")
            {
                var transactions = await _reports.GetTransactionsForAccounts(Transactions.StartDate, Transactions.EndDate);
                return Ok(new { transactions, message = "Report returned" });
            }
            else
            {
                var transactions = await _reports.GetTransactionsForAccounts(Transactions.StartDate, Transactions.EndDate, Transactions.TransactionType);
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

            if (Transactions.PaymentMethod.ToLower() == "all")
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

            if (Transactions.PaymentMethod.ToLower() == "all")
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
           
            if (Transactions.PaymentMethod.ToLower() == "all")
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

        [Route("GetPatientInvoicesForHMO")]
        [HttpPost]
        public async Task<IActionResult> GetPatientInvoicesForHMO(PatientHMOInvoiceDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (Transactions.PatientId.ToLower() == "all")
            {
                var patientInvoices = await _reports.GetPatientInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId);
                return Ok(new { patientInvoices, message = "Report returned" });
            }
            else
            {
                var patientInvoices = await _reports.GetPatientInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId, Transactions.PatientId);
                return Ok(new { patientInvoices, message = "Report returned" });
            }

        }

        [Route("GetDrugInvoicesForHMO")]
        [HttpPost]
        public async Task<IActionResult> GetDrugInvoicesForHMO(DrugHMOInvoiceDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (Transactions.DrugId.ToLower() == "all")
            {
                var drugInvoices = await _reports.GetDrugInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId);
                return Ok(new { drugInvoices, message = "Report returned" });
            }
            else
            {
                var drugInvoices = await _reports.GetDrugInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId, Transactions.DrugId);
                return Ok(new { drugInvoices, message = "Report returned" });
            }

        }

        [Route("GetServiceInvoicesForHMO")]
        [HttpPost]
        public async Task<IActionResult> GetServiceInvoicesForHMO(ServiceHMOInvoiceDtoForView Transactions)
        {
            if (Transactions == null)
            {
                return BadRequest();
            }

            if (Transactions.ServiceId.ToLower() == "all")
            {
                var serviceInvoices = await _reports.GetServiceInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId);
                return Ok(new { serviceInvoices, message = "Report returned" });
            }
            else
            {
                var serviceInvoices = await _reports.GetServiceInvoicesForHMO(Transactions.StartDate, Transactions.EndDate, Transactions.HMOId, Transactions.ServiceId);
                return Ok(new { serviceInvoices, message = "Report returned" });
            }

        }

        [Route("GetReportForExpiryDateofDrugs")]
        [HttpPost]
        public async Task<IActionResult> GetReportForExpiredDrugs(DateTime date)
        {
            var expiredDrugs = await _drug.GetExpiredDrugs(date);
            return Ok(new { expiredDrugs, message = "Report returned" });
        }
    }
}
