﻿using AutoMapper;
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
    public class DrugInvoicingRepository : IDrugInvoicing
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public DrugInvoicingRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }

        public async Task<bool> CheckIfDrugsExist(List<Drugs> drugs)
        {
            if (drugs == null)
                return false;

            var idNotInDrugs = drugs.Where(x => _applicationDbContext.Drugs.Any(y => y.Id == x.drugId));

            return idNotInDrugs.Any();
        }

        public async Task<string> GenerateDrugDispenseInvoice(DrugInvoicingDto drugInvoicing)
        {
            try
            {
                if (drugInvoicing == null)
                    return null;

                //ToDO::Check if the drug is in stock

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugInvoicing.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                decimal totalDrugPricing = 0;
                foreach (var _drug in drugInvoicing.Drugs)
                {
                    //get the drug price based on the health plan above
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();
                    var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                    decimal totalUnitPrice = 0;
                    decimal totalContainerPrice = 0;
                    decimal totalCartonPrice = 0;
                    decimal priceTotal = 0;
                    string priceCalculationFormular = "";

                    if (String.IsNullOrEmpty(_drug.numberOfUnits.ToString()))
                    {
                        _drug.numberOfUnits = 0;
                    }

                    if (String.IsNullOrEmpty(_drug.numberOfContainers.ToString()))
                    {
                        _drug.numberOfContainers = 0;
                    }

                    if (String.IsNullOrEmpty(_drug.numberOfCartons.ToString()))
                    {
                        _drug.numberOfCartons = 0;
                    }

                    if (drugPrice != null)
                    {
                        totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = drugPrice.HealthPlan.Name;
                    }
                    else
                    {

                        totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";

                    }

                    totalDrugPricing = totalDrugPricing + priceTotal;
                    
                }

                var drugDispensingInvoice = new DrugDispensingInvoice()
                {
                    AmountTotal = totalDrugPricing,
                    PaymentStatus = "NOT PAID",
                    GeneratedBy = drugInvoicing.GeneratedBy,
                    PatientId = drugInvoicing.PatientId,
                    ClerkingId = drugInvoicing.ClarkingId
                };

                await _applicationDbContext.DrugDispensingInvoices.AddAsync(drugDispensingInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return drugDispensingInvoice.Id;

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<object> CreateDespenseRequest(DrugInvoicingDto drugInvoicingDto, string invoiceId)
        {
            try
            {
                if (drugInvoicingDto == null || string.IsNullOrEmpty(invoiceId))
                    return false;

                //grab the patient complete details
                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugInvoicingDto.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                List<object> drugList = new List<object>();
                DrugDispensingInvoice drugInvoice = new DrugDispensingInvoice();

                foreach (var _drug in drugInvoicingDto.Drugs)
                {

                    //get the drug price based on the health plan above
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();
                    var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                    decimal totalUnitPrice = 0;
                    decimal totalContainerPrice = 0;
                    decimal totalCartonPrice = 0;
                    decimal priceTotal = 0;
                    string priceCalculationFormular = "";

                    if (drugPrice != null)
                    {
                        totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = drugPrice.HealthPlan.Name;
                    }
                    else
                    {

                        totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";

                    }


                    //save drugs to dispensing
                    DrugDispensing drugDispensing = new DrugDispensing
                    {
                        DrugId = _drug.drugId,
                        NumberOfCartons = _drug.numberOfCartons,
                        NumberOfContainers = _drug.numberOfContainers,
                        NumberOfUnits = _drug.numberOfUnits,

                        TotalCartonPrice = totalCartonPrice,
                        TotalContainerPrice = totalContainerPrice,
                        TotalUnitPrice = totalUnitPrice,
                        PriceTotal = priceTotal,

                        PriceCalculationFormular = priceCalculationFormular,

                        PaymentStatus = "Not Paid",
                        DrugDispensingInvoiceId = invoiceId,
                        ClerkingId = drugInvoicingDto.ClarkingId
                    };
                    await _applicationDbContext.DrugDispensings.AddAsync(drugDispensing );
                    await _applicationDbContext.SaveChangesAsync();

                    //add everything to a list
                    var drugsListed = new
                    {
                        drug = drugDispensing
                    };

                    drugList.Add(drugsListed);
                    drugInvoice = drugDispensing.DrugDispensingInvoice;
                }

                var drugInvoiceList= new
                {
                    drugs = drugList,
                    invoice = drugInvoice
                };

                return drugInvoiceList;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<IEnumerable<DrugDispensingInvoice>> GetDrugDispencingInvoices()
        {
          return  await _applicationDbContext.DrugDispensingInvoices.OrderByDescending(s=> s.DateGenerated)
                .Include(p=>p.Patient)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrugDispensingInvoice>> GetPatientDrugInvoices(string patientId)
        {
            return await _applicationDbContext.DrugDispensingInvoices.Where(p=>p.PatientId == patientId)
                .OrderByDescending(s => s.DateGenerated)
                .Include(d=>d.DrugDispensing)
                .ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetDrugsInAnInvoice(string invoiceNumber)
        {
            var drugsInInvoice = await _applicationDbContext.DrugDispensings.Where(s => s.DrugDispensingInvoice.InvoiceNumber == invoiceNumber)
                .ToListAsync();
            return drugsInInvoice;
        }

        public async Task<bool> CheckIfAmountPaidIsCorrect(string invoiceNumber, decimal amount)
        {
            if (string.IsNullOrEmpty(invoiceNumber))
                return false;

         
            var drugDispensed = _applicationDbContext.DrugDispensingInvoices.Where(i => i.InvoiceNumber == invoiceNumber).FirstOrDefault();
            if(drugDispensed != null)
            {
                //check if the ammount tallys
                if(drugDispensed.AmountTotal != amount)
                {
                    return false;
                         
                }

                return true;
            }
            else
            {
                return false;
            }   
                
           
        }

        public async Task<bool> PayForDrugs(DrugInvoicingPaymentDto drugPayment)
        {

            var drugInvoice = await _applicationDbContext.DrugDispensingInvoices.Where(i => i.InvoiceNumber == drugPayment.InvoiceNumber).FirstOrDefaultAsync();
            var drugsDispensed = await _applicationDbContext.DrugDispensings.Where(d => d.DrugDispensingInvoiceId == drugInvoice.Id).ToListAsync();

            //ToDO::Check if the drug is in stock and deduct

            //mark all drugs as paid
            foreach (var drugs in drugsDispensed)
            {
                var DrugPayment = await _applicationDbContext.DrugDispensings.FirstOrDefaultAsync(s => s.Id == drugs.Id);
                    DrugPayment.PaymentStatus = "PAID";
                    await  _applicationDbContext.SaveChangesAsync();
            }
           
            //mark the invoice imself as paid
            var DrugDispensingInvoice = await _applicationDbContext.DrugDispensingInvoices.FirstOrDefaultAsync(s => s.InvoiceNumber == drugPayment.InvoiceNumber);
            DrugDispensingInvoice.PaymentStatus = "PAID";
            DrugDispensingInvoice.ModeOfPayment = drugPayment.ModeOfPayment;
            DrugDispensingInvoice.ReferenceNumber = drugPayment.ReferenceNumber;
            DrugDispensingInvoice.Description = drugPayment.Description;
            DrugDispensingInvoice.DatePaid = DateTime.Now;
            DrugDispensingInvoice.PaidBy = drugPayment.PaidBy;
 
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
