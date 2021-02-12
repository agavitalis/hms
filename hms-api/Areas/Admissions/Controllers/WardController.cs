using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HMS.Areas.Admin.Dtos;
using HMS.Models;
using Newtonsoft.Json;
using HMS.Services.Helpers;
using HMS.Areas.Admissions.Interfaces;
using HMS.Areas.Admissions.Dtos;

namespace HMS.Areas.Admissions.Controllers
{
    [Route("api/Admission", Name = "Admission - Manage Wards")]
    [ApiController]
    public class WardController : ControllerBase
    {
        private readonly IWard _ward;
        private readonly IBed _bed;
        private readonly IMapper _mapper;

        public WardController(IWard ward, IBed bed, IMapper mapper)
        {
            _ward = ward;
            _bed = bed;
            _mapper = mapper;
        }

        [HttpGet("GetWard/{Id}")]
        public async Task<IActionResult> GetWardById(string Id)
        {
            if (Id == "")
            {
                return BadRequest();
            }

            var res = await _ward.GetWardByIdAsync(Id);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(new { res, mwessage = "Ward returned" });
        }

        

        [HttpGet("Ward/GetAllWards")]
        public async Task<IActionResult> GetWards([FromQuery] PaginationParameter paginationParameter)
        {
            var wards = _ward.GetWardsPagnation(paginationParameter);


            var paginationDetails = new
            {
                wards.TotalCount,
                wards.PageSize,
                wards.CurrentPage,
                wards.TotalPages,
                wards.HasNext,
                wards.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                wards,
                paginationDetails,
                message = "Wards Fetched"
            });

        }


        [HttpPost("Ward/CreateWard", Name = "Ward")]
        public async Task<IActionResult> CreateWard(WardDtoForCreate ward)
        {
            if (ward == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var wardToCreate = _mapper.Map<Ward>(ward);

            var res = await _ward.CreateWard(wardToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to create" });
            }

            return Ok(new
            {
                ward,
                message = "Ward created successfully"
            });
        }


        [HttpPost("Ward/CreateBed", Name = "Bed")]
        public async Task<IActionResult> CreateBed(BedDtoForCreate bed)
        {
            if (bed == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var bedToCreate = _mapper.Map<Bed>(bed);

            var res = await _bed.CreateBed(bedToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Bed failed to create" });
            }

            return Ok(new
            {
                bed,
                message = "Bed created successfully"
            });
        }

        [HttpPost("Ward/UpdateWard", Name = "updateWard")]
        public async Task<IActionResult> EditWard(WardDtoForUpdate ward)
        {
            if (ward == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var wardToUpdate = _mapper.Map<Ward>(ward);

            var res = await _ward.UpdateWard(wardToUpdate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to update" });
            }

            return Ok(new
            {
                ward,
                message = "Ward updated successfully"
            });
        }

        [HttpPost("Ward/DeleteWard", Name = "deleteWard")]
        public async Task<IActionResult> DeleteWard(WardDtoForDelete ward)
        {
            if (ward == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var wardToDelete = _mapper.Map<Ward>(ward);

            var res = await _ward.DeleteWard(wardToDelete);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Ward failed to delete" });
            }

            return Ok(new { ward, message = "Ward Deleted" });
        }

        [HttpGet("Ward/GetBedsInAWard")]
        public async Task<IActionResult> GetBedsInWard([FromQuery] PaginationParameter paginationParameter, string WardId)
        {
            var beds = _ward.GetBedsInWardPagnation(paginationParameter, WardId);


            var paginationDetails = new
            {
                beds.TotalCount,
                beds.PageSize,
                beds.CurrentPage,
                beds.TotalPages,
                beds.HasNext,
                beds.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                beds,
                paginationDetails,
                message = "Wards Fetched"
            });

        }
    }
}
