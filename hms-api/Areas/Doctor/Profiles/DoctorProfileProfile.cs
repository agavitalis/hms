﻿using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Areas.Doctor.Models;

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
        }
    }
}
