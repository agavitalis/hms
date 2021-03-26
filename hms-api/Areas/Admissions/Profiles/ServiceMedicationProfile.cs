using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;

namespace HMS.Areas.Admissions.Profiles
{
    public class ServiceMedicationProfile : Profile
    {
        public ServiceMedicationProfile()
        {
            CreateMap<AdmissionServiceMedication, ServiceMedicationDtoForCreate>().ReverseMap();
        }
    }
}
