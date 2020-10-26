using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMS.Controllers.Auth
{
    [Route("api/Admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        //public RolesController(RoleManager<IdentityRole> roleManager)
        //{
        //    _roleManager = roleManager;

        //}

        [Route("GetRoles")]
        [HttpGet]
        public async Task<Object> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            roles.RemoveAll(r => r.Name == "Admin");

            return Ok(new
            {
                roles

            });

        }
    }
}