using System;
using System.Threading.Tasks;
using HMS.Models;
using HMS.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Interfaces;
using AutoMapper;
using HMS.Database;
using HMS.Areas.Patient.Interfaces;
using HMS.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Configuration;
using HMS.Services.Helpers;
using Newtonsoft.Json;

namespace HMS.Areas.Admin.Controllers
{
    [Route("api/Admin", Name = "Admin - Onboarding")]
    [ApiController]
    public class RegisterController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRegister _registerRepo;
        private readonly IAccount _accountRepo;
        private readonly IPatientProfile _patientRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _config;
        private readonly IUser _user;
        private readonly IEmailSender _emailSender;

        public RegisterController(IConfiguration config, IUser user, IEmailSender emailSender, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper, IRegister registerRepo, IAccount accountRepo, IPatientProfile patientRepository, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _registerRepo = registerRepo;
            _accountRepo = accountRepo;
            _patientRepository = patientRepository;
            _applicationDbContext = applicationDbContext;
            _config = config;
            _user = user;
            _emailSender = emailSender;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<Object> RegisterAsync([FromBody] RegisterViewModel registerDetails)
        {

            var response = await RegisterUserAsync(registerDetails);
            return response;
        }


        [HttpPost]
        [Route("RegisterPatient")]
        public async Task<IActionResult> OnBoardPatient(DtoForPatientRegistration patientToRegister)
        {
            var patient = new ApplicationUser();
            var patientProfile = new PatientProfile();
            var registrationInvoice = new RegistrationInvoice();
            Account accountToCreate = null;
            try
            {
                if (patientToRegister == null)
                {
                    return BadRequest();
                }

                //check if this is a personnal account
                if (string.IsNullOrEmpty(patientToRegister.AccountId))
                {
                    //then create a personal account for him and get me back the ID
                    accountToCreate = new Account()
                    {
                        Name = $"{patientToRegister.LastName} {patientToRegister.FirstName}",
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

                //return Ok(new { patientToRegister, accountToCreate, fileCreated });

                if (fileCreated == null)
                {
                    return BadRequest(new { message = "File Number Generation Failed, Patient Cannot be Registered", success = false });
                }

                Guid newGuid = new Guid();
                patient = _mapper.Map<ApplicationUser>(patientToRegister);
                var response = await _registerRepo.RegisterPatient(patient, fileCreated, accountToCreate);
                if (!Guid.TryParse(response, out newGuid))
                {
                    return BadRequest(new { message = response, status = false });
                }

                patient = await _user.GetUserByIdAsync(response);
                patientProfile = await _patientRepository.GetPatientByIdAsync(response);
                var amount = patientProfile.Account.HealthPlan.Cost;
                var res = _mapper.Map<RegistrationInvoice>(patientToRegister);
                registrationInvoice = await _registerRepo.GenerateRegistrationInvoice(amount, patientProfile.Account.HealthPlan.Id, patientToRegister.InvoiceGeneratedBy, patientProfile.PatientId);



                var token = await _userManager.GenerateEmailConfirmationTokenAsync(patient);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);

               
                string emailSubject = "HMS Confirm Email";
                string url = $"{ _config["AppURL"]}?email={patientToRegister.Email}&token={validToken}";
                string emailContent = "<p>To confirm your Email <a href=" + url + ">Click here</a>";
                var message = new EmailMessage(new string[] { patientToRegister.Email }, emailSubject, emailContent);
                _emailSender.SendEmail(message);              

                return Ok(new
                {
                    patient,
                    message = "Patient Successfully Created. An Email Has been sent to the Email Address"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    patient.Id,
                    patient.FirstName,
                    patient.LastName,
                    patient.OtherNames,
                    patient.Email,
                    patient.EmailConfirmed,
                    patient.PhoneNumber,
                    patient.PhoneNumberConfirmed,
                    patient.ProfileImageUrl,
                    patient.UserName,
                    patient.UserType,
                    message = "Patient Successfuly Created",
                    emailMessage = ex.Message.ToString()
                }); ;
            }
        }

        [Route("VerifyEmail")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ConfirmEmailViewModel email)
        {
            if (ModelState.IsValid)
            {
               

                var encodedToken = WebEncoders.Base64UrlDecode(email.AuthenticationToken);
                var token = Encoding.UTF8.GetString(encodedToken);
                var user = await _user.GetUserByEmailAsync(email.Email);

                if (user == null)
                {
                    return BadRequest(new { message = "Invalid Email" });
                }
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    if (await _userManager.IsEmailConfirmedAsync(user))
                    {
                        return Ok(new
                        {
                            message = "Email Has Been Confirmed"
                        });
                    }
                    else { return BadRequest(new { message = "Email Has Not Been Confirmed" }); }
                }
                return BadRequest(new { message = "Email Verification Failed" });
            }
            else
            {
                return BadRequest(new { message = "Incomplete details" });
            }

        }

        [HttpGet]
        [Route("GetPatientRegistrationInvoice")]
        public async Task<IActionResult> GetPatientRegistrationInvoice(string patientId)
        {
            try
            {
                if (patientId == null)
                {
                    return BadRequest();
                }

                var patientRegistrationInvoice = await _registerRepo.GetPatientRegistrationInvoice(patientId);
                if (patientRegistrationInvoice == null)
                {
                    return NotFound();
                }

                return Ok(new { patientRegistrationInvoice, message = "Registration Invoice returned" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("PayPatientRegistrationFee")]
        public async Task<IActionResult> PayPatientRegistrationFee(PatientRegistrationPaymentDto paymentDetails)
        {
            try
            {
                if (paymentDetails == null)
                {
                    return BadRequest();
                }

                var res = await _registerRepo.PayRegistrationFee(paymentDetails);
                if (res == 0)
                {
                    return Ok(new { paymentDetails, mwessage = "Payment Succesful" });
                }
                if (res == 1)
                {
                    return BadRequest(new { mwessage = "Invalid Amount" });
                }
                if (res == 2)
                {
                    return BadRequest(new { mwessage = "Failed to update invoice" });
                }
                if (res == 3)
                {
                    return BadRequest(new { mwessage = "Invalid Invoice Number" });
                }
                if (res == 4)
                {
                    return BadRequest(new { mwessage = "Invalid PatientId" });
                }
                if (res == 5)
                {
                    return BadRequest(new { mwessage = "Invalid InitiatorId" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("PayPatientRegistrationFeeWithAccount")]
        public async Task<IActionResult> PayPatientRegistrationFeeWithAccount(PatientRegistrationPaymentDto paymentDetails)
        {
            try
            {
                if (paymentDetails == null)
                {
                    return BadRequest();
                }

                var res = await _registerRepo.PayRegistrationFeeWithAccount(paymentDetails);
                if (res == 0)
                {
                    return Ok(new { paymentDetails, message = "Payment Succesful" });
                }
                if (res == 1)
                {
                    return BadRequest(new { message = "Invalid Amount" });
                }
                if (res == 2)
                {
                    return BadRequest(new { message = "Failed to update invoice" });
                }
                if (res == 3)
                {
                    return BadRequest(new { message = "Invalid Invoice Number" });
                }
                if (res == 4)
                {
                    return BadRequest(new { message = "Invalid PatientId" });
                }
                if (res == 5)
                {
                    return BadRequest(new { message = "Account Balance Is Less Than Amount Specified" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetRegistrationFeeInvoice")]
        public async Task<IActionResult> GetRegistrationFeeInvoice(string patientId)
        {
            try
            {
                if (patientId == null)
                {
                    return BadRequest();
                }

                var patient = await _patientRepository.GetPatientByIdAsync(patientId);

                if(patient == null)
                {
                    return BadRequest(new { error = "Invalid patient Id" });
                }

                var registrationInvoice = await _registerRepo.GetRegistrationInvoice(patientId);

                return Ok(new { registrationInvoice, message = "Registration Fee Invoice" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });

            }
        }

        //[HttpGet]
        //[Route("GetRegistrationFeeInvoices")]
        //public async Task<IActionResult> GetRegistrationFeeInvoices()
        //{
        //    try
        //    {
        //        var registrationInvoices = await _registerRepo.GetRegistrationInvoices();

        //        return Ok(new { registrationInvoices, message = "Registration Fee Invoices" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}

        [HttpGet]
        [Route("GetRegistrationFeeInvoices")]
        public async Task<IActionResult> GetRegistrationFeeInvoices([FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var registrationInvoices = _registerRepo.GetRegistrationInvoicesPagnation(paginationParameter);

                var paginationDetails = new
                {
                    registrationInvoices.TotalCount,
                    registrationInvoices.PageSize,
                    registrationInvoices.CurrentPage,
                    registrationInvoices.TotalPages,
                    registrationInvoices.HasNext,
                    registrationInvoices.HasPrevious
                };

                //This is optional
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationDetails));

                return Ok(new
                {
                    registrationInvoices,
                    paginationDetails,
                    message = "Appointments Fetched"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [NonAction]
        private async Task<Object> RegisterUserAsync(RegisterViewModel registerDetails)
        {
            var newApplicationUser = new ApplicationUser();
            try
            {
                var user = await _userManager.FindByEmailAsync(registerDetails.Email);

                if (user == null)
                {
                    //check if the roles this guy entered exist
                    if (await _roleManager.RoleExistsAsync(registerDetails.RoleName))
                    {
                        newApplicationUser = new ApplicationUser()
                        {
                            FirstName = registerDetails.FirstName,
                            LastName = registerDetails.LastName,
                            Email = registerDetails.Email,
                            UserName = registerDetails.Email,
                            UserType = registerDetails.RoleName
                        };

                        var result = await _userManager.CreateAsync(newApplicationUser, "Password1@test");
                        if (result.Succeeded)
                        {
                            //assign him to this role
                            await _userManager.AddToRoleAsync(newApplicationUser, registerDetails.RoleName);
                            //then get him a profile
                            if (registerDetails.RoleName == "Admin" || registerDetails.RoleName == "admin")
                            {
                                var profile = new AdminProfile()
                                {
                                    AdminId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.AdminProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                            if (registerDetails.RoleName == "Doctor" || registerDetails.RoleName == "doctor")
                            {
                                var profile = new DoctorProfile()
                                {
                                    DoctorId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.DoctorProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                            if (registerDetails.RoleName == "HMOAdmin" || registerDetails.RoleName == "hmoadmin")
                            {
                                var profile = new HMOAdminProfile()
                                {
                                    HMOAdminId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.HMOAdminProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }

                            if (registerDetails.RoleName == "Nurse" || registerDetails.RoleName == "nurse")
                            {
                                var profile = new NurseProfile()
                                {
                                    NurseId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.NurseProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }

                            if (registerDetails.RoleName == "Accountant" || registerDetails.RoleName == "accountant")
                            {
                                var profile = new AccountantProfile()
                                {
                                    AccountantId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.AccountantProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }

                            if (registerDetails.RoleName == "Pharmacy" || registerDetails.RoleName == "pharmacy")
                            {
                                var profile = new PharmacyProfile()
                                {
                                    PharmacistId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.PharmacyProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }

                            if (registerDetails.RoleName == "Lab" || registerDetails.RoleName == "lab")
                            {
                                var profile = new LabProfile()
                                {
                                    LabAttendantId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.LabProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                            if (registerDetails.RoleName == "WardPersonnel" || registerDetails.RoleName == "wardpersonnel")
                            {
                                var profile = new WardPersonnelProfile()
                                {
                                    WardPersonnelId = newApplicationUser.Id,
                                    FullName = $"{newApplicationUser.FirstName} {newApplicationUser.LastName}"
                                };
                                _applicationDbContext.WardPersonnelProfiles.Add(profile);
                                await _applicationDbContext.SaveChangesAsync();
                            }
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newApplicationUser);
                            var encodedToken = Encoding.UTF8.GetBytes(token);
                            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                            string emailSubject = "HMS Confirm Email";
                            string url = $"{_config["AppURL"]}?email={registerDetails.Email}&token={validToken}";
                            string emailContent = "<p>To confirm your Email <a href=" + url + ">Click here</a>";
                            var message = new EmailMessage(new string[] { registerDetails.Email }, emailSubject, emailContent);
                            _emailSender.SendEmail(message);

                            return Ok(new
                            {
                                response = 200,
                                newApplicationUser.Id,
                                newApplicationUser.FirstName,
                                newApplicationUser.LastName,
                                newApplicationUser.OtherNames,
                                newApplicationUser.Email,
                                newApplicationUser.EmailConfirmed,
                                newApplicationUser.PhoneNumber,
                                newApplicationUser.PhoneNumberConfirmed,
                                newApplicationUser.ProfileImageUrl,
                                newApplicationUser.UserName,
                                newApplicationUser.UserType,
                                message = "User Successfully Created. An Email Has been sent to the Email Address"
                            });


                        }

                        else
                        {
                            return NotFound(new
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
            catch (Exception ex)
            {
                return Ok(new
                {
                    newApplicationUser.Id,
                    newApplicationUser.FirstName,
                    newApplicationUser.LastName,
                    newApplicationUser.OtherNames,
                    newApplicationUser.Email,
                    newApplicationUser.EmailConfirmed,  
                    newApplicationUser.PhoneNumber,
                    newApplicationUser.PhoneNumberConfirmed,
                    newApplicationUser.ProfileImageUrl,
                    newApplicationUser.UserName,
                    newApplicationUser.UserType,
                    message = "User Successfuly Created",
                    emailMessage = ex.Message.ToString()
                }); 
            }         
        }
    }
}