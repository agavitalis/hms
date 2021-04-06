﻿using HMS.Models;



namespace HMS.Areas.Patient.Dtos
{
    public class PatientDtoForView
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string UserType { get; set; }
        public Account Account { get; set; }
        public HealthPlan HealthPlan { get; set; }

    }
}
