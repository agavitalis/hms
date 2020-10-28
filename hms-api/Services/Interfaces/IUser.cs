using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface IUser
    {
        Task<ApplicationUser> GetUserByIdAsync(string Id);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}
