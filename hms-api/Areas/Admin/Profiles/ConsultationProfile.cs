using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;
using AutoMapper;
using HMS.Areas.Admin.Dtos;

namespace HMS.Areas.Admin.Profiles
{
    public class ConsultationProfile : Profile
    {
        public ConsultationProfile()
        {
            CreateMap<Consultation, BookConsultation>();
            CreateMap<BookConsultation, Consultation>();

            CreateMap<Consultation, ConsultationDtoForUpdate>();
            CreateMap<ConsultationDtoForUpdate, Consultation>();

            CreateMap<Consultation, ReassignConsultationDto>();
            CreateMap<ReassignConsultationDto, Consultation>();
        }
    }
}
