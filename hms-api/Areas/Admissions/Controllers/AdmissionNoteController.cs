using System.Threading.Tasks;
using HMS.Areas.Admissions.Dtos;
using HMS.Areas.Admissions.Interfaces;
using HMS.Services.Helpers;
using HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Admission Notes")]
    [ApiController]
    public class AdmissionNoteController : Controller
    {
        private readonly IAdmissionNote _admissionNote;
        private readonly IAdmission _admission;
        private readonly IMapper _mapper;

        public AdmissionNoteController(IAdmissionNote admissionNote, IAdmission admission, IMapper mapper)
        {
            _admission = admission;
            _admissionNote = admissionNote;
            _mapper = mapper;
        }

        [Route("GetAdmissionNote")]
        [HttpGet]
        public async Task<IActionResult> GetAdmissionNote(string AdmissionNoteId)
        {
            if (AdmissionNoteId == "")
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var admissionNote = await _admissionNote.GetAdmissionNote(AdmissionNoteId);

            if (admissionNote == null)
            {
                return BadRequest(new { message = "Invalid Admission Note Id" });
            }

            return Ok(new { admissionNote, message = "Admission Note returned" });
        }


        [Route("GetAdmissionNotesForAdmission")]
        [HttpGet]
        public async Task<IActionResult> GetAdmissionNotesForAdmission(string AdmissionId, [FromQuery] PaginationParameter paginationParameter)
        {
            var admission = await _admission.GetAdmission(AdmissionId);

            if (admission == null)
            {
                return BadRequest(new { message = "An Admission with this Id was not found" });
            }


            var admissionNotes = _admissionNote.GetAdmissionNotes(AdmissionId, paginationParameter);

            var paginationDetails = new
            {
                admissionNotes.TotalCount,
                admissionNotes.PageSize,
                admissionNotes.CurrentPage,
                admissionNotes.TotalPages,
                admissionNotes.HasNext,
                admissionNotes.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                admissionNotes,
                paginationDetails,
                message = "Admission Note Returned"
            });
        }



        [Route("CreateAdmissionNote")]
        [HttpPost]
        public async Task<IActionResult> UpdatePatientVitals([FromBody] AdmissionNoteDtoForCreate AdmissionNote)
        {
            if (AdmissionNote == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var admissionNoteToCreate = _mapper.Map<AdmissionNote>(AdmissionNote);

            var res = await _admissionNote.CreateAdmissionNote(admissionNoteToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Admission Note Failed To Create" });
            }

            return Ok(new
            {
                admissionNoteToCreate,
                message = "Admission Note created successfully"
            });
        }
    }
}        


