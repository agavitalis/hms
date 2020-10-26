using System.Threading.Tasks;
using HMS.Areas.Patient.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static HMS.Areas.Patient.ViewModels.PatientPreConsultationViewModel;

namespace HMS.Areas.Patient.Controllers

{
    [Route("api/PatientPreConsultation")]
    [ApiController]
    public class PatientPreConsultationController : Controller
    {
        private readonly IPatientPreConsultation _patientPreConsultation;
        public PatientPreConsultationController(IPatientPreConsultation patientPreConsultation)
        {
            _patientPreConsultation = patientPreConsultation;
        }

        [Route("UpdatePatientVitals")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientVitals([FromBody] UpdatePatientVitalsViewModel patientVitals)
        {
            if (ModelState.IsValid)
            {
                if (await _patientPreConsultation.UpdatePatientVitalsAsync(patientVitals))
                {
                    return Ok(new
                    {
                        message = "Updated Patient Vitals"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Updated Patient Vitals"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [Route("UpdatePatientBMI")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientBMI([FromBody] UpdatePatientBMIViewModel patientBMI)
        {
            if (ModelState.IsValid)
            {
                if (await _patientPreConsultation.UpdatePatientBMIAsync(patientBMI))
                {
                    return Ok(new
                    {
                        message = "Updated Patient BMI"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Updated Patient BMI"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }
    }
}
