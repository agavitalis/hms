using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class WardController : ControllerBase
    {
        private readonly IWard _ward;
        private readonly IMapper _mapper;

        public WardController(IWard ward, IMapper mapper)
        {
            _ward = ward;
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
        public async Task<IActionResult> AllWards()
        {
            var wards = await _ward.GetAllWards();

            if (wards.Any())
                return Ok(new { wards, message = "Wards Fetched" });
            else
                return NoContent();
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
    }
}
