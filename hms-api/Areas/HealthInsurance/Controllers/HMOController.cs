using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HMS.Models;
using HMS.Areas.NHIS.Dtos;
using AutoMapper;
using HMS.Services.Helpers;
using Newtonsoft.Json;
using HMS.Areas.Admin.Interfaces;
using HMS.Areas.HealthInsurance.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Configuration;
using HMS.Services.Interfaces;

namespace HMS.Areas.NHIS.Controllers
{
    [Route("api/HealthInsurance", Name = "Health Insurance - Manage HMOS")]
    [ApiController]
    public class HMOController : Controller
    {
        private readonly IHMO _HMO;
        private readonly IMapper _mapper;
        private readonly IHealthPlan _healthPlan;
       
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public HMOController(IHMO HMO, IHealthPlan healthPlan, IMapper mapper, IConfiguration config, IEmailSender emailSender, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _HMO = HMO;
            _healthPlan = healthPlan;
            _mapper = mapper;
            _config = config;
            _emailSender = emailSender;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("GetHMO")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs(string HMOId)
        {
            if (string.IsNullOrEmpty(HMOId))
            {
                return BadRequest(new { message = "Invalid Post Attempt" });
            }
            var HMO = await _HMO.GetHMO(HMOId);

            if (HMO == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HMOId" });
            }
            return Ok(new
            {
                HMO,
                message = "HMO Fetched"
            });
        }

        [Route("GetHMOs")]
        [HttpGet]
        public async Task<IActionResult> GetHMOs([FromQuery] PaginationParameter paginationParameter)
        {
            var HMOs = _HMO.GetHMOs(paginationParameter);

            var paginationDetails = new
            {
                HMOs.TotalCount,
                HMOs.PageSize,
                HMOs.CurrentPage,
                HMOs.TotalPages,
                HMOs.HasNext,
                HMOs.HasPrevious
            };


            //This is optional
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

            return Ok(new
            {
                HMOs,
                paginationDetails,
                message = "HMOs Fetched"
            });
        }
           

        [HttpPost("CreatHMO")]
        public async Task<IActionResult> CreatHMO(HMODtoForCreate HMO)
        {
           
            var profile = new HMOAdminProfile();

            if (HMO == null)
            {
                return BadRequest(new { message = "Invalid post attempt" });
            }

            var healthPlan = await _healthPlan.GetHealthPlanByIdAsync(HMO.HealthPlanId);
            
            if (healthPlan == null)
            {
                return BadRequest(new { response = "301", message = "Invalid HealthPlanId" });
            }
            
            var user = await _userManager.FindByEmailAsync(HMO.Email);

            if (user != null)
            {
                return BadRequest(new { message = "User Already Exists" });
            }

            if (await _roleManager.RoleExistsAsync(HMO.RoleName) == false)
            {
                return BadRequest(new { message = "Role Does Not Exist" });
            }

            var HMOToCreate = _mapper.Map<HMO>(HMO);

            var res = await _HMO.CreateHMO(HMOToCreate);
            if (!res)
            {
                return BadRequest(new { response = "301", message = "HMO failed to create" });
            }

            var newUser = await _HMO.CreateUser(HMO.FirstName, HMO.LastName, HMO.Email, HMO.RoleName);

            //then get him a profile
            if (newUser == null)
            {
                return BadRequest(new { response = "301", message = "Something Went Wrong" });
            }

            var result = await _HMO.CreateUserProfile(newUser.Id, newUser.FirstName, newUser.LastName, HMOToCreate.Id);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string emailSubject = "HMS Confirm Email";
            string url = $"{_config["AppURL"]}?email={HMO.Email}&token={validToken}";
            string emailContent = "<p>To confirm your Email <a href=" + url + ">Click here</a>";
            var message = new EmailMessage(new string[] { HMO.Email }, emailSubject, emailContent);
            _emailSender.SendEmail(message);


            return Ok(new
            {
                message = "HMO created successfully"
            });
        } 
    }
}
