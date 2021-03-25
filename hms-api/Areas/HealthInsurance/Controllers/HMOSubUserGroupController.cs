using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Controllers
{

    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Sub User Groups")]
    [ApiController]
    public class HMOSubUserGroupController : Controller
    {
        private readonly IHMOSubUserGroup _HMOSubUserGroup;
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IHMOUserGroup _HMOUserGroup;
        private readonly IMapper _mapper;


        public HMOSubUserGroupController(IHMOSubUserGroup HMOSubUserGroup, IHMOUserGroup HMOUserGroup, IHMOHealthPlan HMOHealthPlan, IMapper mapper)
        {
            _HMOSubUserGroup = HMOSubUserGroup;
            _HMOUserGroup = HMOUserGroup;
            _HMOHealthPlan = HMOHealthPlan;
            _mapper = mapper;
        }

        [Route("GetHMOSubUserGroup")]
        [HttpGet]
        public async Task<IActionResult> GetHMOSubUserGroup(string HMOSubUserGroupId)
        {
            if (string.IsNullOrEmpty(HMOSubUserGroupId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HMOSubUserGroup = await _HMOSubUserGroup.GetHMOSubUserGroup(HMOSubUserGroupId);

            if (HMOSubUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOSubUserGroupId" });
            }
            return Ok(new
            {
                HMOSubUserGroup,
                message = "HMO Sub User Group Returned"
            });
        }

       


        [Route("GetHMOSubUserGroups")]
        [HttpGet]
        public async Task<IActionResult> GetHMOSubUserGroups([FromQuery] PaginationParameter paginationParameter, string HMOUserGroupId)
        {
            var HMOSubUserGroups = _HMOSubUserGroup.GetHMOSubUserGroups(paginationParameter, HMOUserGroupId);

            var paginationDetails = new
            {
                HMOSubUserGroups.TotalCount,
                HMOSubUserGroups.PageSize,
                HMOSubUserGroups.CurrentPage,
                HMOSubUserGroups.TotalPages,
                HMOSubUserGroups.HasNext,
                HMOSubUserGroups.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOSubUserGroups,
                paginationDetails,
                message = "HMO Sub User Groups Fetched"
            });
        }



        [HttpPost("CreatHMOSubUserGroup")]
        public async Task<IActionResult> CreatHMOSubUserGroup(HMOSubUserGroupDtoForCreate hMOSubUserGroup)
        {
            if (hMOSubUserGroup == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var HMOUserGroup = await _HMOUserGroup.GetHMOUserGroup(hMOSubUserGroup.HMOUserGroupId);

            if (HMOUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOUserGroupId" });
            }
            var HMOSubUserGroupToCreate = _mapper.Map<HMOSubUserGroup>(hMOSubUserGroup);

            var res = await _HMOSubUserGroup.CreateHMOUserGroup(HMOSubUserGroupToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO Sub User Group failed to create" });
            }

            return Ok(new
            {
                message = "HMO Sub User Group created successfully"
            });
        }

        [HttpPost("UpdateHMOSubUserGroup")]
        public async Task<IActionResult> UpdateHMOSubUserGroup(HMOSubUserGroupDtoForUpdate hMOSubUserGroup)
        {
            if (hMOSubUserGroup == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var HMOUserGroup = await _HMOUserGroup.GetHMOUserGroup(hMOSubUserGroup.HMOUserGroupId);

            if (HMOUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOUserGroupId" });
            }

            var HMOHealthPlan = await _HMOHealthPlan.GetHMOHealthPlan(hMOSubUserGroup.HMOHealthPlanId);

            if (HMOHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOHealthPlanId" });
            }
            
            var HMOSubUserGroupToUpdate = _mapper.Map<HMOSubUserGroup>(hMOSubUserGroup);

            var res = await _HMOSubUserGroup.UpdateHMOUserGroup(HMOSubUserGroupToUpdate);
            
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO Sub User Group failed to create" });
            }

            return Ok(new
            {
                message = "HMO Sub User Group created successfully"
            });
        }
    }
}
