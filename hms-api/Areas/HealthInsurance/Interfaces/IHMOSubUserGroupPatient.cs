using HMS.Areas.Patient.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOSubUserGroupPatient
    {
        Task<int> GetHMOSubUserGroupPatientCount(string HMOSubUserGroupId);
        Task<HMOSubUserGroupPatient> GetHMOSubUserGroupPatient(string HMOSubGroupPatientId);
        PagedList<PatientDtoForView> GetHMOSubUserGroupPatients(string HMOSubGroupId, PaginationParameter paginationParameter);
        Task<bool> CreateHMOSubGroupPatient(HMOSubUserGroupPatient HMOSubUserGroupPatient);
        Task<bool> DeleteHMOSubGroupPatient(HMOSubUserGroupPatient HMOSubUserGroupPatient);
    }
}
