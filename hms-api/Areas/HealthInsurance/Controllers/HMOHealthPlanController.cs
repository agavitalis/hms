using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HMS.Models;
using HMS.Areas.NHIS.Interfaces;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Services.Helpers;
using Newtonsoft.Json;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Health Plans")]
    [ApiController]
    public class HMOHealthPlanController : Controller
    {
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IHMO _HMO;
        private readonly IMapper _mapper;


        public HMOHealthPlanController(IHMOHealthPlan HMOHealthPlan, IMapper mapper, IHMO HMO)
        {
            _HMOHealthPlan = HMOHealthPlan;
            _HMO = HMO;
            _mapper = mapper;
        }

        [Route("GetHMOHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetHMOHealthPlan(string HMOHealthPlanId)
        {
            if (string.IsNullOrEmpty(HMOHealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HMOHealthPlan = await _HMOHealthPlan.GetHMOHealthPlan(HMOHealthPlanId);

            if (HMOHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }

            return Ok(new
            {
                HMOHealthPlan,
                message = "HMO Health Plan Returned"
            });
        }

        [Route("GetHMOHealthPlans")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs([FromQuery] PaginationParameter paginationParameter)
        {
            var HMOHealthPlans = _HMOHealthPlan.GetHMOHealthPlans(paginationParameter);

            var paginationDetails = new
            {
                HMOHealthPlans.TotalCount,
                HMOHealthPlans.PageSize,
                HMOHealthPlans.CurrentPage,
                HMOHealthPlans.TotalPages,
                HMOHealthPlans.HasNext,
                HMOHealthPlans.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOHealthPlans,
                paginationDetails,
                message = "HMOs Fetched"
            });
        }


        [HttpPost("CreateHealthPlan")]
        public async Task<IActionResult> CreateHealthPlan(HMOHealthPlanDtoForCreate HMOHealthPlan)
        {
            if (HMOHealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HMO = await _HMO.GetHMO(HMOHealthPlan.HMOId);
            
            if (HMO == null)
            {
                return BadRequest(new { message = "Invalid HMOId" });
            }

            var HMOHealthPlanToCreate = _mapper.Map<HMOHealthPlan>(HMOHealthPlan);

            var res = await _HMOHealthPlan.CreateHMOHealthPlan(HMOHealthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO HealthPlan failed to create" });
            }

            return Ok(new
            {
                message = "HMO HealthPlan created successfully"
            });
        }
    }
}

