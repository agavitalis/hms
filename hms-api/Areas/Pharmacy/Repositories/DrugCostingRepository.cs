using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Database;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
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
                decimal totalUnitPrice = 0;
                decimal totalContainerPrice = 0;
                decimal totalCartonPrice = 0;
                decimal priceTotal = 0;
                decimal AmountToBePaidByPatient = 0;
                decimal AmountToBePaidByHMO = 0;
                string priceCalculationFormular = "";

                if (drugCosting == null)
                    return null;

                var PatientProfile = await _applicationDbContext.PatientProfiles.Where(p => p.PatientId == drugCosting.PatientId).Include(p => p.Account).ThenInclude(p => p.HealthPlan).FirstOrDefaultAsync();
                var healthplanId = PatientProfile.Account.HealthPlanId;
               
                List<object> drugPricing = new List<object>();


             

                //get the drug price based on the health plan above
                var HMOHealthPlanPatient = await _applicationDbContext.HMOHealthPlanPatients.Include(d => d.HMOHealthPlan).ThenInclude(d => d.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var HMOHealthPlanSubGroupPatient = await _applicationDbContext.HMOSubUserGroupPatients.Include(d => d.HMOSubUserGroup).ThenInclude(h => h.HMOHealthPlan).ThenInclude(d => d.HMO).Where(p => p.PatientId == PatientProfile.PatientId).FirstOrDefaultAsync();
                var NHISHealthPlanPatient = await _applicationDbContext.NHISHealthPlanPatients.Where(p => p.PatientId == PatientProfile.PatientId).Include(n => n.NHISHealthPlan).FirstOrDefaultAsync();




                decimal totalDrugPricing = 0;
                decimal amountDue = 0;
                decimal HMOAmount = 0;

                foreach (var _drug in drugCosting.Drugs)
                {
                    //get the drug price based on the health plan above
                    var drugPrice = await _applicationDbContext.DrugPrices.Where(p => p.HealthPlanId == healthplanId).FirstOrDefaultAsync();
                    var drug = _applicationDbContext.Drugs.Find(_drug.drugId);

                    if (HMOHealthPlanPatient != null)
                    {
                        var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanPatient.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanDrugPrice != null)
                        {
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                        }
                    }
                    else if (HMOHealthPlanSubGroupPatient != null)
                    {
                        var HMOHealthPlanDrugPrice = await _applicationDbContext.HMOHealthPlanDrugPrices.Where(p => p.HMOHealthPlanId == HMOHealthPlanSubGroupPatient.HMOSubUserGroup.HMOHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (HMOHealthPlanDrugPrice != null)
                        {
                            totalUnitPrice = HMOHealthPlanDrugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = HMOHealthPlanDrugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = HMOHealthPlanDrugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByHMO = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = HMOHealthPlanPatient.HMOHealthPlan.HMO.Name + " " + HMOHealthPlanPatient.HMOHealthPlan.Name;
                        }
                    }
                    else if (NHISHealthPlanPatient != null)
                    {
                        var NHISDrug = await _applicationDbContext.NHISHealthPlanDrugs.Where(p => p.NHISHealthPlanId == NHISHealthPlanPatient.NHISHealthPlanId && p.DrugId == drug.Id).FirstOrDefaultAsync();

                        if (NHISDrug != null)
                        {
                            totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = priceTotal * NHISHealthPlanPatient.NHISHealthPlan.Percentage / 100;
                            priceCalculationFormular = NHISHealthPlanPatient.NHISHealthPlan.HealthPlan.Name + " " + NHISHealthPlanPatient.NHISHealthPlan.Name;
                        }
                    }
                    else if (drugPrice != null)
                    {
                        if (drugPrice != null)
                        {
                            totalUnitPrice = drugPrice.PricePerUnit * _drug.numberOfUnits;
                            totalContainerPrice = drugPrice.PricePerContainer * _drug.numberOfContainers;
                            totalCartonPrice = drugPrice.PricePerCarton * _drug.numberOfCartons;
                            priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                            priceCalculationFormular = drugPrice.HealthPlan.Name;
                        }
                    }
                    else
                    {
                        totalUnitPrice = drug.DefaultPricePerUnit * _drug.numberOfUnits;
                        totalContainerPrice = drug.DefaultPricePerContainer * _drug.numberOfContainers;
                        totalCartonPrice = drug.DefaultPricePerCarton * _drug.numberOfCartons;
                        AmountToBePaidByPatient = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceTotal = totalCartonPrice + totalContainerPrice + totalUnitPrice;
                        priceCalculationFormular = "Default Price";
                    }

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
