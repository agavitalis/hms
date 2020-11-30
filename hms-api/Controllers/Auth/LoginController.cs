using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HMS.Models;
using HMS.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using MailKit;

namespace HMS.Controllers.Auth
{
    [Route("api/Auth")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUser _user;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public LoginController(IConfiguration config, IUser user, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _user = user;
        }



        [Route("Login")]
        [HttpPost]
        public async Task<Object> LoginAsync([FromBody] LoginViewModel loginDetails)
        {

            var authenticatedUser = await AuthenticateUserAsync(loginDetails);
            object response;

            if (authenticatedUser != null)
            {
                try
                {
                    var tokenString = await GenerateJSONWebTokenAsync(authenticatedUser);
                    response = Ok(new
                    {
                        authenticatedUser,
                        token = tokenString
                    }); ;
                }
                catch (Exception ex)
                {
                    response = BadRequest(new
                    {

                        error = ex.Message
                    });

                }

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

        [Route("SendForgotPasswordMail")]
        [HttpPost]
        public async Task<IActionResult> SendForgotPasswordEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid Email" });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                string url = $"{ _config["AppURL"]}/ResetPassword?email={email}&token={validToken}";


                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("HMS", "jcuudeh@gmail.com"));
                message.To.Add(new MailboxAddress("ugochukwu", "jcuudeh@gmail.com"));
                message.Subject = "HMS Reset Password";
                message.Body = new TextPart("html")
                {
                    Text = "<a href=" + url + ">Click here</a>"
                };
             
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {

                    client.Connect("smtp.gmail.com", 587, false);

                    //SMTP server authentication if needed
                    client.Authenticate("jcuudeh@gmail.com", "N0vember30");

                    client.Send(message);

                    client.Disconnect(true);
                    return Ok(new { message = "reset password email has been sent successfully" });
                };
            }

          
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error occured");
            }
        }

        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePasswordViewModel password)
        {
            if (ModelState.IsValid)
            {
                var user = await _user.GetUserByIdAsync(password.UserId);

                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId" });
                }

                var res = await _userManager.ChangePasswordAsync(user, password.CurrentPassword, password.NewPassword);
                
                if (res.Succeeded == true)
                {
                    return Ok(new
                    {
                        message = "Password Changed Successfully"
                    });
                }

                var errorMessage = "";
                foreach (var item in res.Errors)
                {
                    errorMessage = item.Description;
                }
                
                return BadRequest(new { message = errorMessage + "There Was An Error Try Again" });
            }
            else
            {
                return BadRequest(new { message = "Incomplete details" });
            }
            
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