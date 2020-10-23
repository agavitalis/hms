using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers.Doctor
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorPatientQueueController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorPatientQueueController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Route("GetDoctorQueue")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorQueue(string DoctorId)
        {

            var DoctorQueue = _applicationDbContext.PatientQueue.Where(p => p.DateOfConsultation.Date == DateTime.Today && p.DoctorId == DoctorId).Join(
                           _applicationDbContext.ApplicationUsers,
                           PatientQueue => PatientQueue.PatientId,
                           applicationUsers => applicationUsers.Id,
                           (PatientQueue, patient) => new { PatientQueue, patient }
                       ).Join(
                            _applicationDbContext.ApplicationUsers,
                            PatientQueue => PatientQueue.PatientQueue.DoctorId,
                           applicationUsers => applicationUsers.Id,
                            (PatientQueue, doctor) => new { PatientQueue.PatientQueue, PatientQueue.patient, doctor }
                       ).ToList();


            //.ToListAsync();
            if (DoctorQueue != null)
            {
                return Ok(new
                {
                    DoctorQueue,
                    message = "Complete Patient List"
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
