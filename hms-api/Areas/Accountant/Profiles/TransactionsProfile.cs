using AutoMapper;
using HMS.Areas.Accountant.Dtos;
using HMS.Models;


namespace HMS.Areas.Accountant.Profiles
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
