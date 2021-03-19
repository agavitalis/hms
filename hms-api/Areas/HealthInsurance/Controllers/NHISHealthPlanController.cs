using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage NHIS Health Plans")]
    [ApiController]
    public class NHISHealthPlanController : Controller
    {
        private readonly IHealthPlan _healthPlan;
        private readonly INHISHealthPlan _NHISHealthPlan;
        private readonly IMapper _mapper;


        public NHISHealthPlanController(INHISHealthPlan NHISHealthPlan, IMapper mapper, IHealthPlan healthPlan)
        {
            _NHISHealthPlan = NHISHealthPlan;
            _healthPlan = healthPlan;
            _mapper = mapper;
        }

        [Route("GetNHISHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetNHISHealthPlan(string NHISHealthPlanId)
        {
            if (string.IsNullOrEmpty(NHISHealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(NHISHealthPlanId);

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }

            return Ok(new
            {
                NHISHealthPlan,
                message = "NHIS Health Plan Returned"
            });
        }

        [Route("GetNHISHealthPlans")]
        [HttpGet]
        public async Task<IActionResult> GetNHISHealthPlans([FromQuery] PaginationParameter paginationParameter)
        {
            var NHISHealthPlans = _NHISHealthPlan.GetNHISHealthPlans(paginationParameter);

            var paginationDetails = new
            {
                NHISHealthPlans.TotalCount,
                NHISHealthPlans.PageSize,
                NHISHealthPlans.CurrentPage,
                NHISHealthPlans.TotalPages,
                NHISHealthPlans.HasNext,
                NHISHealthPlans.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                NHISHealthPlans,
                paginationDetails,
                message = "HMOs Fetched"
            });
        }


     
        [Route("CreateNHISHealthPlan")]
        [HttpPost]
        public async Task<IActionResult> CreateHealthPlan(NHISHealthPlanDtoForCreate nHISHealthPlan)
        {
            if (nHISHealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlan = await _healthPlan.GetHealthPlanByIdAsync(nHISHealthPlan.HealthPlanId);

            if (healthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }

            var NHISHealthPlanToCreate = _mapper.Map<NHISHealthPlan>(nHISHealthPlan);

            var res = await _NHISHealthPlan.CreateNHISHealthPlan(NHISHealthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "NHIS HealthPlan failed to create" });
            }

            return Ok(new
            {
                message = "NHIS HealthPlan created successfully"
            });
        }
    }
}
