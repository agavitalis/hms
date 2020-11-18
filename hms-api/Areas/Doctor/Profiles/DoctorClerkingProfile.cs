using AutoMapper;
using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;


namespace HMS.Areas.Doctor.Profiles
{
    public class DoctorClerkingProfile : Profile
    {
        public DoctorClerkingProfile()
        {
            CreateMap<DoctorClerking, DoctorClerkingDtoForCreate>();
            CreateMap<DoctorClerkingDtoForCreate, DoctorClerking>();

            CreateMap<DoctorClerking, DoctorClerkingDtoForUpdate>();
            CreateMap<DoctorClerkingDtoForUpdate, DoctorClerking>();

            CreateMap<DoctorClerking, JsonPatchDocument<DoctorClerkingDtoForUpdate>>();
            CreateMap<JsonPatchDocument<DoctorClerkingDtoForUpdate>, DoctorClerking>();
        }
    }
}
