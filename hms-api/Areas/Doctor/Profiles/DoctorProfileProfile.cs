using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using System;

namespace HMS.Areas.Doctor.Profiles
{
    public class DoctorProfileProfile : Profile
    {
        public DoctorProfileProfile()
        {
            CreateMap<DoctorProfile, DoctorDtoForView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Doctor.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Doctor.FirstName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Doctor.OtherNames));

            CreateMap<DoctorEducationDtoForCreate, DoctorEducation>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));
            CreateMap<DoctorEducation, DoctorEducationDtoForView>();
            CreateMap<DoctorEducationDtoForView, DoctorEducation>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));

            CreateMap<DoctorExperienceDtoForCreate, DoctorExperience>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));
            CreateMap<DoctorExperience, DoctorExperienceDtoForView>();
            CreateMap<DoctorExperienceDtoForView, DoctorExperience>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));

            CreateMap<DoctorSocialDtoForCreate, DoctorSocial>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));
            CreateMap<DoctorSocial, DoctorSocialDtoForView>();
            CreateMap<DoctorSocialDtoForView, DoctorSocial>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));

            CreateMap<DoctorOfficeTimeDtoForCreate, DoctorOfficeTime>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));
            CreateMap<DoctorOfficeTime, DoctorOfficeTimeDtoForView>();
            CreateMap<DoctorOfficeTimeDtoForView, DoctorOfficeTime>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now.ToString()));

        }
    }
}
