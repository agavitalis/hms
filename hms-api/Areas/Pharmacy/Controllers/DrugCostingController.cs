using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Patient.Interfaces;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Areas.Pharmacy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Pharmacy.Controllers
{

    [Route("api/Pharmacy", Name = "Pharmacy - Drug Costing")]
    [ApiController]
    public class DrugCostingController : ControllerBase
    {
        private readonly IDrugCosting _drugCosting;
        private readonly IMapper _mapper;
        private readonly IPatientProfile _patientRepo;

        public DrugCostingController(IDrugCosting drugCosting, IMapper mapper, IPatientProfile patientRepo)
        {
            _drugCosting = drugCosting;
            _mapper = mapper;
            _patientRepo = patientRepo;
        }


        [HttpPost("CostDrugs")]
        public async Task<IActionResult> CostDrugs(DrugCostingDto DrugCostings)
        {
            if (DrugCostings == null)
            {
                return BadRequest(new { message = "Please fill all the required parameters" });
            }

            //check if the patient exist
            var patient = await _patientRepo.GetPatientByIdAsync(DrugCostings.PatientId);
            if (patient == null)
            {
                return BadRequest(new
                {
                    response = "301",
                    message = "Invalid Patient Credentials Passed, Patient not found",
                });
            }

            //check if all drugs id passed exist
            var drugCheck = await _drugCosting.CheckIfDrugsExist(DrugCostings.Drugs);
            if (!drugCheck)
                return BadRequest(new
                {
                    response = "301",
                    message = "One or more Drugs Passed is/are invalid"
                });

            //Cost these drugs sent
            var costings = await _drugCosting.CostDrugs(DrugCostings);

            return Ok(new
            {
                costings,
                response = "200",
                message = "Drug Costing Was Successful"
            });

        }
    }

}

