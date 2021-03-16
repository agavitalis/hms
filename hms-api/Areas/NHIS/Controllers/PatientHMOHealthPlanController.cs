using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/NHIS", Name = "NHIS - Manage Patient HMO Health Plans")]
    [ApiController]
    public class PatientHMOHealthPlanController : Controller
    {
        private readonly IPatientHMOHealthPlan _patientHMOHealthPlan;
        private readonly IMapper _mapper;


        public PatientHMOHealthPlanController(IPatientHMOHealthPlan patientHMOHealthPlan, IMapper mapper)
        {
            _patientHMOHealthPlan = patientHMOHealthPlan;
            _mapper = mapper;
        }



        [HttpPost("AssignPatientToHealthPlan")]
        public async Task<IActionResult> AssignPatientToHealthPlan(PatientHMOHealthPlanDtoForCreate PatientHMOHealthPlan)
        {
            if (PatientHMOHealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var PatientHMOHealthPlanToCreate = _mapper.Map<PatientHMOHealthPlan>(PatientHMOHealthPlan);

            var res = await _patientHMOHealthPlan.CreatePatientHMOHealthPlan(PatientHMOHealthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO failed to create" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HealthPlan Successfully"
            });
        }
    }
}
