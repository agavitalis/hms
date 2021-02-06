using AutoMapper;
using HMS.Models;
using HMS.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<TransactionsDtoForView, Transactions>();
            CreateMap<Transactions, TransactionsDtoForView>();
        }

    }
}
