using System.Threading.Tasks;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Observation Chart")]
    [ApiController]
    public class ObservationChartController : Controller
    {
        private readonly IObservationChart _observationChart;
        private readonly IUser _user;
        private readonly IAdmission _admission;
        public ObservationChartController(IAdmission admission, IObservationChart observationChart, IUser user)
        {
            _admission = admission;
            _observationChart = observationChart;
            _user = user;
        }

        [Route("GetObservationChart")]
        [HttpGet]
        public async Task<IActionResult> GetPatientObservationChart(string AdmissionId)
        {
            var patientObservationChart = await _observationChart.GetAdmissionObservationChart(AdmissionId);

            if (patientObservationChart == null)
            {
                return BadRequest(new { message = "This Admission Has No Observation Chart History" });
            }

            return Ok(new
            {
                patientObservationChart,
                message = "Patient Observation Chart"
            });
        }


        [Route("UpdateObservationChart")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientVitals([FromBody] ObservationChartDtoForUpdate ObservationChart)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.GetUserByIdAsync(ObservationChart.InitiatorId);
                var admission = await _admission.GetAdmission(ObservationChart.AdmissionId);
                if (user == null)
                {
                    return BadRequest(new { response = 301, message = "Invalid InitiatorId" });
                }

                if (admission == null)
                {
                    return BadRequest(new { response = 301, message = "Invalid AdmissionId" });
                }

                if (await _observationChart.UpdateObservationChart(ObservationChart))
                {
                    return Ok(new
                    {
                        message = "Updated Patient Observation Chart"
                    });
                }

                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to Updated  Patient Observation Chart"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }
    }
}
