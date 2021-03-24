using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Sub User Group Health Plans")]
    [ApiController]
    public class HMOSubGroupHeathPlanController : Controller
    {
        private readonly IHMOSubUserGroupHealthPlan _HMOSubGroupHealthPlan;
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IHMOSubUserGroup _HMOSubUserGroup;
        private readonly IMapper _mapper;

        public HMOSubGroupHeathPlanController(IHMOSubUserGroupHealthPlan HMOSubGroupHealthPlan, IHMOHealthPlan HMOHealthPlan, IHMOSubUserGroup HMOSubUserGroup, IMapper mapper)
        {
            _HMOSubGroupHealthPlan = HMOSubGroupHealthPlan;
            _HMOHealthPlan = HMOHealthPlan;
            _HMOSubUserGroup = HMOSubUserGroup;
            _mapper = mapper;
        }



        [HttpPost("AssignSubGroupToHealthPlan")]
        public async Task<IActionResult> AssignSubGroupToHealthPlan(HMOSubUserGroupHealthPlanDtoForCreate hMOSubUserGroupHealthPlan)
        {
            if (hMOSubUserGroupHealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HMOHealthPlan = await _HMOHealthPlan.GetHMOHealthPlan(hMOSubUserGroupHealthPlan.HMOHealthPlanId);

            if (HMOHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOHealthPlanId" });
            }

            var HMOSubUserGroup = await _HMOSubUserGroup.GetHMOSubUserGroup(hMOSubUserGroupHealthPlan.HMOSubUserGroupId);
            
            if (HMOSubUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOSubUserGroupId" });
            }

            var HMOSubUSerGroupHealthPlanToCreate = _mapper.Map<HMOSubUserGroupHealthPlan>(hMOSubUserGroupHealthPlan);

            var res = await _HMOSubGroupHealthPlan.CreateHMOSubUserGroupHealthPlan(HMOSubUSerGroupHealthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Assign SubGroup To HealthPlan" });
            }

            return Ok(new
            {
                message = "SubGroup Assigned To HealthPlan Successfully"
            });
        }

        [Route("DeleteSubGroupFromHealthPlan")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSubGroupFromHealthPlan(HMOSubUserGroupHealthPlanDtoForDelete HealthPlanSubGroup)
        {
            if (HealthPlanSubGroup == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HealthPlanSubGroupToDelete = _mapper.Map<HMOSubUserGroupHealthPlan>(HealthPlanSubGroup);

            var res = await _HMOSubGroupHealthPlan.DeleteHMOSubUserGroupHealthPlan(HealthPlanSubGroupToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Sub Group Health Plan Failed To Delete" });
            }

            return Ok(new { HealthPlanSubGroup, message = "Health Plan Sub Group Deleted" });
        }
    }
}
