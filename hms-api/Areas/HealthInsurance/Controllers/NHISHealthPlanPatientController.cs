using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.HealthInsurance.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage NHIS Health Plan Patients")]
    [ApiController]
    public class NHISHealthPlanPatientController : Controller
    {
        private readonly INHISHealthPlanPatient _NHISHealthPlanPatient;
        private readonly IMapper _mapper;
        private readonly INHISHealthPlan _NHISHealthPlan;
        private readonly IPatientProfile _patient;


        public NHISHealthPlanPatientController(INHISHealthPlanPatient NHISHealthPlanPatient, IMapper mapper, IPatientProfile patient, INHISHealthPlan NHISHealthPlan)
        {
            _NHISHealthPlanPatient = NHISHealthPlanPatient;
            _patient = patient;
            _NHISHealthPlan = NHISHealthPlan;
            _mapper = mapper;
        }

        [Route("AssignPatientToNHISHealthPlan")]
        [HttpPost]
        public async Task<IActionResult> AssignPatientToHealthPlan(NHISHealthPlanPatientDtoForCreate nHISHealthPlanPatient)
        {
            if (nHISHealthPlanPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(nHISHealthPlanPatient.NHISHealthPlanId);

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid NHISHealthPlanId" });
            }

            var patient = await _patient.GetPatientByIdAsync(nHISHealthPlanPatient.PatientId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid PatientId" });
            }


            var NHISHealthPlanPatientToCreate = _mapper.Map<NHISHealthPlanPatient>(nHISHealthPlanPatient);

            var res = await _NHISHealthPlanPatient.CreateNHISHealthPlanPatient(NHISHealthPlanPatientToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed to Assign Patient To HealthPlan" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HealthPlan Successfully"
            });
        }

        [Route("UpdatePatientHealthPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientHealthPlan(NHISHealthPlanPatientDtoForUpdate nHISHealthPlanPatient)
        {
            if (nHISHealthPlanPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var NHISHealthPlan = await _NHISHealthPlan.GetNHISHealthPlan(nHISHealthPlanPatient.NHISHealthPlanId);

            if (NHISHealthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid NHISHealthPlanId" });
            }

            var patient = await _patient.GetPatientByIdAsync(nHISHealthPlanPatient.PatientId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid PatientId" });
            }


            var NHISHealthPlanPatientToUpdate = _mapper.Map<NHISHealthPlanPatient>(nHISHealthPlanPatient);

            var res = await _NHISHealthPlanPatient.UpdateNHISHealthPlanPatient(NHISHealthPlanPatientToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed to Update Patient HealthPlan" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HealthPlan Successfully"
            });
        }
    }
}
