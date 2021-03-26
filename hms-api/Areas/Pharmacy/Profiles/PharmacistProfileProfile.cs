using AutoMapper;
using HMS.Areas.Pharmacy.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Pharmacy.Profiles
{
    public class PharmacistProfileProfile : Profile
    {
        public PharmacistProfileProfile()
        {

            CreateMap<PharmacyProfile, PharmacistDtoForView>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Pharmacist.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Pharmacist.LastName))
                .ForMember(dest => dest.OtherNames, opt => opt.MapFrom(src => src.Pharmacist.OtherNames))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Pharmacist.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Pharmacist.PhoneNumber))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.Pharmacist.UserType))
                .ReverseMap();
        }
    }
}
