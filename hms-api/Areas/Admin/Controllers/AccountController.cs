using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using HMS.Areas.Patient.Dtos;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHealthPlan _healthPlan;
        private readonly IMapper _mapper;
        private readonly IAdmin _adminRepo;

        public AccountController(IHealthPlan healthPlan, IMapper mapper, IAdmin adminRepo)
        {
            _healthPlan = healthPlan;
            _mapper = mapper;
            _adminRepo = adminRepo;
        }

        [HttpGet("Accounts")]
        public async Task<IActionResult> AllAccount(PaginationParameter paginationParam)
        {

            return Ok();
        }

        [HttpPost("/HealthPlan/CreateHealthPlan")]
        public async Task<IActionResult> CreateHealthPlan(HealthPlanDtoForCreate healthPlan)
        {
            if(healthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanToCreate = _mapper.Map<HealthPlan>(healthPlan);

            var res = await _healthPlan.InsertHealthPlan(healthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan failed to create" });
            }

            return CreatedAtRoute("HealthPlan", healthPlan);
        }

        [HttpGet("HealthPlan", Name = "HealthPlan")]
        public async Task<IActionResult> AllHealthPlan()
        {
            var plans = await _healthPlan.GetAllHealthPlan();

            if (plans.Any())
                return Ok(new { plans, message = "HealthPlans Fetched" });
            else
                return NoContent();
        }

        [HttpGet("HealthPlan/{id:int}")]
        public async Task<IActionResult> GetHealthPlan(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }

            var res = await _healthPlan.GetHealthPlanByIdAsync(Id);

            if(res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Health Plan returned" });
        }

        /// <summary>
        /// To on board a patient, checks is account Id was provided from model
        /// if not create account, then generate fileNumber for patient
        /// once this is done we create a patient, Remember to add transaction
        /// </summary>
        /// <param name="patientToCreate"></param>
        /// <returns></returns>
        [HttpPost("PatientOnBoarding")]
        public async Task<IActionResult> OnBoardPatient(PatientDtoForCreate patientToCreate)
        {
            try
            {
                if (patientToCreate == null)
                    return BadRequest();
                Models.Account createdAccount = null;
                //create account fer patient
                if (string.IsNullOrEmpty(patientToCreate.AccountId))
                {
                    var accountToCreate = _mapper.Map<Models.Account>(patientToCreate);

                     createdAccount = await _adminRepo.InsertAccount(accountToCreate);

                    if (createdAccount == null)
                        return BadRequest(new { message = "Account failed to create", status = false });
                }
                else
                {
                    createdAccount = await _adminRepo.GetAccountById(patientToCreate.AccountId);

                    if(createdAccount == null)
                    {
                        return NotFound(new { message = "Account not found", status = "false" });
                    }
                }

                //create file number
                var fileToCreate = _mapper.Map<FileDtoForCreate>(createdAccount);

                var filecreated = await _adminRepo.GenerateFileNumber(fileToCreate);

                if (filecreated == null)
                    return BadRequest(new { message = "file failed to create", status = false });

                patientToCreate.AccountId = createdAccount.Id;
                patientToCreate.FileNumber = filecreated.FileNumber;

                var patient = _mapper.Map<PatientProfile>(patientToCreate);

                var res = await _adminRepo.InsertPatient(patient);
                if (!res)
                {
                    return BadRequest(new { message = "Error occured while creating patient", status = false });
                }

                return CreatedAtRoute("Patients", patientToCreate);
            }
            catch (Exception)
            {

                throw;
            }           
        }
    }
}
