using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission")]
    [ApiController]
    public class AdmissionController : Controller
    {
        private readonly IAdmission _admission;
        private readonly IBed _bed;
        private readonly IPatientProfile _patient;

        public AdmissionController(IAdmission admission, IBed bed, IPatientProfile patient)
        {
            _admission = admission;
            _patient = patient;
            _bed = bed;

        }


        [Route("GetAdmissions")]
        [HttpGet]
        public async Task<IActionResult> GetAdmittedPatients([FromQuery] PaginationParameter paginationParameter)
        {

            var admissions = _admission.GetAdmissions(paginationParameter);

            var paginationDetails = new
            {
                admissions.TotalCount,
                admissions.PageSize,
                admissions.CurrentPage,
                admissions.TotalPages,
                admissions.HasNext,
                admissions.HasPrevious
            };

            
            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                admissions,
                paginationDetails,
                message = "Admissions Fetched"
            });
        }


        [Route("AssignPatientToBedspace")]
        [HttpPost]
        public async Task<IActionResult> GetAdmittedPatients(AdmissionDtoForBedAssignment Admission)
        {
            var admission = await _admission.GetAdmission(Admission.AdmissionId);
            var bed = await _bed.GetBed(Admission.BedId);
            var patient = await _patient.GetPatientByIdAsync(admission.PatientId);
            

            var accountBalance = patient.Account.AccountBalance;

            if (accountBalance < 20000)
            {
                return BadRequest(new { message = "Insuficient Account Balance" });
            }

            admission.BedId = Admission.BedId;
            bed.IsAvailable = false;
            var res = await _admission.UpdateAdmission(admission);
            var res1 = await _bed.UpdateBed(bed);

            if (!res)
            {
                return BadRequest(new { response = "301", message = "Failed To Assign Patient a Bed Space" });
            }

            return Ok(new
            {
                admission,
                message = "Assigned BedSpace To Patient"
            });
        }
    }
}
