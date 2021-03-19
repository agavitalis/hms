using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class AdmissionServiceRequestProfile : Profile
    {
        public AdmissionServiceRequestProfile()
        {
            CreateMap<AdmissionServiceRequest, AdmissionServiceRequestDtoForCreate>().ReverseMap();
            CreateMap<AdmissionServiceRequest, AdmissionServiceRequestDtoForView>().ReverseMap();
            CreateMap<AdmissionServiceRequest, AdmissionServiceRequestPaymentDto>().ReverseMap();
            CreateMap<AdmissionServiceRequestResult, AdmissionServiceUploadResultDto>().ReverseMap();
            CreateMap<AdmissionServiceRequestResult, AdmissionServiceRequestDtoForView>().ReverseMap();
            CreateMap<AdmissionServiceRequestResult, AdmissionServiceRequestResultDtoForView>().ReverseMap();
        }
        
    }
}
