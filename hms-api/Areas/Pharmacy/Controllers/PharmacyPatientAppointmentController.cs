﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Pharmacy.Controllers
{
    [Route("api/Pharmacy")]
    [ApiController]
    public class PharmacyPatientAppointmentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PharmacyPatientAppointmentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Route("GetPatientAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPatientAppointments(string PatientId)
        {
            var apponintments = await _applicationDbContext.DoctorAppointments.Where(p => p.PatientId == PatientId)
                                  .Join(
                                      _applicationDbContext.ApplicationUsers,
                                      appointment => appointment.PatientId,
                                      patient => patient.Id,
                                      (appointment, patient) => new { appointment, patient }
                                  )
                                  .FirstAsync();

            if (apponintments != null)
            {
                return Ok(new
                {
                    apponintments,
                    message = "Patient Profile"
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Credentials Passed"
                });
            }

        }


    }
}