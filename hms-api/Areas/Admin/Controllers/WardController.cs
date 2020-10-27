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

        [HttpGet("Ward/GetWard")]
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
        public async Task<IActionResult> CreateHealthPlan(WardDtoForCreate ward)
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

            return CreatedAtRoute("Ward", ward);
        }
    }
}
