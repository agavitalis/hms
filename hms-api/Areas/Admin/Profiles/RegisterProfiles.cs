using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using HMS.Areas.Patient.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class RegisterProfiles : Profile
    {
        public RegisterProfiles()
        {
            CreateMap<Models.Account, FileDtoForCreate>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileNumber, opt => opt.MapFrom(src => $"HMS-{src.HealthPlanId.ToString().PadLeft(3, '0')}{src.Id}"));
            CreateMap<FileDtoForCreate, File>();

            CreateMap<PatientDtoForCreate, AccountDtoForCreate>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
