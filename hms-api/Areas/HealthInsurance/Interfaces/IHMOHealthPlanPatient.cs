using HMS.Areas.Patient.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMOHealthPlanPatient
    {
        Task<int> GetHealthPlanPatientCount(string HMOHealthPlanId);
        Task<HMOHealthPlanPatient> GetHMOHealthPlanPatient(string HMOHealthPlanId);
        PagedList<PatientDtoForView> GetHMOHealthPlanPatients(string HMOHealthPlanId, PaginationParameter paginationParameter);
        Task<bool> CreateHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan);
        Task<bool> DeleteHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan);
    }
}
