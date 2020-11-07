using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HMS.Areas.Patient.ViewModels.PatientConsultationViewModel;
using AutoMapper;

namespace HMS.Areas.Admin.Profiles
{
    public class ConsultationProfile : Profile
    {
        public ConsultationProfile()
        {
            CreateMap<Consultation, BookConsultation>();
            CreateMap<BookConsultation, Consultation>();
        }
    }
}
