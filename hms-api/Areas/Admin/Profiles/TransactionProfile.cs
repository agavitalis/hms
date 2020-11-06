using AutoMapper;
using HMS.Areas.Admin.Dtos;
using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transactions, AccountDtoForAdminFunding>();
            CreateMap<AccountDtoForAdminFunding, Transactions>();

            CreateMap<Transactions, AccountDtoForPatientFunding>();
            CreateMap<AccountDtoForPatientFunding, Transactions>();
        }
    }
}
