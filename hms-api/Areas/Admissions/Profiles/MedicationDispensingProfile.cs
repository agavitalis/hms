using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;


namespace HMS.Areas.Admissions.Profiles
{
    public class MedicationDispensingProfile : Profile
    {
        public MedicationDispensingProfile()
        {
            CreateMap<AdmissionMedicationDispensing, AdmissionMedicationDispensingDtoForCreate>().ReverseMap();
        }
    }
}
