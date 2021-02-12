using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Manage Health Plans")]
    [ApiController]
    public class HealthPlanController : ControllerBase
    {
        private readonly IHealthPlan _healthPlan;
        private readonly IMapper _mapper;
        private readonly IConsultation _consultation;


        public HealthPlanController(IHealthPlan healthPlan, IMapper mapper, IConsultation consultation)
        {
            _healthPlan = healthPlan;
            _mapper = mapper;
            _consultation = consultation;
        }


        [HttpPost("CreateHealthPlan")]
        public async Task<IActionResult> CreateHealthPlan(HealthPlanDtoForCreate healthPlan)
        {
            if(healthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanToCreate = _mapper.Map<HealthPlan>(healthPlan);

            var res = await _healthPlan.InsertHealthPlan(healthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan failed to create" });
            }

           
            return CreatedAtRoute("HealthPlan", healthPlan);
        }

        //[HttpGet("GetAllHealthPlans", Name = "HealthPlan")]
        //public async Task<IActionResult> AllHealthPlan()
        //{
        //    var plans = await _healthPlan.GetAllHealthPlan();
        //    return Ok(new { plans, message = "HealthPlans Fetched" });

        //}

        [HttpGet("GetAllHealthPlans", Name = "HealthPlan")]
        public async Task<IActionResult> AllHealthPlan([FromQuery] PaginationParameter paginationParameter)
        {
            var healthPlans = _healthPlan.GetHealthPlansPagnation(paginationParameter);

            var paginationDetails = new
            {
                healthPlans.TotalCount,
                healthPlans.PageSize,
                healthPlans.CurrentPage,
                healthPlans.TotalPages,
                healthPlans.HasNext,
                healthPlans.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                healthPlans,
                message = "Healthplans Fetched"
            });
        }


        [HttpGet("GetAHealthPlan/{Id}")]
        public async Task<IActionResult> GetHealthPlan(string Id)
        {
            if(Id == null)
            {
                return BadRequest();
            }

            var res = await _healthPlan.GetHealthPlanByIdAsync(Id);

            if(res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Health Plan returned" });
        }

        [HttpPost("UpdateHealthPlan")]
        public async Task<IActionResult> EditWard(HealthPlanDtoForUpdate healthplan)
        {
            if (healthplan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthplanToUpdate = _mapper.Map<HealthPlan>(healthplan);

            var res = await _healthPlan.UpdateHealthPlan(healthplanToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HealthPlan failed to update" });
            }

            return Ok(new
            {
                healthplan,
                message = "Health plan updated successfully"
            });
        }

        [HttpPost("DisableHealthPlan")]
        public async Task<IActionResult> DeleteWard(HealthPlanDtoForDelete HealthPlan)
        {
            if (HealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var healthPlan = await _healthPlan.GetHealthPlanByIdAsync(HealthPlan.Id);
            healthPlan.Status = false;
            
            var res = await _healthPlan.UpdateHealthPlan(healthPlan);

            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Disable Healthplan" });
            }

            return Ok(new { healthPlan, message = "Health Plan Disabled" });
        }

    }
}
