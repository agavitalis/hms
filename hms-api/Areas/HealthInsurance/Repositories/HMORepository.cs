using AutoMapper;
using HMS.Areas.HealthInsurance.Interfaces;
using HMS.Areas.NHIS.Dtos;
using HMS.Database;
using HMS.Models;
using HMS.Services.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.HealthInsurance.Repositories
{
    public class HMORepository : IHMO
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public HMORepository(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<bool> CreateHMO(HMO HMO)
        {
            try
            {
                if (HMO == null)
                {
                    return false;
                }

                _applicationDbContext.HMOs.Add(HMO);
                await _applicationDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationUser> CreateUser(string firstName, string lastName, string email, string roleName)
        {
            var newApplicationUser = new ApplicationUser();

            newApplicationUser = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                UserType = roleName
            };

            var result = await _userManager.CreateAsync(newApplicationUser, "Password1@test");
            if (result.Succeeded)
            {
                var res = await _userManager.AddToRoleAsync(newApplicationUser, roleName);

                if (res.Succeeded)
                {
                    return newApplicationUser;
                }
                return null;
            }
            return null;
        }

        public async Task<bool> CreateUserProfile(string UserId, string FirstName, string LastName, string HMOId)
        {
            var profile = new HMOAdminProfile()
            {
                HMOAdminId = UserId,
                FullName = $"{FirstName} {LastName}",
                HMOId = HMOId
            };
            _applicationDbContext.HMOAdminProfiles.Add(profile);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<HMO> GetHMO(string HMOId) => await _applicationDbContext.HMOs.Where(h => h.Id == HMOId).Include(h => h.HealthPlan).FirstOrDefaultAsync();

        public async Task<int> GetHMOCount() => await _applicationDbContext.HMOs.CountAsync();

        public PagedList<HMODtoForView> GetHMOs(PaginationParameter paginationParameter)
        {
            var HMOs = _applicationDbContext.HMOs.Include(h => h.HealthPlan).OrderBy(h => h.Name).ToList();
            var HMOsToReturn = _mapper.Map<IEnumerable<HMODtoForView>>(HMOs);
            return PagedList<HMODtoForView>.ToPagedList(HMOsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }
    }
}
