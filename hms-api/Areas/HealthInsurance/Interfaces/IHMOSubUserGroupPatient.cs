using HMS.Models;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMOSubUserGroupPatient
    {
        Task<bool> CreateHMOSubGroupPatient(HMOSubUserGroupPatient HMOSubUserGroupPatient);
        Task<bool> DeleteHMOSubGroupPatient(HMOSubUserGroupPatient HMOSubUserGroupPatient);
    }
}
