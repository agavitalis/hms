using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Database;
using HMS.Models.Patient;
using HMS.ViewModels.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers.Doctor
{
    [Route("api/Doctor")]
    [ApiController]
    public class DoctorDrugPrescriptionController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DoctorDrugPrescriptionController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        [Route("PrescribeDrugs")]
        [HttpPost]
        public async Task<IActionResult> ViewDrugPrescriptionsPerAppointment([FromBody] DrugPrescriptionViewModel Prescribe)
        {
            if (ModelState.IsValid)
            {
                var prescriptions = new PatientDrugPrescription()
                {
                    Ailment = Prescribe.Ailment,
                    DrugId = Prescribe.DrugId,
                    Quantity = Prescribe.Quantity,
                    Frequency = Prescribe.Frequency,
                    Dosage = Prescribe.Dosage,
                    Comment = Prescribe.Comment,

                    AppointmentId = Prescribe.AppointmentId,
                    PatientId = Prescribe.PatientId,
                    DoctorId = Prescribe.DoctorId
                };

                _applicationDbContext.PatientDrugPrescritions.Add(prescriptions);
                await _applicationDbContext.SaveChangesAsync();


                return Ok(new
                {
                    response = 200,
                    message = "Prescription Added"
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

        [Route("ViewMyDrugPrescriptions")]
        [HttpGet]
        public async Task<IActionResult> ViewMyDrugPrescriptions(string DoctorId)
        {
            //get all the prescriptions made for this patient
            var prescriptions = await _applicationDbContext.PatientDrugPrescritions
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
                    (prescriptions, patientProfile) => new {  patientProfile, prescriptions }
                )
                .GroupBy(x => new { x.prescriptions.prescriptions.AppointmentId })
                .ToListAsync();

            return Ok(new
            {
                prescriptions,
                message = "Doctors Prescriptions"
            });

        }

        [Route("ViewDrugPrescriptionsPerAppointment")]
        [HttpGet]
        public async Task<IActionResult> ViewDrugPrescriptionsPerAppointment(string AppintmentId)
        {
            //get all the prescriptions made for this patient
            var prescriptions = await _applicationDbContext.PatientDrugPrescritions
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

        [Route("DeleteDrugPrescription")]
        [HttpDelete]
        public async Task<IActionResult> DeleteScheduleAsync(string PrescriptionId)
        {
                        
            // Retrieve Doctor Drug Prescription by id
            var DoctorPrescription = await _applicationDbContext.PatientDrugPrescritions.FirstOrDefaultAsync(d => d.Id == PrescriptionId);

            // Validate Doctor Drug Prescription selected is not null
            if (DoctorPrescription != null)
            {
                //Delete Drug Prescription From Database
                _applicationDbContext.PatientDrugPrescritions.Remove(DoctorPrescription);
                // Save changes in database
                await _applicationDbContext.SaveChangesAsync();
                return Ok(new
                {
                    message = "doctor schedule Deleted Successfully"
                });

            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Failed to Delete doctor schedule"
                });
            }

        }

    }
}