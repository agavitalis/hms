using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Patient.Dtos;
using HMS.Models;
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
            CreateMap<File, DtoForFileCreation>();
            CreateMap<DtoForFileCreation, File>();

            CreateMap<ApplicationUser, DtoForPatientRegistration>();
            CreateMap<DtoForPatientRegistration, ApplicationUser>();

            CreateMap<RegistrationInvoice, DtoForPatientRegistration>();
            CreateMap<DtoForPatientRegistration, RegistrationInvoice>();

            CreateMap<RegistrationInvoice, PatientRegistrationPaymentDto>();
            CreateMap<PatientRegistrationPaymentDto, RegistrationInvoice>();

            CreateMap<RegistrationInvoice, DtoForPatientRegistrationInvoice>();


            //CreateMap<Models.Account, FileDtoForCreate>()
            //    .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.FileNumber, opt => opt.MapFrom(src => $"HMS-{src.HealthPlanId.ToString().PadLeft(3, '0')}{src.Id}"));
            //CreateMap<FileDtoForCreate, File>();

            //CreateMap<PatientDtoForCreate, AccountDtoForCreate>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
