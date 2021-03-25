using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Interfaces
{
    public interface IHMO
    {
        Task<int> GetHMOCount();
        Task<HMO> GetHMO(string HMOId);
        PagedList<HMODtoForView> GetHMOs(PaginationParameter paginationParameter);
        Task<bool> CreateHMO(HMO HMO);
        Task<ApplicationUser> CreateUser(string FirstName, string LastName, string Email, string RoleName);
        Task<bool> CreateUserProfile(string UserId, string FirstName, string LastName, string HMOId);
    }
}
