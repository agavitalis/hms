using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHealthPlan _healthPlan;
        private readonly IMapper _mapper;

        public AccountController(IHealthPlan healthPlan, IMapper mapper)
        {
            _healthPlan = healthPlan;
            _mapper = mapper;
        }

        [HttpGet("index")]
        public async Task<IActionResult> AllAccount()
        {

        }

        [HttpPost("CreateHealthPlan")]
        public async Task<IActionResult> CreateHealthPlan(HealthPlanDtoForCreate healthPlan)
        {
            if(healthPlan == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlanToCreate = _mapper.Map<HealthPlan>(healthPlan);

            var res = await _healthPlan.InsertHealthPlan(healthPlanToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "Health Plan failed to create" });
            }

            return CreatedAtRoute("CreateHealthPlan", healthPlan);
        }
    }
}
