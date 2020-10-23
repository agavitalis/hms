﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models.Lab;
using HMS.Services.Interfaces.Lab;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Lab.LabTestViewModel;

namespace HMS.Controllers.Lab
{
    [Route("api/Lab")]
    [ApiController]
    public class LabTestController : Controller
    {
        private readonly ILabTest _LabTestRepository;

        public LabTestController(ILabTest LabTestRepository)
        {
            _LabTestRepository = LabTestRepository;
        }

        [HttpGet]
        [Route("GetAllLabTests")]
        public async Task<IEnumerable<LabTest>> GetAllLabTestsAsync()
        {
            return await _LabTestRepository.GetAllLabTestsAsync();
        }

        [HttpGet]
        [Route("GetLabTestById")]
        public async Task<IActionResult> GetLabTestById(string Id)
        {
            var LabTest = await _LabTestRepository.GetLabTestByIdAsync(Id);
            if (LabTest != null)
            {
                return Ok(new { LabTest });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Lab Test Id"
                });
            }
        }

        [HttpPost]
        [Route("CreateLabTest")]
        public async Task<IActionResult> CreateLabTest([FromBody] CreateLabTestViewModel labTestVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestRepository.CreateLabTestAsync(labTestVM))
                {
                    return Ok(new
                    {
                        message = "Lab Test successfully created"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to create Lab Test"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("EditLabTest")]
        public async Task<IActionResult> Edit([FromBody] EditLabTestViewModel labTestVM)
        {
            if (ModelState.IsValid)
            {
                if (await _LabTestRepository.EditLabTestAsync(labTestVM))
                {
                    return Ok(new
                    {
                        message = "Lab Test Updated Successfully"
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(new { message = "Please Fill all fields" });
            }
        }

        [Route("GetLabTestsCount")]
        [HttpGet]
        public async Task<Int64> GetLabTestsCount()
        {
            return await _LabTestRepository.TotalNumber();
        }
        [Route("DeleteLabTest")]
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (await _LabTestRepository.DeleteLabTestAsync(Id))
            {
                return Ok(new { message = "Lab Test deleted successfully" });
            }
            else
            {
                return BadRequest(new { code = 301, message = "Unable to delete Lab Test" });
            }
        }

        [Route("FindLabTestByName")]
        [HttpGet]
        public async Task<IEnumerable<LabTest>> Find(string name)
        {
            return await _LabTestRepository.FindByNameAsync(name);
        }
    }
}
