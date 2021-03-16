using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IPatientHMOHealthPlan
    {
        Task<bool> CreatePatientHMOHealthPlan(PatientHMOHealthPlan PatientHMOHealthPlan);
    }
}
