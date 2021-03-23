using HMS.Areas.Patient.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOHealthPlanPatient
    {
        Task<HMOHealthPlanPatient> GetHMOHealthPlanPatient(string HMOHealthPlanId);
        PagedList<PatientDtoForView> GetHMOHealthPlanPatients(string HMOHealthPlanId, PaginationParameter paginationParameter);
        Task<bool> CreateHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan);
        Task<bool> DeleteHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan);
    }
}
