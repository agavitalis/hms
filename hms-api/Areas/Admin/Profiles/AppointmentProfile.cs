using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Doctor.Models;

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
