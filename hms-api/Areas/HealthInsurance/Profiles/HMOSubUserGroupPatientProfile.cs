using AutoMapper;
using HMS.Areas.NHIS.Dtos;
using HMS.Models;

namespace HMS.Areas.NHIS.Profiles
{
    public class HMOSubUserGroupPatientProfile : Profile
    {
        public HMOSubUserGroupPatientProfile()
        {
            CreateMap<HMOSubUserGroupPatient, HMOSubUserGroupPatientDtoForCreate>().ReverseMap();
            CreateMap<HMOSubUserGroupPatient, HMOSubUserGroupPatientDtoForDelete>().ReverseMap();
        }
    }
}
