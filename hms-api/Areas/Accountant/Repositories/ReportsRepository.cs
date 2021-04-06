using HMS.Areas.Accountant.Interfaces;
using HMS.Areas.Accountant.ViewModels;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Accountant.Repositories
{
    public class ReportsRepository : IReports
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHMO _HMO;
        private readonly IPatientProfile _patient;
        private readonly IDrug _drug;
        public ReportsRepository(ApplicationDbContext applicationDbContext, IHMO HMO, IPatientProfile patient, IDrug drug)
        {
            _applicationDbContext = applicationDbContext;
            _HMO = HMO;
            _patient = patient;
            _drug = drug;
            
        }
        
        public async Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate).OrderBy(t => t.InvoiceType).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactions(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.PaymentMethod == PaymentMethod).OrderBy(t => t.InvoiceType).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Drug").OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForDrugs(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Drug" && t.PaymentMethod == PaymentMethod).OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Service Request").OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForServiceRequests(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Service Request" && t.PaymentMethod == PaymentMethod).OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Registration").OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForRegistration(DateTime startDate, DateTime endDate, string PaymentMethod) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.Benefactor).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.InvoiceType == "Registration" && t.PaymentMethod == PaymentMethod).OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForAccounts(DateTime startDate, DateTime endDate) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.BenefactorAccount).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <=endDate && t.InvoiceType == "Account").OrderByDescending(t => t.TrasactionDate).ToListAsync();
        public async Task<IEnumerable<Transactions>> GetTransactionsForAccounts(DateTime startDate, DateTime endDate, string TransactionType) => await _applicationDbContext.Transactions.Include(t => t.Initiator).Include(t => t.BenefactorAccount).Where(t => t.TrasactionDate >= startDate && t.TrasactionDate <= endDate && t.TransactionType == TransactionType && t.InvoiceType == "Account").OrderByDescending(t => t.TrasactionDate).ToListAsync();

        public async Task<object> GetPatientInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            var drugInvoices = await _applicationDbContext.DrugDispensings.Include(d => d.Drug).Include(p => p.DrugDispensingInvoice).ThenInclude(p => p.Patient).Where(i => i.DrugDispensingInvoice.DateGenerated >= startDate && i.DrugDispensingInvoice.DateGenerated <= endDate && i.PriceCalculationFormular.Contains(HMO.Name)).ToListAsync();
            var serviceInvoices = await _applicationDbContext.ServiceRequests.Include(s => s.Service).Include(s => s.ServiceInvoice).ThenInclude(p => p.Patient).Where(i => i.ServiceInvoice.DateGenerated >= startDate && i.ServiceInvoice.DateGenerated <= endDate && i.ServiceInvoice.PriceCalculationFormular.Contains(HMO.Name)).ToListAsync();
            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();



            var HMOInvoices = new HMOInvoiceViewModel
            {
                DrugInvoices = drugInvoices,
                ServiceInvoices = serviceInvoices,
            };
            return HMOInvoices;
        }
        

        public async Task<object> GetPatientInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId, string PatientId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            var patient = await _patient.GetPatient(PatientId);
            var drugInvoices = await _applicationDbContext.DrugDispensings.Include(d => d.Drug).Include(p => p.DrugDispensingInvoice).ThenInclude(p => p.Patient).Where(i => i.DrugDispensingInvoice.DateGenerated >= startDate && i.DrugDispensingInvoice.DateGenerated <= endDate && i.PriceCalculationFormular.Contains(HMO.Name) && i.DrugDispensingInvoice.PatientId == PatientId).ToListAsync();
            var serviceInvoices = await _applicationDbContext.ServiceRequests.Include(s => s.Service).Include(s => s.ServiceInvoice).ThenInclude(p => p.Patient).Where(i => i.ServiceInvoice.DateGenerated >= startDate && i.ServiceInvoice.DateGenerated <= endDate && i.ServiceInvoice.PriceCalculationFormular.Contains(HMO.Name) && i.ServiceInvoice.PatientId == PatientId).ToListAsync();
            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();



            var HMOInvoices = new HMOInvoiceViewModel
            {
                DrugInvoices = drugInvoices,
                ServiceInvoices = serviceInvoices,
            };
            return HMOInvoices;
        }

        public async Task<object> GetDrugInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            var drugInvoices = await _applicationDbContext.DrugDispensings.Include(d => d.Drug).Include(p => p.DrugDispensingInvoice).ThenInclude(p => p.Patient).Where(i => i.DrugDispensingInvoice.DateGenerated >= startDate && i.DrugDispensingInvoice.DateGenerated <= endDate && i.PriceCalculationFormular.Contains(HMO.Name)).ToListAsync();
            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();
            return drugInvoices;
        }


        public async Task<object> GetDrugInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId, string DrugId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            
            var drugInvoices = await _applicationDbContext.DrugDispensings.Include(p => p.Drug).Include(p => p.DrugDispensingInvoice).ThenInclude(p => p.Patient).Where(i => i.DrugDispensingInvoice.DateGenerated >= startDate && i.DrugDispensingInvoice.DateGenerated <= endDate && i.PriceCalculationFormular.Contains(HMO.Name) && i.DrugId ==  DrugId).ToListAsync();

            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();
            
            return drugInvoices;
        }

        public async Task<object> GetServiceInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            var serviceInvoices = await _applicationDbContext.ServiceRequests.Include(s => s.Service).Include(p => p.ServiceInvoice).ThenInclude(p => p.Patient).Where(i => i.ServiceInvoice.DateGenerated >= startDate && i.ServiceInvoice.DateGenerated <= endDate && i.ServiceInvoice.PriceCalculationFormular.Contains(HMO.Name)).ToListAsync();
            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();
            return serviceInvoices;
        }


        public async Task<object> GetServiceInvoicesForHMO(DateTime startDate, DateTime endDate, string HMOId, string ServiceId)
        {
            var HMO = await _HMO.GetHMO(HMOId);
            var serviceInvoices = await _applicationDbContext.ServiceRequests.Include(p => p.Service).Include(p => p.ServiceInvoice).ThenInclude(p => p.Patient).Where(i => i.ServiceInvoice.DateGenerated >= startDate && i.ServiceInvoice.DateGenerated <= endDate && i.ServiceInvoice.PriceCalculationFormular.Contains(HMO.Name) && i.ServiceId == ServiceId).ToListAsync();

            //var admissionInvoices = await _applicationDbContext.AdmissionInvoices.Include(p => p.Admission.Patient).Where(i => i.PriceCalculationFormular.Contains("HMO")).ToListAsync();

            return serviceInvoices;
        }

    }
}
