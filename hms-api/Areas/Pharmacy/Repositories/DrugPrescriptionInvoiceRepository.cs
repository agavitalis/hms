using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Repositories
{
    public class DrugPrescriptionRepository : IDrugPrescriptionInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DrugPrescriptionRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }

        public async Task<bool> CheckIfAmountPaidIsCorrect(DrugPrescriptionPaymentDto drugPrescription)
        {
            if (drugPrescription == null)
                return false;

            decimal amountTotal = 0;
            //check if the amount tallies
            drugPrescription.DrugPrescriptionId.ForEach(drugPrescriptionId =>
            {
                var drugPrescription = _applicationDbContext.DrugPrescriptions.Where(i => i.Id == drugPrescriptionId).FirstOrDefault();
                amountTotal = amountTotal + drugPrescription.Amount;

            });

            if (amountTotal != drugPrescription.TotalAmount)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckIfDrugPrescriptionIdExists(List<string> drugPrescriptionId)
        {
            if (drugPrescriptionId == null)
                return false;

            var idNotInDrugPrescriptions = drugPrescriptionId.Where(x => _applicationDbContext.DrugPrescriptions.Any(y => y.Id == x));
            return idNotInDrugPrescriptions.Any();
        }

        public async Task<bool> CheckIfDrugsExist(List<string> drugIds)
        {
           if (drugIds == null)
                return false;

            var idNotInDrugs = drugIds.Where(x => _applicationDbContext.Drugs.Any(y => y.Id == x));

            return idNotInDrugs.Any();
        }

        //public async Task<bool> CreateDrugPrescription(DrugPrescriptionInvoiceDtoForCreate drugPrescription, string invoiceId)
        //{
        //    try
        //    {
        //        if (drugPrescription == null || string.IsNullOrEmpty(invoiceId))
        //            return false;
        //        if (drugPrescription.IdType.ToLower() == "appointment")
        //        {
        //            drugPrescription.DrugId.ForEach(x =>
        //            _applicationDbContext.DrugPrescriptions.AddAsync(
        //               new DrugPrescription
        //               {
        //                   DrugId = x,
        //                   Amount = _applicationDbContext.Drugs.Where(s => s.Id == x).FirstOrDefault().Cost,
        //                   PaymentStatus = "False",
        //                   DrugPrescriptionInvoiceId = invoiceId,
        //                   AppointmentId = drugPrescription.Id
        //               })
        //            );
        //        }
        //        else if (drugPrescription.IdType.ToLower() == "consultation")
        //        {
        //            drugPrescription.DrugId.ForEach(x =>
        //            _applicationDbContext.DrugPrescriptions.AddAsync(
        //                new DrugPrescription
        //                {
        //                    DrugId = x,
        //                    Amount = _applicationDbContext.Drugs.Where(s => s.Id == x).FirstOrDefault().Cost,
        //                    PaymentStatus = "False",
        //                    DrugPrescriptionInvoiceId = invoiceId,
        //                    ConsultationId = drugPrescription.Id
        //                })
        //           );
        //        }
        //        else
        //        {
        //            drugPrescription.DrugId.ForEach(x =>
        //           _applicationDbContext.DrugPrescriptions.AddAsync(
        //               new DrugPrescription
        //               {
        //                   DrugId = x,
        //                   Amount = _applicationDbContext.Drugs.Where(s => s.Id == x).FirstOrDefault().Cost,
        //                   PaymentStatus = "False",
        //                   DrugPrescriptionInvoiceId = invoiceId
        //               })
        //          );
        //        }

        //        await _applicationDbContext.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<string> GenerateInvoiceForDrugPrescription(DrugPrescriptionInvoiceDtoForCreate drugPrescription)
        //{
        //    try
        //    {
        //        if (drugPrescription == null)
        //            return null;
                
        //        var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId ==drugPrescription.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
        //        var healthplanId = PatientProfile.Account.HealthPlanId;
        //        var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();

        //        List<Drug> drugs = new List<Drug>();
        //        foreach (var id in drugPrescription.DrugId)
        //        {
        //            drugs.Add(_applicationDbContext.Drugs.Find(id));
        //        }

        //        var drugPrescriptionInvoice = new DrugPrescriptionInvoice()
        //        {
        //            AmountTotal = drugs.Sum(x => x.Cost),
        //            Description = drugPrescription.Description,
        //            PaymentStatus = "NOT PAID",
        //            GeneratedBy = drugPrescription.GeneratedBy,
        //            ClerkingId = drugPrescription.PatientId
        //        };

        //        await _applicationDbContext.DrugPrescriptionInvoices.AddAsync(drugPrescriptionInvoice);
        //        await _applicationDbContext.SaveChangesAsync();

        //        return drugPrescriptionInvoice.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public async Task<DrugPrescription> GetDrugPrescription(string drugPrescriptionId) => await _applicationDbContext.DrugPrescriptions.Where(s => s.Id == drugPrescriptionId).Include(s => s.DrugPrescriptionInvoice).Include(s => s.Drug).FirstOrDefaultAsync();
        
        public async Task<IEnumerable<DrugPrescriptionInvoice>> GetDrugPrescriptionInvoices() => await _applicationDbContext.DrugPrescriptionInvoices.ToListAsync();

        public Task<IEnumerable<DrugPrescriptionInvoiceDtoForView>> GetDrugPrescriptionInvoicesForPatient(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetDrugPrescriptionsByInvoice(string invoiceId)
        {
            var drugPrescription = await _applicationDbContext.DrugPrescriptions.Where(s => s.DrugPrescriptionInvoiceId == invoiceId).ToListAsync();
            return drugPrescription;
        }

        public async Task<bool> PayForDrugs(DrugPrescriptionPaymentDto drugPrescription)
        {
            int drugsPaid = 0;
            string drugPrescriptionInvoiceId = "";
            drugPrescription.DrugPrescriptionId.ForEach(drugPrescriptionId =>
            {
                var DrugPrescription = _applicationDbContext.DrugPrescriptions.FirstOrDefault(s => s.Id == drugPrescriptionId);
                DrugPrescription.PaymentStatus = "PAID";
                drugPrescriptionId = DrugPrescription.DrugPrescriptionInvoiceId;

                drugsPaid++;
            });

            _applicationDbContext.SaveChanges();

            //now check of all the servies in this invoice was paid for
            var drugCount = await _applicationDbContext.DrugPrescriptions.Where(s => s.DrugPrescriptionInvoiceId == drugPrescriptionInvoiceId).CountAsync();

            var DrugPrescriptionInvoice = await _applicationDbContext.DrugPrescriptionInvoices.FirstOrDefaultAsync(s => s.Id == drugPrescriptionInvoiceId);

            if (drugCount == drugsPaid)
                DrugPrescriptionInvoice.PaymentStatus = "PAID";
            else
                DrugPrescriptionInvoice.PaymentStatus = "INCOMPLETE";

            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
