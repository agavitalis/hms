using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMO Sub Group Patients")]
    [ApiController]
    public class HMOSubGroupPatientController : Controller
    {
        private readonly IHMOSubUserGroupPatient _HMOSubGroupPatient;
        private readonly IHMOSubUserGroup _HMOSubUserGroup;
        private readonly IPatientProfile _patient;
        private readonly IMapper _mapper;


        public HMOSubGroupPatientController(IHMOSubUserGroupPatient HMOSubGroupPatient, IHMOSubUserGroup HMOSubUserGroup, IPatientProfile patient, IMapper mapper)
        {
            _HMOSubGroupPatient = HMOSubGroupPatient;
            _HMOSubUserGroup = HMOSubUserGroup;
            _patient = patient;
            _mapper = mapper;
        }

        [Route("GetSubGroupPatient")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs(string SubGroupPatientId)
        {
            if (string.IsNullOrEmpty(SubGroupPatientId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var patient = await _HMOSubGroupPatient.GetHMOSubUserGroupPatient(SubGroupPatientId);

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

        [Route("GetSubGroupPatientsBySubGroup")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanPatientsByHealthPlan([FromQuery] PaginationParameter paginationParameter, string HMOSubGroupId)
        {
            var Patients = _HMOSubGroupPatient.GetHMOSubUserGroupPatients(HMOSubGroupId, paginationParameter);

            var paginationDetails = new
            {
                Patients.TotalCount,
                Patients.PageSize,
                Patients.CurrentPage,
                Patients.TotalPages,
                Patients.HasNext,
                Patients.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                Patients,
                paginationDetails,
                message = "HMO HealthPlan Patients Fetched"
            });
        }

        [HttpPost("AssignPatientToHMOSubGroup")]
        public async Task<IActionResult> AssignPatientToSubGroup(HMOSubUserGroupPatientDtoForCreate HMOSubUserGroupPatient)
        {
            if (HMOSubUserGroupPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var HMOSubUserGroup = await _HMOSubUserGroup.GetHMOSubUserGroup(HMOSubUserGroupPatient.HMOSubUserGroupId);

            if (HMOSubUserGroup == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOSubUserGroup" });
            }

            var patient = await _patient.GetPatientByIdAsync(HMOSubUserGroupPatient.PatientId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid patientId" });
            }

            var HMOSubUserGroupPatientToCreate = _mapper.Map<HMOSubUserGroupPatient>(HMOSubUserGroupPatient);

            var res = await _HMOSubGroupPatient.CreateHMOSubGroupPatient(HMOSubUserGroupPatientToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO failed to create" });
            }

            return Ok(new
            {
                message = "Patient Assigned To HMOSubUserGroup Successfully"
            });
        }

        [Route("DeletePatientFromSubGroup")]
        [HttpDelete]
        public async Task<IActionResult> DeletePatientFromSubGroup(HMOSubUserGroupPatientDtoForDelete SubGroupPatient)
        {
            if (SubGroupPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var subGroupPatientPatientToDelete = _mapper.Map<HMOSubUserGroupPatient>(SubGroupPatient);

            var res = await _HMOSubGroupPatient.DeleteHMOSubGroupPatient(subGroupPatientPatientToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Sub Group Patient failed to delete" });
            }

            return Ok(new { SubGroupPatient, message = "Sub Group Patient Deleted" });
        }
    }
}
