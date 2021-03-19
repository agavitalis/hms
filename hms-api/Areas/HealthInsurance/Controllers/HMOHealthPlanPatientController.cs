using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Health Plan Patients")]
    [ApiController]
    public class HMOHealthPlanPatientController : Controller
    {
        private readonly IHMOHealthPlanPatient _patientHMOHealthPlan;
        private readonly IMapper _mapper;
        private readonly IHMOHealthPlan _HMOHealthPlan;
        private readonly IPatientProfile _patient;


        public HMOHealthPlanPatientController(IHMOHealthPlanPatient patientHMOHealthPlan, IMapper mapper, IPatientProfile patient, IHMOHealthPlan HMOHealthPlan)
        {
            _patientHMOHealthPlan = patientHMOHealthPlan;
            _patient = patient;
            _HMOHealthPlan = HMOHealthPlan;
            _mapper = mapper;
        }



        [HttpPost("AssignPatientToHealthPlan")]
        public async Task<IActionResult> AssignPatientToHealthPlan(HMOHealthPlanPatientDtoForCreate HMOHealthPlanPatient)
        {
            if (HMOHealthPlanPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HMOHealthPlan = await _HMOHealthPlan.GetHMOHealthPlan(HMOHealthPlanPatient.HMOHealthPlanId);

            if (HMOHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOHealthPlanId" });
            }

            var patient = await _patient.GetPatientByIdAsync(HMOHealthPlanPatient.PatientId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid PatientId" });
            }


            var HMOHealthPlanPatientToCreate = _mapper.Map<HMOHealthPlanPatient>(HMOHealthPlanPatient);

            var res = await _patientHMOHealthPlan.CreateHMOHealthPlanPatient(HMOHealthPlanPatientToCreate);
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
