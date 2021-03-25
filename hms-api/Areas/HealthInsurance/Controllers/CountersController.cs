using System.Threading.Tasks;
using HMS.Areas.HealthInsurance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage Counters")]
    [ApiController]
    public class CountersController : Controller
    {
        private readonly IHMOSubUserGroupPatient _HMOSubUserGroupPatient;
        private readonly IHMOHealthPlanPatient _HMOHealthPlanPatient;
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IHMO _HMO;
        private readonly IHMOSubUserGroup _HMOSubUserGroup;
        private readonly IHMOUserGroup _HMOUserGroup;
        private readonly INHISHealthPlan _NHISHealthPlan;
        private readonly INHISHealthPlanDrug _NHISHealthPlanDrug;
        private readonly INHISHealthPlanPatient _NHISHealthPlanPatient;
        private readonly INHISHealthPlanService _NHISHealthPlanService;
       
        public CountersController(IHMOSubUserGroupPatient HMOSubUserGroupPatient, IHMOHealthPlanPatient HMOHealthPlanPatient, IHMOHealthPlan HMOHealthPlan, IHMO HMO, IHMOSubUserGroup HMOSubUserGroup, IHMOUserGroup HMOUserGroup, INHISHealthPlan NHISHealthPlan, INHISHealthPlanDrug NHISHealthPlanDrug, INHISHealthPlanService NHISHealthPlanService, INHISHealthPlanPatient NHISHealthPlanPatient)
        {
            _HMOSubUserGroupPatient = HMOSubUserGroupPatient;
            _HMOHealthPlanPatient = HMOHealthPlanPatient;
            _HMOHealthPlan = HMOHealthPlan;
            _HMO = HMO;
            _HMOSubUserGroup = HMOSubUserGroup;
            _HMOUserGroup = HMOUserGroup;
            _NHISHealthPlan = NHISHealthPlan;
            _NHISHealthPlanDrug = NHISHealthPlanDrug;
            _NHISHealthPlanPatient = NHISHealthPlanPatient;
            _NHISHealthPlanService = NHISHealthPlanService;
        }


        [Route("GetHMOCounters")]
        [HttpGet]
        public async Task<IActionResult> GetHMOCounters(string HMOSubUserGroupId, string HMOHealthPlanId, string HMOId, string HMOUserGroupId)
        {
            var HMOCount = await _HMO.GetHMOCount();
            var HMOHealthPlanCount = await _HMOHealthPlan.GetHealthPlanCount(HMOId);
            var HMOHealthPlanPatientCount = await _HMOHealthPlanPatient.GetHealthPlanPatientCount(HMOHealthPlanId);
            var HMOSubUserGroupCount = await _HMOSubUserGroup.GetSubUserGroupCount(HMOUserGroupId);
            var HMOSubUserGroupPatientCount = await _HMOSubUserGroupPatient.GetHMOSubUserGroupPatientCount(HMOSubUserGroupId);
            var HMOUserGroupPatientCount = await _HMOUserGroup.GetUserGroupCount(HMOId);

            return Ok(new
            {
                HMOCount,
                HMOHealthPlanCount, 
                HMOHealthPlanPatientCount,
                HMOSubUserGroupCount,
                HMOSubUserGroupPatientCount, 
                HMOUserGroupPatientCount,
                message = "HMO Counts"
            });
        }

        [Route("GetNHISCounters")]
        [HttpGet]
        public async Task<IActionResult> GetNHISCounters(string NHISHealthPlanId)
        {
            var NHISHealthPlanCount = await _NHISHealthPlan.GetNHISHealthPlanCount();
            var NHISHealthPlanDrugCount = await _NHISHealthPlanDrug.GetNHISHealthPlanDrugCount(NHISHealthPlanId);
            var NHISHealthPlanPatientCount = await _NHISHealthPlanPatient.GetNHISHealthPlanPatientCount(NHISHealthPlanId);
            var NHISHealthPlanServiceCount = await _NHISHealthPlanService.GetNHISHealthPlanServiceCount(NHISHealthPlanId);
           
            return Ok(new
            {
                NHISHealthPlanCount,
                NHISHealthPlanDrugCount,
                NHISHealthPlanPatientCount,
                NHISHealthPlanServiceCount,
               
                message = "NHIS Counts"
            });
        }
    }
}
