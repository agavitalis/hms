﻿using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.NHIS.Dtos;
using HMS.Areas.Patient.Interfaces;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        [Route("GetHealthPlanPatient")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs(string HealthPlanId)
        {
            if (string.IsNullOrEmpty(HealthPlanId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var patient = await _patientHMOHealthPlan.GetHMOHealthPlanPatient(HealthPlanId);

            if (patient == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }
            return Ok(new
            {
                patient,
                message = "Patient Fetched"
            });
        }

        [Route("GetHealthPlanPatientsByHealthPlan")]
        [HttpGet]
        public async Task<IActionResult> GetHealthPlanPatientsByHealthPlan([FromQuery] PaginationParameter paginationParameter, string HMOHealthPlanId)
        {
            var HealthPlanPatients = _patientHMOHealthPlan.GetHMOHealthPlanPatients(HMOHealthPlanId, paginationParameter);

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
                message = "HMO HealthPlan Patients Fetched"
            });
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

        [Route("DeletePatientFromHealthPlan")]
        [HttpDelete]
        public async Task<IActionResult> DeleteHealthPlanDrug(HMOHealthPlanPatientDtoForDelete HealthPlanPatient)
        {
            if (HealthPlanPatient == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanPatientToDelete = _mapper.Map<HMOHealthPlanPatient>(HealthPlanPatient);

            var res = await _patientHMOHealthPlan.DeleteHMOHealthPlanPatient(healthPlanPatientToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan Patient failed to delete" });
            }

            return Ok(new { HealthPlanPatient, message = "Health Plan Patient Deleted" });
        }
    }
}
