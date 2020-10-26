using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreateBookAppointmentDto, DoctorAppointment>();
            CreateMap<DoctorAppointment, CreateBookAppointmentDto>();
               
        }
    }
}
