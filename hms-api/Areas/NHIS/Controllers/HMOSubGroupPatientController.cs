using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.NHIS.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/NHIS", Name = "NHIS - Manage Patient HMO Sub Group Patients")]
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
    }
}
