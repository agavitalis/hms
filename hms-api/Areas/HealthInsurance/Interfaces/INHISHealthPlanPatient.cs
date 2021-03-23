using HMS.Models;
using System;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface INHISHealthPlanPatient
    {
        Task<bool> CreateNHISHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient);
        Task<bool> DeleteHealthPlanPatient(NHISHealthPlanPatient NHISHealthPlanPatient);
    }
}
