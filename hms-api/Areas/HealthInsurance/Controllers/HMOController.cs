using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HMS.Models;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using AutoMapper;
using HMS.Services.Helpers;
using Newtonsoft.Json;
using HMS.Areas.Admin.Interfaces;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMOS")]
    [ApiController]
    public class HMOController : Controller
    {
        private readonly IHMO _HMO;
        private readonly IMapper _mapper;
        private readonly IHealthPlan _healthPlan;


        public HMOController(IHMO HMO, IHealthPlan healthPlan, IMapper mapper)
        {
            _HMO = HMO;
            _healthPlan = healthPlan;
            _mapper = mapper;
        }

        [Route("GetHMO")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs(string HMOId)
        {
            if (string.IsNullOrEmpty(HMOId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HMO = await _HMO.GetHMO(HMOId);

            if (HMO == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOId" });
            }
            return Ok(new
            {
                HMO,
                message = "HMO Fetched"
            });
        }

        [Route("GetHMOs")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs([FromQuery] PaginationParameter paginationParameter)
        {
            var HMOs = _HMO.GetHMOs(paginationParameter);

            var paginationDetails = new
            {
                HMOs.TotalCount,
                HMOs.PageSize,
                HMOs.CurrentPage,
                HMOs.TotalPages,
                HMOs.HasNext,
                HMOs.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOs,
                paginationDetails,
                message = "HMOs Fetched"
            });
        }
           

        [HttpPost("CreatHMO")]
        public async Task<IActionResult> CreateWard(HMODtoForCreate HMO)
        {
            if (HMO == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlan = await _healthPlan.GetHealthPlanByIdAsync(HMO.HealthPlanId);
            
            if (healthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }
            
            var HMOToCreate = _mapper.Map<HMO>(HMO);

            var res = await _HMO.CreateHMO(HMOToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO failed to create" });
            }

            return Ok(new
            {
                message = "HMO created successfully"
            });
        } 
    }
}
