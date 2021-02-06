using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;

namespace HMS.Areas.Admin.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<BookAppointmentDto, Appointment>();
            CreateMap<Appointment, BookAppointmentDto>();

            CreateMap<ReassignAppointmentDto, Appointment>();
            CreateMap<Appointment, ReassignAppointmentDto>();

            CreateMap<AppointmentDtoForView, Appointment>();
            CreateMap<Appointment, AppointmentDtoForView>();

        }
    }
}
