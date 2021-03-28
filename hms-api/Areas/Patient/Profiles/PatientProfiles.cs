using AutoMapper;
using HMS.Areas.Patient.Dtos;
using HMS.Models;


namespace HMS.Areas.Patient.Profiles
{
    public class PatientProfiles : Profile
    {
        public PatientProfiles()
        {
            CreateMap<PatientProfile, PatientDtoForView>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Patient.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Patient.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Patient.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Patient.UserType))
                .ReverseMap();
            

            CreateMap<HMOHealthPlanPatient, PatientDtoForView>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Patient.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Patient.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Patient.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Patient.UserType)).ReverseMap();


            CreateMap<NHISHealthPlanPatient, PatientDtoForView>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Patient.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Patient.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Patient.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Patient.UserType)).ReverseMap();


            CreateMap<HMOSubUserGroupPatient, PatientDtoForView>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Patient.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Patient.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Patient.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Patient.UserType)).ReverseMap();
        }
    }
}
