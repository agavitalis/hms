using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO User Groups")]
    [ApiController]
    public class HMOUserGroupController : Controller
    {
        private readonly IHMOUserGroup _HMOUserGroup;
        private readonly IHMO _HMO;
        private readonly IMapper _mapper;


        public HMOUserGroupController(IHMOUserGroup HMOUserGroup, IHMO HMO, IMapper mapper)
        {
            _HMOUserGroup = HMOUserGroup;
            _HMO = HMO;
            _mapper = mapper;
        }

        [Route("GetHMOUserGroup")]
        [HttpGet]
        public async Task<IActionResult> GetHMOUserGroup(string HMOUserGroupId)
        {
            if (string.IsNullOrEmpty(HMOUserGroupId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HMOUserGroup = await _HMOUserGroup.GetHMOUserGroup(HMOUserGroupId);

            if (HMOUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOUserGroupId" });
            }

            return Ok(new
            {
                HMOUserGroup,
                message = "HMO User Group Returned"
            });
        }

        [Route("GetHMOUserGroups")]
        [HttpGet]
        public async Task<IActionResult> GetHMOUserGroups([FromQuery] PaginationParameter paginationParameter)
        {
            var HMOUserGroups = _HMOUserGroup.GetHMOUserGroups(paginationParameter);

            var paginationDetails = new
            {
                HMOUserGroups.TotalCount,
                HMOUserGroups.PageSize,
                HMOUserGroups.CurrentPage,
                HMOUserGroups.TotalPages,
                HMOUserGroups.HasNext,
                HMOUserGroups.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOUserGroups,
                paginationDetails,
                message = "HMO User Groups Fetched"
            });
        }


        [HttpPost("CreatHMOUserGroup")]
        public async Task<IActionResult> CreateWard(HMOUserGroupDtoForCreate hMOUserGroup)
        {
            if (hMOUserGroup == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var HMO = await _HMO.GetHMO(hMOUserGroup.HMOId);
           
            if (HMO == null)
            {
                return BadRequest(new { message = "Invalid HMOId" });
            }
            var HMOUserGroupToCreate = _mapper.Map<HMOUserGroup>(hMOUserGroup);

            var res = await _HMOUserGroup.CreateHMOUserGroup(HMOUserGroupToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO User Group failed to create" });
            }

            return Ok(new
            {
                message = "HMO User Group created successfully"
            });
        }
    }
}
