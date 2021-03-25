using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;


namespace HMS.Areas.Admissions.Profiles
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<AdmissionMedication, MedicationDtoForView>().ReverseMap();
            CreateMap<AdmissionMedication, MedicationDtoForCreate>().ReverseMap();
            CreateMap<AdmissionMedication, MedicationStatusDtoForUpdate>().ReverseMap();
        }
    }
} 
