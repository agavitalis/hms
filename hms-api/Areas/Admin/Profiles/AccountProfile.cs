using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;

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
