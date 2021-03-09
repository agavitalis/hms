using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Interfaces.Nurse;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMS.Areas.Nurse.Controllers
{
    [Route("api/Nurse", Name = "Nurse - Manage Profile")]
    [ApiController]
    public class NurseProfileController : Controller
    {
        private readonly INurse _nurse;

        public NurseProfileController(INurse nurse)
        {
            _nurse = nurse;
        }

        [HttpGet("GetNurses")]
        public async Task<IActionResult> GetNurses([FromQuery] PaginationParameter paginationParameter)
        {
            var nurses = _nurse.GetNurses(paginationParameter);

            var paginationDetails = new
            {
                nurses.TotalCount,
                nurses.PageSize,
                nurses.CurrentPage,
                nurses.TotalPages,
                nurses.HasNext,
                nurses.HasPrevious
            };

            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                nurses,
                paginationDetails,
                message = "Nurses Returned"
            });

        }
    }
}
