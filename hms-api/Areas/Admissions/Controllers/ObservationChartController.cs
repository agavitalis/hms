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
        public ObservationChartController(IObservationChart observationChart, IUser user)
        {
            _observationChart = observationChart;
            _user = user;
        }

        [Route("GetAdmissionObservationChart")]
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


        [Route("UpdatePatientObservationChart")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientVitals([FromBody] ObservationChartDtoForUpdate ObservationChart)
        {
            if (ModelState.IsValid)
            {
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
