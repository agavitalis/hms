using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Patient;
using HMS.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorLabTestPrescriptionController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorLabTestPrescriptionController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        [Route("PrescribeLabTests")]
        [HttpPost]
        public async Task<IActionResult> ViewDrugPrescriptionsPerAppointment([FromBody] LabTestPrescriptionViewModel Prescribe)
        {
            if (ModelState.IsValid)
            {
                var prescriptions = new PatientLabPrescription()
                {
                    LabTestId = Prescribe.LabTestId,
                    CommentByDoctor = Prescribe.CommentByDoctor,
                    AppointmentId = Prescribe.AppointmentId,
                    PatientId = Prescribe.PatientId,
                    DoctorId = Prescribe.DoctorId
                };

                _applicationDbContext.PatientLabPrescritions.Add(prescriptions);
                await _applicationDbContext.SaveChangesAsync();


                return Ok(new
                {
                    response = 200,
                    message = "Lab Prescriptions Added"
                });

            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Parameters Passed"
                });
            }

        }

        [Route("ViewMyLabPrescriptions")]
        [HttpGet]
        public async Task<IActionResult> ViewMyLabPrescriptions(string DoctorId)
        {
            //get all the prescriptions made for this patient
            var prescriptions = await _applicationDbContext.PatientLabPrescritions
                .Where(s => s.DoctorId == DoctorId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    prescriptions => prescriptions.PatientId,
                    applicationUser => applicationUser.Id,
                    (prescriptions, applicationUser) => new { prescriptions, applicationUser }
                )
                .Join(
                    _applicationDbContext.PatientProfiles,
                    prescriptions => prescriptions.applicationUser.Id,
                    patientProfile => patientProfile.PatientId,
                    (prescriptions, patientProfile) => new { patientProfile, prescriptions }
                )
                .GroupBy(x => new { x.prescriptions.prescriptions.AppointmentId })
                .ToListAsync();

            return Ok(new
            {
                prescriptions,
                message = "Doctors Prescriptions"
            });

        }

        [Route("ViewLabPrescriptionsPerAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewDrugPrescriptionsPerAppointment(string AppintmentId)
        {
            //get all the prescriptions made for this patient
            var prescriptions = await _applicationDbContext.PatientLabPrescritions
                .Where(s => s.AppointmentId == AppintmentId)
                .Join(
                    _applicationDbContext.ApplicationUsers,
                    prescriptions => prescriptions.PatientId,
                    applicationUser => applicationUser.Id,
                    (prescriptions, applicationUser) => new { prescriptions, applicationUser }
                )
                .Join(
                    _applicationDbContext.PatientProfiles,
                    prescriptions => prescriptions.applicationUser.Id,
                    patientProfile => patientProfile.PatientId,
                    (prescriptions, patientProfile) => new { patientProfile, prescriptions }
                )
                .ToListAsync();

            return Ok(new
            {
                prescriptions,
                message = "Doctors Prescriptions"
            });

        }


    }
}