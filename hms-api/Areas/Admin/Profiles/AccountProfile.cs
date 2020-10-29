using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class AccountProfile: Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDtoForCreate, Account>();
            CreateMap<Account, AccountDtoForCreate>();

            CreateMap<AccountDtoForView, Account>();
            CreateMap<Account, AccountDtoForView>();

            CreateMap<AccountDtoForUpdate, Account>();
            CreateMap<Account, AccountDtoForUpdate>();

            CreateMap<AccountDtoForDelete, Account>();
            CreateMap<Account, AccountDtoForDelete>();
        }
           
    }
}
