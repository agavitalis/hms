using HMS.Database;
using HMS.Models;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext _applicationDbcontext;

        public UserRepository(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbcontext = applicationDbcontext;
        }


        public async Task<int> GetUserCount() =>
           await _applicationDbcontext.ApplicationUsers.CountAsync();

        public async Task<ApplicationUser> GetUserByEmailAsync(string email) => 
            await _applicationDbcontext.ApplicationUsers.FirstOrDefaultAsync(d => d.Email == email);

        public async Task<ApplicationUser> GetUserByIdAsync(string Id) =>
              await _applicationDbcontext.ApplicationUsers.FirstOrDefaultAsync(d => d.Id == Id);

    }
}
