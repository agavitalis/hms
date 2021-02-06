using HMS.Areas.Admin.Dtos;
using HMS.Models;
using HMS.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Interfaces
{
    public interface IWard
    {
        Task<Ward> GetWardByIdAsync(string id);
        Task<IEnumerable<Ward>> GetAllWards();
        PagedList<WardDtoForView> GetWardsPagnation(PaginationParameter paginationParameter);
        Task<bool> CreateWard(Ward ward);
        Task<bool> UpdateWard(Ward ward);
        Task<bool> DeleteWard(Ward ward);

    }
}
