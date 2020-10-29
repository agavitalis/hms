using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class HealthPlanController : ControllerBase
    {
        private readonly IHealthPlan _healthPlan;
        private readonly IMapper _mapper;
        private readonly IRegister _adminRepo;

        public HealthPlanController(IHealthPlan healthPlan, IMapper mapper, IRegister adminRepo)
        {
            _healthPlan = healthPlan;
            _mapper = mapper;
            _adminRepo = adminRepo;
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

        [HttpGet("GetAllHealthPlans", Name = "HealthPlan")]
        public async Task<IActionResult> AllHealthPlan()
        {
            var plans = await _healthPlan.GetAllHealthPlan();

            if (plans.Any())
                return Ok(new { plans, message = "HealthPlans Fetched" });
            else
                return NoContent();
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

        
    }
}
