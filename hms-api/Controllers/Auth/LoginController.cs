using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using HMS.Models;
using HMS.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HMS.Controllers.Auth
{
    [Route("api/Auth")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public LoginController(IConfiguration config, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<Object> LoginAsync([FromBody]LoginViewModel loginDetails)
        {

            var authenticatedUser = await AuthenticateUserAsync(loginDetails);
            object response;

            if (authenticatedUser != null)
            {
                string tokenString = await GenerateJSONWebTokenAsync(authenticatedUser);

                response = Ok(new
                {
                    authenticatedUser,
                    token = tokenString
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid Login Credentials"
                });
            }

            return response;
        }


        [NonAction]
        private async Task<string> GenerateJSONWebTokenAsync(ApplicationUser authenticatedUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, authenticatedUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, authenticatedUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(authenticatedUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [NonAction]
        private async Task<ApplicationUser> AuthenticateUserAsync(LoginViewModel loginDetails)
        {
            ApplicationUser user = null;

            //Validate the User Credentials    
            var result = await _signInManager.PasswordSignInAsync(loginDetails.Email, loginDetails.Password, true, false);
            if (result.Succeeded)
            {
                // Resolve the user via their email
                user = await _userManager.FindByEmailAsync(loginDetails.Email);
            }

            return user;

        }
    }

}