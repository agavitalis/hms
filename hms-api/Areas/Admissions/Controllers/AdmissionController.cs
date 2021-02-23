using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Patient.Interfaces;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        private readonly IMapper _mapper;
        private readonly IWard _ward;

   

        public AdmissionController(IAdmission admission, IBed bed, IWard ward, IPatientProfile patient, IMapper mapper)
        {
            _admission = admission;
            _patient = patient;
            _bed = bed;
            _ward = ward;
            _mapper = mapper;

        }


        [Route("GetAdmissionsWithBed")]
        [HttpGet]
        public async Task<IActionResult> GetAdmittedPatients([FromQuery] PaginationParameter paginationParameter , string WardId)
        {
            if (string.IsNullOrEmpty(WardId))
            {
                var admissions = _admission.GetAdmissionsWithBed(paginationParameter);

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
            else
            {
                var admissions = _admission.GetAdmissionsWithBed(paginationParameter);

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
           
        }

        [Route("GetAdmissionsWithoutBed")]
        [HttpGet]
        public async Task<IActionResult> GetAdmittedPatientsWithoutBed([FromQuery] PaginationParameter paginationParameter)
        {

            var admissions = _admission.GetAdmissionsWithoutBed(paginationParameter);

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

        [Route("GetAdmission")]
        [HttpGet]
        public async Task<IActionResult> GetAdmittedPatient(string AdmissionId)
        {
            
            if (AdmissionId == "")
            {
                return BadRequest();
            }

            var admission = await _admission.GetAdmission(AdmissionId);

            if (admission == null)
            {
                return NotFound();
            }

            return Ok(new { admission, mwessage = "Admission returned" });
        }


        [Route("AssignPatientToBedspace")]
        [HttpPost]
        public async Task<IActionResult> AssignPatientToBedspace(AdmissionDtoForBedAssignment Admission)
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
            var wardAvailable = await _ward.CheckWardAvailability(bed.WardId);
            if (wardAvailable == false)
            {
                var ward = await _ward.GetBedsWard(bed.Id);
                ward.IsAvailable = false;
                await _ward.UpdateWard(ward);
            }
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


        [Route("DischargePatient")]
        [HttpPost]
        public async Task<IActionResult> DischargePatient(AdmissionDtoForDischarge Admission)
        {
           
            if (Admission == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }
            var admission = await _admission.GetAdmission(Admission.AdmissionId);
            var bed = await _bed.GetBed(admission.BedId);
            admission.IsDischarged = true;
            admission.DischargeNote = Admission.DischargeNote;
            admission.DateOfDischarge = DateTime.Now;
            bed.IsAvailable = true;
         
            var admissionUpdated = await _admission.UpdateAdmission(admission);
            var res1 = await _bed.UpdateBed(bed);
            var ward = await _ward.GetBedsWard(bed.Id);
            ward.IsAvailable = true;
            await _ward.UpdateWard(ward);


            if (!admissionUpdated)
            {
                return BadRequest(new { response = "301", message = "Admission failed to update" });
            }

            return Ok(new
            {
                message = "Patient Discharged successfully"
            });
        }
    }
}
