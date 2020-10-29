using System;
using System.Threading.Tasks;
using HMS.Models;
using HMS.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using AutoMapper;
using HMS.Areas.Admin.Models;
using HMS.Database;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class RegisterController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRegister _registerRepo;
        private readonly IAccount _accountRepo;
        private readonly ApplicationDbContext _applicationDbContext;

        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper, IRegister registerRepo, IAccount accountRepo ,ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _registerRepo = registerRepo;
            _accountRepo = accountRepo;
            _applicationDbContext = applicationDbContext;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<Object> RegisterAsync([FromBody]RegisterViewModel registerDetails)
        {

            var response = await RegisterUserAsync(registerDetails);
            return response;
        }

       
        [HttpPost]
        [HttpPost("RegisterPatient")]
        public async Task<IActionResult> OnBoardPatient(DtoForPatientRegistration patientToRegister)
        {
            try
            {
                if (patientToRegister == null)
                {
                    return BadRequest();
                }

                Account accountToCreate = null;

                //check if this is a personnal account
                if(string.IsNullOrEmpty(patientToRegister.AccountId))
                {
                    //then create a personal account for him and get me back the ID
                    accountToCreate = new Account()
                    {
                        Name = patientToRegister.LastName + patientToRegister.FirstName,
                        HealthPlanId = patientToRegister.HealthPlanId,
                    };

                    _applicationDbContext.Accounts.Add(accountToCreate);
                    await _applicationDbContext.SaveChangesAsync();

                    //then assign this Account Id to the Payload
                    patientToRegister.AccountId = accountToCreate.Id;
                }
                else
                {
                    accountToCreate = await _accountRepo.GetAccountByIdAsync(patientToRegister.AccountId);

                    if (accountToCreate == null)
                    {
                        return NotFound(new { message = "This Patient Account was not Found..Are you registering him as a Single Account?", success = "false" });
                    }
                }


                //proceed to create file and patient account
                var fileCreated = await _registerRepo.CreateFile(patientToRegister.AccountId);

                if (fileCreated == null)
                {
                    return BadRequest(new { message = "File Number Generation Failed, Patient Cannot be Registered", success = false });
                }
                   

                var patient = _mapper.Map<ApplicationUser>(patientToRegister);
                var response = await _registerRepo.RegisterPatient(patient,fileCreated,accountToCreate);

                if (!response)
                {
                    return BadRequest(new { message = "Error occured while creating patient", status = false });
                }

                return Ok(new { 

                    patientToRegister,
                    message = "Patient Successfuly Created"
                });
            }
            catch (Exception)
            {

                throw;
            }
        }




        [NonAction]
        private async Task<Object> RegisterUserAsync(RegisterViewModel registerDetails)
        {

            // Ensure that this given email have not been used
            var user = await _userManager.FindByEmailAsync(registerDetails.Email);

            if (user == null)
            {
                //check if the roles this guy entered exist
                if (await _roleManager.RoleExistsAsync(registerDetails.RoleName))
                {
                    var newApplicationUser = new ApplicationUser()
                    {
                        FirstName = registerDetails.FirstName,
                        LastName = registerDetails.LastName,
                        Email = registerDetails.Email,
                        UserName = registerDetails.Email,
                        UserType = registerDetails.RoleName
                    };

                    var result = await _userManager.CreateAsync(newApplicationUser, registerDetails.Password);
                    if (result.Succeeded)
                    {
                        //assign him to this role
                        await _userManager.AddToRoleAsync(newApplicationUser, registerDetails.RoleName);

                        return Ok(new
                        {
                            response = 200,
                            newApplicationUser,
                            message = "User Successfully Created"
                        });

                       
                    }

                    else
                    {
                        return NotFound( new
                        {
                            response = 400,
                            message = "User Could not be created"
                        });
                    }

                }
                else
                {
                    return NotFound(new
                    {
                        response = 400,
                        message = "The specified user role does not exist in our system"
                    });

                   
                }

            }
            else
            {
                return BadRequest(new
                {
                    response = 301,
                    message = "A User with this Email Already Exist"
                });
            }

        }


    }
}