using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Repositories
{
    public class AdmissionInvoiceRepository : IAdmissionInvoice
    {
        private readonly ApplicationDbContext _applicationDbContext;
        

        public AdmissionInvoiceRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }
        public async Task<string> CreateAdmissionInvoice(AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionInvoice == null)
                {
                    return "";
                }

                _applicationDbContext.AdmissionInvoices.Add(AdmissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return AdmissionInvoice.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AdmissionInvoice> GetAdmissionInvoice(string AdmissionInvoiceId) =>  await _applicationDbContext.AdmissionInvoices.Where(a => a.Id == AdmissionInvoiceId).FirstOrDefaultAsync();

        public async Task<AdmissionInvoice> GetAdmissionInvoiceByAdmissionId(string AdmissionId) => await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionId).FirstOrDefaultAsync();


        public async Task<string> UpdateAdmissionInvoice(AdmissionRequestDtoForCreate AdmissionRequest, AdmissionInvoice admissionInvoice)
        {
            try
            {
                decimal totalDrugPricing = 0;
                if (AdmissionRequest == null)
                    return null;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == admissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;

                List<Service> services = new List<Service>();
                if (AdmissionRequest.ServiceId != null)
                {
                    
                    foreach (var id in AdmissionRequest.ServiceId)
                    {
                        services.Add(_applicationDbContext.Services.Find(id));
                    }
                }
                



                if (AdmissionRequest.Drugs != null)
                {
                    foreach (var _drug in AdmissionRequest.Drugs)
                    {
                        //Check if the drug is in stock
                        var drug = _applicationDbContext.Drugs.Find(_drug.drugId);
                        if (drug.QuantityInStock < _drug.numberOfCartons * drug.ContainersPerCarton * drug.QuantityPerContainer + _drug.numberOfContainers * drug.QuantityPerContainer + _drug.numberOfUnits)
                        {
                            return "1";
                        }

                        //get the drug price based on the health plan above
                        var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();

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

                        totalDrugPricing += priceTotal;

                    }


                }

                if (AdmissionRequest.ServiceId != null && AdmissionRequest.Drugs != null)
                {
                    admissionInvoice.Amount = admissionInvoice.Amount + services.Sum(x => x.Cost) + totalDrugPricing;
                }
                else if (AdmissionRequest.Drugs != null)
                {
                    admissionInvoice.Amount += services.Sum(x => x.Cost);
                }
                else if (true)
                {
                    admissionInvoice.Amount += totalDrugPricing;
                }

                admissionInvoice.PaymentStatus = "NOT PAID";
                


                _applicationDbContext.AdmissionInvoices.Update(admissionInvoice);
                await _applicationDbContext.SaveChangesAsync();

                return admissionInvoice.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
