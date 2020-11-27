using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor - Manage Consultation")]
    [ApiController]
    public class DoctorConsultationController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorConsultationController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Route("ViewAllConsultations")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorConsultationQueue(string DoctorId)
        {

            var doctorConsultations = _applicationDbContext.Consultations.Where(p => p.DoctorId == DoctorId).Join(
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
            if (doctorConsultations != null)
            {
                return Ok(new
                {
                    doctorConsultations,
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
