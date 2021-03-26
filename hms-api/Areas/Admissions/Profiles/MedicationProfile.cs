using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;


namespace HMS.Areas.Admissions.Profiles
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<AdmissionDrugMedication, DrugMedicationDtoForView>().ReverseMap();
            CreateMap<AdmissionDrugMedication, DrugMedicationDtoForCreate>().ReverseMap();
            CreateMap<AdmissionDrugMedication, DrugMedicationStatusDtoForUpdate>().ReverseMap();
            CreateMap<AdmissionServiceMedication, ServiceMedicationDtoForView>().ReverseMap();
            CreateMap<AdmissionServiceRequest, ServiceMedicationDtoForAdminister>().ReverseMap();
        }
    }
} 
