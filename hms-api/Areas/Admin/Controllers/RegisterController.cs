using System;
using System.Threading.Tasks;
using HMS.Areas.Patient.Dtos;
using HMS.Models;
using HMS.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using AutoMapper;
using HMS.Areas.Patient.Models;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class RegisterController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRegister _adminRepo;

        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper, IRegister adminRepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _adminRepo = adminRepo;
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<Object> RegisterAsync([FromBody]RegisterViewModel registerDetails)
        {

            var response = await RegisterUserAsync(registerDetails);
            return response;
        }

        [HttpPost("PatientOnBoarding")]
        public async Task<IActionResult> OnBoardPatient(PatientDtoForCreate patientToCreate)
        {
            try
            {
                if (patientToCreate == null)
                    return BadRequest();
                Models.Account createdAccount = null;
                //create account fer patient
                if (string.IsNullOrEmpty(patientToCreate.AccountId))
                {
                    var accountToCreate = _mapper.Map<Models.Account>(patientToCreate);

                    createdAccount = await _adminRepo.InsertAccount(accountToCreate);

                    if (createdAccount == null)
                        return BadRequest(new { message = "Account failed to create", status = false });
                }
                else
                {
                    createdAccount = await _adminRepo.GetAccountById(patientToCreate.AccountId);

                    if (createdAccount == null)
                    {
                        return NotFound(new { message = "Account not found", status = "false" });
                    }
                }

                //create file number
                var fileToCreate = _mapper.Map<FileDtoForCreate>(createdAccount);

                var filecreated = await _adminRepo.GenerateFileNumber(fileToCreate);

                if (filecreated == null)
                    return BadRequest(new { message = "file failed to create", status = false });

                patientToCreate.AccountId = createdAccount.Id;
                patientToCreate.FileNumber = filecreated.FileNumber;

                var patient = _mapper.Map<PatientProfile>(patientToCreate);

                var res = await _adminRepo.InsertPatient(patient);
                if (!res)
                {
                    return BadRequest(new { message = "Error occured while creating patient", status = false });
                }

                return CreatedAtRoute("Patients", patientToCreate);
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