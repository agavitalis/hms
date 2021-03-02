using AutoMapper;
using HMS.Areas.Admissions.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admissions.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<AdmissionInvoice, AdmissionPaymentDto>().ReverseMap();
        }
    }
}
