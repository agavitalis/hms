using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Doctor.Models;

namespace HMS.Areas.Admin.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<BookAppointmentDto, DoctorAppointment>();
            CreateMap<DoctorAppointment, BookAppointmentDto>();
               
        }
    }
}
