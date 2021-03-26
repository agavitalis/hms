﻿using HMS.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HMS.Areas.Admissions.Dtos
{
    public class WardPersonnelDtoForView
    {
        public string Id { get; set; }
        public string WardPersonnelId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class WardPersonnelBasicInfoDtoForUpdate
    {
        [Required]
        public string WardPersonnelId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }

    public class WardPersonnelContactDetailsDtoForUpdate
    {
        [Required]
        public string WardPersonnelId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class WardPersonnelProfilePictureDtoForUpdate
    {
        [Required]
        public string WardPersonnelId { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
}
