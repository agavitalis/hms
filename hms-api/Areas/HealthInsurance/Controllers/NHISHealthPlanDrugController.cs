using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Pharmacy.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage NHIS Health Plan Drugs")]
    [ApiController]
    public class NHISHealthPlanDrugController : Controller
    {
        private readonly INHISHealthPlanDrug _healthPlanDrug;
        private readonly IMapper _mapper;
        private readonly IDrug _drug;
        private readonly INHISHealthPlan _NHISHealthPlan;

        public NHISHealthPlanDrugController(INHISHealthPlanDrug healthPlanDrug, INHISHealthPlan NHISHealthPlan, IDrug drug, IMapper mapper)
        {
            _NHISHealthPlan = NHISHealthPlan;
            _drug = drug;
            _healthPlanDrug = healthPlanDrug;
            _mapper = mapper;
        }


        [Route("GetHealthPlanDrug")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanDrug(string HealthPlanDrugId)
        {
            if (string.IsNullOrEmpty(HealthPlanDrugId))
            {
                return BadRequest();
            }

            var healthPlanDrug = await _healthPlanDrug.GetHealthPlanDrug(HealthPlanDrugId);

            if (healthPlanDrug == null)
            {
                return NotFound();
            }

            return Ok(new { healthPlanDrug, mwessage = "Health Plan Drug Returned" });
        }


        [Route("GetHealthPlanDrugs")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanDrugs()
        {
            var healthPlanDrugs = await _healthPlanDrug.GetHealthPlanDrugs();
            return Ok(new { healthPlanDrugs, message = "Health Plan Drugs Returned" });
        }

        [Route("GetHealthPlanDrugsByDrug")]
        [HttpGet]
        public async Task<IActionResult> GetDrugPricesByDrug(string DrugId)
        {
            var healthPlanDrugs = await _healthPlanDrug.GetHealthPlanDrugsByDrug(DrugId);
            return Ok(new { healthPlanDrugs, message = "Health Plan Drugs Returned" });
        }



        [Route("GetHealthPlanDrugsByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanDrugsByHealthPlan(string HealthPlanId, [FromQuery] PaginationParameter paginationParameter)
        {
            var healthPlanDrugs = _healthPlanDrug.GetHealthPlanDrugsByHealthPlan(HealthPlanId, paginationParameter);

            var paginationDetails = new
            {
                healthPlanDrugs.TotalCount,
                healthPlanDrugs.PageSize,
                healthPlanDrugs.CurrentPage,
                healthPlanDrugs.TotalPages,
                healthPlanDrugs.HasNext,
                healthPlanDrugs.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                healthPlanDrugs,
                paginationDetails,
                message = "Drugs Returned"
            });
        }

        [Route("CreateHealthPlanDrug")]
        [HttpPost]
        public async Task<IActionResult> CreateHealthPlanDrug(NHISHealthPlanDrugDtoForCreate HealthPlanDrug)
        {
            if (HealthPlanDrug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            if (string.IsNullOrEmpty(HealthPlanDrug.NHISHealthPlanId) || string.IsNullOrEmpty(HealthPlanDrug.DrugId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }

            var drug = await _drug.GetDrug(HealthPlanDrug.DrugId);
            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(HealthPlanDrug.NHISHealthPlanId);

            if (drug == null)
            {
                return BadRequest(new { response = "301", message = "Invalid DrugId" });
            }

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid NHIS Health Plan Id" });
            }
            var drugPriceToCreate = _mapper.Map<NHISHealthPlanDrug>(HealthPlanDrug);

            var res = await _healthPlanDrug.CreateHealthPlanDrug(drugPriceToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Drug Failed To Create" });
            }

            return Ok(new
            {
                HealthPlanDrug,
                message = "Health Plan Drug Created Successfully"
            });
        }


        [Route("UpdateHealthPlanDrug")]
        [HttpPost]
        public async Task<IActionResult> UpdateHealthPlanDrug(NHISHealthPlanDrugDtoForUpdate HealthPlanDrug)
        {
            if (HealthPlanDrug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var drug = await _drug.GetDrug(HealthPlanDrug.DrugId);
            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(HealthPlanDrug.NHISHealthPlanId);

            if (drug == null)
            {
                return BadRequest(new { response = "301", message = "Invalid DrugId" });
            }

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid NHIS Health Plan Id" });
            }
            var HealthPlanDrugToUpdate = _mapper.Map<NHISHealthPlanDrug>(HealthPlanDrug);

            var res = await _healthPlanDrug.UpdateHealthPlanDrug(HealthPlanDrugToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Drug Failed To Update" });
            }

            return Ok(new
            {
                HealthPlanDrug,
                message = "HealthPlan Drug Updated Successfully"
            });
        }



        [Route("DeleteHealthPlanDrug")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHealthPlanDrug(NHISHealthPlanDrugDtoForDelete HealthPlanDrug)
        {
            if (HealthPlanDrug == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanDrugToDelete = _mapper.Map<NHISHealthPlanDrug>(HealthPlanDrug);

            var res = await _healthPlanDrug.DeleteHealthPlanDrug(healthPlanDrugToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Drug failed to delete" });
            }

            return Ok(new { HealthPlanDrug, message = "Health Plan Drug Deleted" });
        }
    }
}
