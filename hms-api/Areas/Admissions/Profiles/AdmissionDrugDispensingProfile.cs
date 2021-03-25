using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;

namespace HMS.Areas.Admissions.Profiles
{
    public class AdmissionDrugDispensingProfile : Profile
    {
        public AdmissionDrugDispensingProfile()
        {
            CreateMap<AdmissionDrugDispensing, AdmissionDrugDispensingDtoForCreate>().ReverseMap();
            CreateMap<AdmissionDrugDispensing, DrugDispensingPaymentDto>().ReverseMap();
            CreateMap<AdmissionDrugDispensing, AdmissionDrugDispensingDtoForView>().ReverseMap();
            CreateMap<AdmissionDrugDispensing, MedicationDtoForAdminister>().ReverseMap();
        }

    }
}
