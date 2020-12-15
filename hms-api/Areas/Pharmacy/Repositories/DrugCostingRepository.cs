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
    public class DrugCostingRepository : IDrugCosting
    {
        private readonly ApplicationDbContext _applicationDbContext;
       

        public DrugCostingRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
           
        }

       

        public async Task<bool> CheckIfDrugsExist(List<Drugs> drugs)
        {
           if (drugs == null)
                return false;

            var idNotInDrugs = drugs.Where(x => _applicationDbContext.Drugs.Any(y => y.Id == x.drugId));

            return idNotInDrugs.Any();
        }

        public async Task <object> CostDrugs(DrugCostingDto drugCosting)
        {
            try
            {
                if (drugCosting == null)
                    return null;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugCosting.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
               
                List<object> drugPricing = new List<object>();
                foreach (var _drug in drugCosting.Drugs)
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
                    var price = new
                    {
                        drugName = drug.Name,
                        drugMeasurement = drug.Measurment,
                        drugManufacturer = drug.Manufacturer,
                        drugType = drug.DrugType,
                        drugGenericName = drug.GenericName,

                        numberOfCartons = _drug.numberOfCartons,
                        numberOfContainers = _drug.numberOfContainers,
                        numberOfUnits = _drug.numberOfUnits,

                        totalUnitPrice = totalUnitPrice,
                        totalContainerPrice =totalContainerPrice,
                        totalCartonPrice = totalCartonPrice,

                        priceTotal = priceTotal,
                        priceCalculationFormular = priceCalculationFormular

                    };
                   

                    drugPricing.Add(price);
                }

                return drugPricing;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

     
    }
}
