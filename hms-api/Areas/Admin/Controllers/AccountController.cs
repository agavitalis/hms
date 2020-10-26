using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHealthPlan _healthPlan;

        public AccountController(IHealthPlan healthPlan)
        {
            _healthPlan = healthPlan;
        }
    }
}
