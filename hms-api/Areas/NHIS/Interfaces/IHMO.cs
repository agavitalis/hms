using HMS.Areas.NHIS.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System.Threading.Tasks;

namespace HMS.Areas.NHIS.Interfaces
{
    public interface IHMO
    {
        Task<HMO> GetHMO(string HMOId);
        PagedList<HMODtoForView> GetHMOs(PaginationParameter paginationParameter);
        Task<bool> CreateHMO(HMO HMO);
    }
}
