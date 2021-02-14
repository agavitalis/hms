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
    public class AdmissionRequestRepository : IAdmissionRequest
    {
        private readonly ApplicationDbContext _applicationDbContext;
     
        public AdmissionRequestRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
    
        }

        public async Task<bool> CreateAdmissionRequest(AdmissionRequest AdmissionRequest)
        {
            try
            {
                if (AdmissionRequest == null)
                {
                    return false;
                }

                _applicationDbContext.AdmissionRequests.Add(AdmissionRequest);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAdmissionRequest(AdmissionRequestDtoForCreate AdmissionRequest, AdmissionInvoice AdmissionInvoice)
        {
            try
            {
                if (AdmissionRequest == null)
                    return false;




                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == AdmissionInvoice.Admission.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
                var admissionInvoice = await _applicationDbContext.AdmissionInvoices.Where(a => a.AdmissionId == AdmissionRequest.AdmissionId).FirstOrDefaultAsync();
                if (AdmissionRequest.ServiceId != null)
                {
                    AdmissionRequest.ServiceId.ForEach(x =>

                   _applicationDbContext.AdmissionRequests.AddAsync(
                       new AdmissionRequest
                       {
                           ServiceId = x,
                           ServiceAmount = _applicationDbContext.Services.Where(s => s.Id == x).FirstOrDefault().Cost,
                           PaymentStatus = "False",
                           AdmissionInvoiceId = admissionInvoice.Id
                       })
                  );
                }

                if (AdmissionRequest.Drugs != null)
                {
                    foreach (var _drug in AdmissionRequest.Drugs)
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
                        AdmissionRequest admissionRequest = new AdmissionRequest
                        {
                            DrugId = _drug.drugId,
                            NumberOfCartons = _drug.numberOfCartons,
                            NumberOfContainers = _drug.numberOfContainers,
                            NumberOfUnits = _drug.numberOfUnits,

                            TotalCartonPrice = totalCartonPrice,
                            TotalContainerPrice = totalContainerPrice,
                            TotalUnitPrice = totalUnitPrice,
                            DrugPriceTotal = priceTotal,

                            DrugPriceCalculationFormular = priceCalculationFormular,

                            PaymentStatus = "Not Paid",
                            AdmissionInvoiceId = admissionInvoice.Id,
                        };
                        await _applicationDbContext.AdmissionRequests.AddAsync(admissionRequest);

                    }
                }

                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
