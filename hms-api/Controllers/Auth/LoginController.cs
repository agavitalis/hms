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
using MailKit.Net.Smtp;
using System.Linq;

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
        private readonly IEmailSender _emailSender;


        public LoginController(IConfiguration config, IUser user, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
            _user = user;
            _emailSender = emailSender;
        }



        [Route("Login")]
        [HttpPost]
        public async Task<Object> LoginAsync([FromBody] LoginViewModel loginDetails)
        {

            var authenticatedUser = await AuthenticateUserAsync(loginDetails);
            var tokenString = "";

            if (authenticatedUser != null)
            {
                try
                {
                    var emailConfirmed = await _userManager.IsEmailConfirmedAsync(authenticatedUser);
                    if (emailConfirmed)
                    {
                        tokenString = await GenerateJSONWebTokenAsync(authenticatedUser);
                        return Ok(new { authenticatedUser, token = tokenString }); 
                    }
                    tokenString = await GenerateJSONWebTokenAsync(authenticatedUser);
                    return Ok(new { authenticatedUser, token = tokenString, EmailConfirmationStatus = false });
                }
                catch (Exception ex)
                {
                   return BadRequest(new { error = ex.Message });

                }
            }
            else
            {
                return BadRequest(new { response = 301, message = "Invalid Login Credentials"});
            }
        }

        [Route("SendResetPasswordMail")]
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
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
                string emailSubject = "HMS Reset Password";
                string url = $"{ _config["AppURL"]}/ResetPassword?email={email}&token={validToken}";
                string emailContent = "<p>To reset your password <a href=" + url + ">Click here</a>";
                var message = new EmailMessage(new string[] { email }, emailSubject, emailContent);
                _emailSender.SendEmail(message);
                return Ok(new { message = "Reset Password Email Has Been Sent Successfully" });
            }

          
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel password)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(password.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId" });
                }

                
                var token = await _userManager.ChangePasswordAsync(user, password.CurrentPassword , password.NewPassword);

                if (token.Succeeded)
                {
                    return Ok(new { message = "Password Changed Successfully" });
                }

                return BadRequest(new { message = token.Errors });
            }


            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ResetPasswordViewModel password)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _user.GetUserByEmailAsync(password.Email);

                if (user == null)
                {
                    return BadRequest(new { message = "Invalid Email" });
                }
                var encodedToken = WebEncoders.Base64UrlDecode(password.AuthenticationToken);
                var token = Encoding.UTF8.GetString(encodedToken);
         
                if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
                {
                    return BadRequest(new { message = "Invalid Authentication Token" });
                }
                var res = await _userManager.ResetPasswordAsync(user, token, password.NewPassword);
                
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