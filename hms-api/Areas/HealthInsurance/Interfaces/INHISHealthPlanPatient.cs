using HMS.Areas.Patient.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlanPatient
    {
        Task<int> GetNHISHealthPlanPatientCount(string NHISHealthPlanId);
        Task<NHISHealthPlanPatient> GetNHISHealthPlanPatient(string NHISHealthPlanId);
        PagedList<PatientDtoForView> GetNHISHealthPlanPatients(string NHISHealthPlanId, PaginationParameter paginationParameter);
        Task<bool> CreateNHISHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient);
        Task<bool> UpdateNHISHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient);
        Task<bool> DeleteHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient);
    }
}
