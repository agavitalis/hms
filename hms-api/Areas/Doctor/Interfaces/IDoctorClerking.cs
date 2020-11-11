using HMS.Areas.Doctor.Dtos;
using HMS.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace HMS.Areas.Doctor.Interfaces
{
    public interface IDoctorClerking
    {
        Task<DoctorClerking> GetDoctorClerking(string consultationId);
        Task<bool> CreateDoctorClerking(DoctorClerking clerking);

        Task<DoctorClerking> CreateDoctorClerking(string Id, string IdType);
        Task<bool> UpdateDoctorClerking(DoctorClerking doctorClerking, JsonPatchDocument<DoctorClerkingDtoForUpdate> clerking);
    }
}
