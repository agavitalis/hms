using System;


namespace HMS.Areas.Patient.Dtos
{
    public class PatientsDtoForView
    {
        public string FileNumber { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string BloodGroup { get; set; }
        public string GenoType { get; set; }
        public string Allergies { get; set; }
        public string Disabilities { get; set; }
        public Boolean Diabetic { get; set; }
        public string ConsentCode { get; set; }
    }

    public class PatientDtoForView
    {
        public string PatientId { get; set; }
        public string FullName { get; set; }
        public string FileNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Gender { get; set; }
       // public string ProfileImage { get; set; }
    }
}
