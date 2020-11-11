using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMS.Models;
using HMS.Areas.Doctor.Controllers;
using HMS.Areas.Doctor.Interfaces;
using HMS.Areas.Doctor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static HMS.Areas.Doctor.ViewModels.DoctorProfilePictureViewModel;
using HMS.Services.Helpers;
using HMS.Areas.Doctor.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace HMS.Areas.Doctor.Controllers
{
    [Route("api/Doctor", Name = "Doctor- Manage Profile")]
    [ApiController]
    public class DoctorProfileController : Controller
    {
        private readonly IDoctorProfile _doctorProfile;

        public DoctorProfileController(IDoctorProfile doctorProfile)
        {
            _doctorProfile = doctorProfile;
        }

        [Route("GetDoctors")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync()
        {

            var doctors = await _doctorProfile.GetDoctorsAsync();

            if (doctors != null)
            {
                return Ok(new
                {
                    doctors
                });
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Doctor Id"
                });
            }

        }

        [Route("GetDoctor")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorByIdAsync(string DoctorId)
        {

            var doctorProfile = await _doctorProfile.GetDoctorAsync(DoctorId);

            return Ok(new
            {
                doctorProfile
            });

        }

        [Route("EditDoctorProfilePicture")]
        [HttpPost]
        public async Task<IActionResult> EditDoctorProfilePictureAsync([FromBody] DoctorProfilePictureViewModel DoctorProfile)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorProfilePictureAsync(DoctorProfile))
                {
                    return Ok(new
                    {
                        message = "Profile picture Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 400,
                        message = "Failed to Update profile picture"
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    response = 400,
                    message = "Invalid Parameters"
                });
            }
        }


        [HttpPost]
        [Route("UpdateDoctorBasicInfo")]
        public async Task<IActionResult> EditDoctorBasicInfoAsync([FromBody] EditDoctorBasicInfoViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorBasicInfoAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Doctor Basic Info Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        [HttpPost]
        [Route("UpdateDoctorContactDetails")]
        public async Task<IActionResult> EditDoctorContactAsync([FromBody] DoctorContactViewModel doctor)
        {
            if (ModelState.IsValid)
            {
                if (await _doctorProfile.EditDoctorContactAsync(doctor))
                {
                    return Ok(new
                    {
                        message = "Doctor Address Successfully Saved"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        response = 301,
                        message = "Failed to insert patient details"
                    });
                }
            }
            return BadRequest(new { message = "Incomplete details" });
        }

        //[HttpPost]
        //[Route("UpdateDoctorProfessionalDetails")]
        //public async Task<IActionResult> EditDoctorProfessionalDetails([FromBody] DoctorProfessionalDetailsViewModel doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (await _doctorProfile.EditDoctorProfessionalDetailsAsync(doctor))
        //        {
        //            return Ok(new
        //            {
        //                message = "Doctor Professional Details Successfully Saved"
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new
        //            {
        //                response = 301,
        //                message = "Failed to insert  details"
        //            });
        //        }
        //    }
        //    return BadRequest(new { message = "Incomplete Details" });
        //}

        //[HttpPost]
        //[Route("UpdateDoctorAvaliabilityDetails")]
        //public async Task<IActionResult> EditDoctorAvaliablityDetails([FromBody] DoctorAvaliablityViewModel doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (await _doctorProfile.EditDoctorAvaliabilityAsync(doctor))
        //        {
        //            return Ok(new
        //            {
        //                message = "Avaliability Details Successfully Updated"
        //            });
        //        }
        //        else
        //        {
        //            return BadRequest(new
        //            {
        //                response = 301,
        //                message = "Failed to update  details"
        //            });
        //        }
        //    }
        //    return BadRequest(new { message = "Incomplete Details" });
        //}

        [HttpGet("GetDoctorEducation/{Id}")]
        public async Task<IActionResult> GetDoctorEducationById(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorEducationById(Id);
            if(res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }
        
        [HttpGet("GetDoctorExperience/{Id}")]
        public async Task<IActionResult> GetDoctorExperienceById(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorExperienceById(Id);
            if(res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }    
        
        [HttpGet("GetDoctorOfficeTime/{Id}")]
        public async Task<IActionResult> GetDoctorOfficeTimeById(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorOfficeTimeById(Id);
            if(res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }      
        
        [HttpGet("GetDoctorSocial/{Id}")]
        public async Task<IActionResult> GetDoctorSocialById(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorSocialById(Id);
            if(res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }
        
        [HttpGet("GetDoctorSkill/{Id}")]
        public async Task<IActionResult> GetDoctorSkillById(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorSkillById(Id);
            if(res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }

        [HttpGet("GetDoctorEducationWithDoctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorEducationByDoctorId(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorEductions(doctorId);
            if (res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }

        [HttpGet("GetDoctorExperiencewithDoctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorExperienceByDoctorId(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorExperience(doctorId);
            if (res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }

        [HttpGet("GetDoctorOfficeTimeWithdoctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorOfficeTimeByDoctorId(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorOfficeTime(doctorId);
            if (res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }

        [HttpGet("GetDoctorSocialwithDoctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorSocialByDoctorId(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorSocial(doctorId);
            if (res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }
        
        [HttpGet("GetDoctorSkillsWithDoctorId/{doctorId}")]
        public async Task<IActionResult> GetDoctorSkillsWithDoctorId(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.GetDoctorSkills(doctorId);
            if (res is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to get Data, an error occured"
                });

            return Ok(new
            {
                res,
                message = "Successful"
            });
        }

        [HttpPost("AddDoctorEductions")]
        public async Task<IActionResult> AddEductions(IEnumerable<DoctorEducationDtoForCreate> doctorEducations)
        {
            if(!doctorEducations.Any())
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.AddDoctorEducation(doctorEducations);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to add doctor education"
                });

            return Ok(new
            {
                message = "Successfully Created"
            });
        }
        
        [HttpPost("AddDoctorExperience")]
        public async Task<IActionResult> AddExperince(IEnumerable<DoctorExperienceDtoForCreate> doctorExperiences)
        {
            if(!doctorExperiences.Any())
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.AddDoctorExperience(doctorExperiences);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to add doctor experience"
                });

            return Ok(new
            {
                message = "Successfully Created"
            });
        } 
        
        [HttpPost("AddDoctorOfficeTime")]
        public async Task<IActionResult> AddOfficalTime(IEnumerable<DoctorOfficeTimeDtoForCreate> doctorOfficeTime)
        {
            if(!doctorOfficeTime.Any())
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.AddDoctorOfficeTime(doctorOfficeTime);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to add doctor office time"
                });

            return Ok(new
            {
                message = "Successfully Created"
            });
        }
        
        [HttpPost("AddDoctorSocial")]
        public async Task<IActionResult> AddSocials(IEnumerable<DoctorSocialDtoForCreate> doctorSocials)
        {
            if(!doctorSocials.Any())
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.AddDoctorSocial(doctorSocials);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to add doctor socials"
                });

            return Ok(new
            {
                message = "Successfully Created"
            });
        }
        
        [HttpPost("AddDoctorSkill")]
        public async Task<IActionResult> AddSkill(IEnumerable<DoctorSkillsDtoForCreate> doctorSkills)
        {
            if(!doctorSkills.Any())
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.AddDoctorSkills(doctorSkills);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Failed to add doctor socials"
                });

            return Ok(new
            {
                message = "Successfully Created"
            });
        }

        [HttpPatch("EditDoctorEducation/{doctorEductionId}")]
        public async Task<IActionResult> UpdateDoctorEducation(string doctorEductionId, JsonPatchDocument<DoctorEducationDtoForView> jsonPatch)
        {
            if(string.IsNullOrEmpty(doctorEductionId)|| jsonPatch is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _doctorProfile.EditDoctorEduction(doctorEductionId, jsonPatch);
            if(!result)
                return BadRequest(new
                {
                    response = 301,
                    message = "Update operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpPatch("EditDoctorExperience/{doctorExperienceId}")]
        public async Task<IActionResult> UpdateDoctorExperience(string doctorExperienceId, JsonPatchDocument<DoctorExperienceDtoForView> jsonPatch)
        {
            if(string.IsNullOrEmpty(doctorExperienceId) || jsonPatch is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _doctorProfile.EditDoctorExperience(doctorExperienceId, jsonPatch);
            if(!result)
                return BadRequest(new
                {
                    response = 301,
                    message = "Update operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpPatch("EditDoctorOfficeTime/{doctorOfficeTimeId}")]
        public async Task<IActionResult> UpdateDoctorOfficeTime(string doctorOfficeTimeId, JsonPatchDocument<DoctorOfficeTimeDtoForView> jsonPatch)
        {
            if(string.IsNullOrEmpty(doctorOfficeTimeId) || jsonPatch is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _doctorProfile.EditDoctorOfficeTime(doctorOfficeTimeId, jsonPatch);
            if(!result)
                return BadRequest(new
                {
                    response = 301,
                    message = "Update operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpPatch("EditDoctorSocial/{doctorSocialId}")]
        public async Task<IActionResult> UpdateDoctorSocial(string doctorSocialId, JsonPatchDocument<DoctorSocialDtoForView> jsonPatch)
        {
            if(string.IsNullOrEmpty(doctorSocialId) || jsonPatch is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _doctorProfile.EditDoctorSocials(doctorSocialId, jsonPatch);
            if(!result)
                return BadRequest(new
                {
                    response = 301,
                    message = "Update operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpPatch("EditDoctorSkill/{doctorSkillId}")]
        public async Task<IActionResult> UpdateDoctorSo(string doctorSkillId, JsonPatchDocument<DoctorSkillsDtoForView> jsonPatch)
        {
            if(string.IsNullOrEmpty(doctorSkillId) || jsonPatch is null)
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var result = await _doctorProfile.EditDoctorSkill(doctorSkillId, jsonPatch);
            if(!result)
                return BadRequest(new
                {
                    response = 301,
                    message = "Update operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }

        [HttpDelete("DeleteDoctorEducation/{doctorEductionId}")]
        public async Task<IActionResult> DeleteDoctorEducation(string doctorEductionId)
        {
            if(string.IsNullOrEmpty(doctorEductionId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.DeleteDoctorEduction(doctorEductionId);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Delete operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpDelete("DeleteDoctorExperience/{doctorExperienceId}")]
        public async Task<IActionResult> DeleteDoctorExperience(string doctorExperienceId)
        {
            if(string.IsNullOrEmpty(doctorExperienceId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.DeleteDoctorExperience(doctorExperienceId);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Delete operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
           
        [HttpDelete("DeleteDoctorOfficeTime/{doctorOfficeTimeId}")]
        public async Task<IActionResult> DeleteDoctorOfficeTime(string doctorOfficeTimeId)
        {
            if(string.IsNullOrEmpty(doctorOfficeTimeId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.DeleteDoctorOfficeTime(doctorOfficeTimeId);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Delete operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
         
        [HttpDelete("DeleteDoctorSocial/{doctorSocialId}")]
        public async Task<IActionResult> DeleteDoctorSocial(string doctorSocialId)
        {
            if(string.IsNullOrEmpty(doctorSocialId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.DeleteDoctorSocial(doctorSocialId);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Delete operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }
        
        [HttpDelete("DeleteDoctorSkill/{doctorSkillId}")]
        public async Task<IActionResult> DeleteDoctorSkill(string doctorSkillId)
        {
            if(string.IsNullOrEmpty(doctorSkillId))
                return BadRequest(new
                {
                    response = 301,
                    message = "Invalid request"
                });

            var res = await _doctorProfile.DeleteDoctorSkills(doctorSkillId);
            if(!res)
                return BadRequest(new
                {
                    response = 301,
                    message = "Delete operation Failed, Try Again"
                });

            return Ok(new
            {
                message = "Update Successful"
            });
        }

    }
}