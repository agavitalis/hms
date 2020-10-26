using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Patient.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientProfile _patientProfileRepo;
        private readonly IPatientPrescription _patientPrescriptionRepo;
        private readonly IDoctor _doctorRepo;

        public PatientController(IPatientProfile patientProfileRepo, IPatientPrescription patientPrescription, IDoctor doctorRepo)
        {
            _patientProfileRepo = patientProfileRepo;
            _patientPrescriptionRepo = patientPrescription;
            _doctorRepo = doctorRepo;
        }

        [Route("GetPatients")]
        [HttpGet]
        public async Task<IActionResult> GetPatients(string PatientId)
        {
            var PatientProfiles = await _patientProfileRepo.GetPatientsAsync();

            if (PatientProfiles != null)
            {
                return Ok(new
                {
                    PatientProfiles,
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

        [Route("GetPatientById")]
        [HttpGet]
        public async Task<IActionResult> GetPatientByIdAsync(string PatientId)
        {
            var PatientProfile = await _patientProfileRepo.GetPatientByIdAsync(PatientId);

            if (PatientProfile != null)
            {
                return Ok(new
                {
                    PatientProfile,
                    message = "Complete Patient Profile"
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

        /// <summary>
        /// Gets Patient appointment by patient Id
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>

        [Route("GetPatientAppointments")]
        [HttpGet]
        public async Task<IActionResult> GetPatientAppointments(string PatientId)
        {
            var apponintments = await _patientProfileRepo.GetPatientAppointmentByIdAsync(PatientId);

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

        /// <summary>
        /// Patien tPrescriptions section
        /// </summary>
        /// <param name="appointmentid"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("PatientPrescriptionsPerAppointment")]
        public async Task<IActionResult> PatientPrescriptionsPerAppointment(string appointmentid)
        {
            //check if the appointment id is existing
            var appointment = _doctorRepo.GetAppointment(appointmentid);
            if (appointment == null)
            {
                return BadRequest(new { message = "Please, select a valid appointment id" });
            }

            var prescription = await _patientPrescriptionRepo.GetPatientPrescriptionByAppointment(appointmentid);

            if (prescription.Any())
            {
                return Ok(new { prescription });
            }
            return NotFound(new { message = "No prescription is found for this appointment." });


        }

        [HttpGet]
        [Route("ViewAllPatientPrescriptions")]
        public async Task<IActionResult> ViewAllPatientPrescriptions(string patientid)
        {
            //check if patient id is valid
            var patient = await _patientProfileRepo.GetPatientByIdAsync(patientid);
            if (patient == null)
            {
                return NotFound(new { message = "Invalid patient id" });
            }

            var allprescriptions = await _patientPrescriptionRepo.AllPatientPrescription(patientid);
            if (allprescriptions.Any())
            {
                return Ok(new { allprescriptions });
            }
            return NotFound(new { message = "Not prescriptions yet for this patient." });
        }

        [HttpPost]
        [Route("GenerateLabTestInvoice")]
        public async Task<IActionResult> GenerateLabTestInvoice(string[] drugs, string appointmentid)
        {
            if (drugs == null || appointmentid == null)
            {
                return BadRequest(new { message = "Verify that drugs were selected and an appointment id was sent" });
            }

            var res = await _patientPrescriptionRepo.GenerateInvoice(drugs, appointmentid);
            if (!res.Item1)
                return NotFound(new { message = res.Item2 });

            return Ok(new { message = "Prescription invoice generated successfully" });
        }

        [HttpGet]
        [Route("ViewAllLabTestInvoicesGenerated")]
        public async Task<IActionResult> ViewAllLabTestInvoicesGenerated()
        {
            var AllInvoices = await _patientPrescriptionRepo.GetAllLabTestInvoices();

            if (AllInvoices.Any())
            {
                return Ok(new { AllInvoices });
            }

            return NotFound(new { message = "No Invoice found." });
        }



    }
}
