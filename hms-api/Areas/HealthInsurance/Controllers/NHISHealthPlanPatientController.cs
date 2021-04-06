using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.HealthInsurance.Dtos;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [Route("GetNHISHealthPlanPatient")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs(string HealthPlanId)
        {
            if (string.IsNullOrEmpty(HealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var patient = await _NHISHealthPlanPatient.GetNHISHealthPlanPatient(HealthPlanId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }
            return Ok(new
            {
                patient,
                message = "Patient Returned"
            });
        }

        [Route("GetNHISHealthPlanPatientsByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanPatientsByHealthPlan([FromQuery] PaginationParameter paginationParameter, string HealthPlanId)
        {
            var HealthPlanPatients = _NHISHealthPlanPatient.GetNHISHealthPlanPatients(HealthPlanId, paginationParameter);

            var paginationDetails = new
            {
                HealthPlanPatients.TotalCount,
                HealthPlanPatients.PageSize,
                HealthPlanPatients.CurrentPage,
                HealthPlanPatients.TotalPages,
                HealthPlanPatients.HasNext,
                HealthPlanPatients.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HealthPlanPatients,
                paginationDetails,
                message = "NHIS HealthPlan Patients Returned"
            });
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

        [Route("UpdatePatientNHISHealthPlan")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientNHISHealthPlan(NHISHealthPlanPatientDtoForUpdate nHISHealthPlanPatient)
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
                return BadRequest(new { response = "301", message = "Failed to Assign Patient To HealthPlan" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HealthPlan Successfully"
            });
        }

        [Route("DeletePatientFromNHISHealthPlan")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHealthPlanDrug(NHISHealthPlanPatientDtoForDelete HealthPlanPatient)
        {
            if (HealthPlanPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanPatientToDelete = _mapper.Map<NHISHealthPlanPatient>(HealthPlanPatient);

            var res = await _NHISHealthPlanPatient.DeleteHealthPlanPatient(healthPlanPatientToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Patient failed to delete" });
            }

            return Ok(new { HealthPlanPatient, message = "Health Plan Patient Deleted" });
        }
    }
}
