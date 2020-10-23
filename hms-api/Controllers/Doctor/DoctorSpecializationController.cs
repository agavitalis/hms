using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Services.Interfaces.Doctor;
using Microsoft.AspNetCore.Mvc;
using static HMS.ViewModels.Doctor.DoctorSpecializationViewModel;

namespace HMS.Controllers.Doctor
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorSpecializationController : Controller
    {
        private readonly IDoctorSpecialization _doctorSpecialization;

        public DoctorSpecializationController(IDoctorSpecialization doctorSpecialization)
        {
            _doctorSpecialization = doctorSpecialization;
        }

        [Route("ViewDoctorSpecialization")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorSpecializationsAsync(string DoctorId)
        {
            var specialization = await _doctorSpecialization.GetDoctorSpecializationAsync(DoctorId);
            if (specialization != null)
            {
                return Ok(new
                {
                    specialization
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to Get Doctor Specialization"
                });
            }
        }


        [Route("AddDoctorSpecialization")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctorSpecializationAsync([FromBody] CreateDoctorSpecializationViewModel DoctorSpecialization)
        {
            if (await _doctorSpecialization.CreateDoctorSpecializationAsync(DoctorSpecialization))
            {
                return Ok(new
                {
                    message = "Doctor Specialization Added Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to Add Doctor Specialization"
                });
            }
        }

        [Route("EditDoctorSpecialization")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorSpecializationAsync([FromBody] EditDoctorSpecializationViewModel DoctorSpecialization)
        {
            if (await _doctorSpecialization.EditDoctorSpecializationAsync(DoctorSpecialization))
            {
                return Ok(new
                {
                    message = "Doctor Specialization Updated Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to Update Doctor Specialization"
                });
            }
        }

        [Route("DeleteDoctorSpecialization")]
        [HttpPost]
        public async Task<IActionResult> DeleteDoctorSpecializationAsync(string SpecializationId)
        {
            if (await _doctorSpecialization.DeleteDoctorSpecializationAsync(SpecializationId))
            {
                return Ok(new
                {
                    message = "Doctor Specialization Updated Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to Update Doctor Specialization"
                });
            }
        }
    }
}
