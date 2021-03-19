using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOHealthPlanPatient
    {
        Task<bool> CreateHMOHealthPlanPatient(HMOHealthPlanPatient PatientHMOHealthPlan);
    }
}
