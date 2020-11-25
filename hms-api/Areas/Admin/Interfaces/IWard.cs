using HMS.Models;
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
        Task<bool> CreateWard(Ward ward);
        Task<bool> UpdateWard(Ward ward);
        Task<bool> DeleteWard(Ward ward);


        //Task<Ward> GetWardSubCategoryByIdAsync(string id);
        //Task<IEnumerable<Ward>> GetWardSubCategories();
        //Task<bool> CreateWardSubCategory(WardSubCategory ward);
        //Task<bool> UpdateWardSubCategory(WardSubCategory ward);
        //Task<bool> DeleteWardSubCategory(WardSubCategory ward);


        //Task<Ward> GetWardCategoryByIdAsync(string id);
        //Task<IEnumerable<Ward>> GetWardCategoriess();
        //Task<bool> CreateWardCategory(WardCategory ward);
        //Task<bool> UpdateWardCategory(WardCategory ward);
        //Task<bool> DeleteWardCategory(WardCategory ward);
    }
}
