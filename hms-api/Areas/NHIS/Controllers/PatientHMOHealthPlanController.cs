using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Patient.Interfaces;
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
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IPatientProfile _patient;


        public PatientHMOHealthPlanController(IPatientHMOHealthPlan patientHMOHealthPlan, IMapper mapper, IPatientProfile patient, IHMOHealthPlan HMOHealthPlan)
        {
            _patientHMOHealthPlan = patientHMOHealthPlan;
            _patient = patient;
            _HMOHealthPlan = HMOHealthPlan;
            _mapper = mapper;
        }



        [HttpPost("AssignPatientToHealthPlan")]
        public async Task<IActionResult> AssignPatientToHealthPlan(PatientHMOHealthPlanDtoForCreate PatientHMOHealthPlan)
        {
            if (PatientHMOHealthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HMOHealthPlan = await _HMOHealthPlan.GetHMOHealthPlan(PatientHMOHealthPlan.HMOHealthPlanId);

            if (HMOHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOHealthPlanId" });
            }

            var patient = await _patient.GetPatientByIdAsync(PatientHMOHealthPlan.PatientId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid PatientId" });
            }


            var PatientHMOHealthPlanToCreate = _mapper.Map<PatientHMOHealthPlan>(PatientHMOHealthPlan);

            var res = await _patientHMOHealthPlan.CreatePatientHMOHealthPlan(PatientHMOHealthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed to Assign Patient To HealthPlan" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HealthPlan Successfully"
            });
        }
    }
}
