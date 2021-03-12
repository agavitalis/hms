using AutoMapper;
using HMS.Database;
using HMS.Models;
using HMS.Services.Dtos;
using HMS.Services.Helpers;
using HMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Repositories
{
    public class TransactionLogRepository : ITransactionLog
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public TransactionLogRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<dynamic>> GetAccountTransactions(string AccountId) => await _applicationDbContext.Transactions
            .Where(t => t.BenefactorAccountId == AccountId).Include(t => t.Benefactor).OrderBy(a => a.TrasactionDate)
            .Select(p => new
            {
                Id = p.Id,
                Amount = p.Amount,
                TransactionType = p.TransactionType,
                Description = p.PaymentMethod,
                TrasactionDate = p.TrasactionDate,
                AccountBalance = p.BenefactorAccount.AccountBalance,
                Initiator = p.Initiator.FirstName + " " + p.Initiator.LastName

            })

            .ToListAsync();
        
        public async Task<bool> LogLinkPaymentTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAccountId, decimal BenefactorAccountPreviousBalance, string Initiator)
        {
            try
            {
                if (amount != 0 && transactionType != null && PaymentMethod != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        PaymentMethod = PaymentMethod,
                        TrasactionDate = transactionDate,
                        BenefactorAccountId = BenefactorAccountId,
                        BenefactorAccountPreviousBalance = BenefactorAccountPreviousBalance,
                        DepositorsName = Initiator
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> LogTransactionAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorId, string InitiatorId)
        {
            try
            {
                if (amount != 0 && transactionType != null && PaymentMethod != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        PaymentMethod = PaymentMethod,
                        TrasactionDate = transactionDate,
                        BenefactorId = BenefactorId,
                        InitiatorId = InitiatorId
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public bool LogTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorId, string InitiatorId)
            {
                try
                {
                    if (amount != 0 && transactionType != null && PaymentMethod != null && transactionDate != null)
                    {
                        var transaction = new Transactions()
                        {
                            Amount = amount,
                            TransactionType = transactionType,
                            InvoiceType = invoiceType,
                            InvoiceId = invoiceId,
                            PaymentMethod = PaymentMethod,
                            TrasactionDate = transactionDate,
                            BenefactorId = BenefactorId,
                            InitiatorId = InitiatorId
                        };

                        _applicationDbContext.Transactions.Add(transaction);
                         _applicationDbContext.SaveChanges();

                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
         }

        public bool LogAccountTransaction(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAccountId, string InitiatorId)
        {
            try
            {
                if (amount != 0 && transactionType != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        PaymentMethod = PaymentMethod,
                        TrasactionDate = transactionDate,
                        BenefactorAccountId = BenefactorAccountId,
                        InitiatorId = InitiatorId
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> LogAccountTransactionAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAccountId, decimal previousAccountBalance, string InitiatorId)
        {
            try
            {
                if (amount != 0 && transactionType != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        PaymentMethod = PaymentMethod,
                        TrasactionDate = transactionDate,
                        BenefactorAccountId = BenefactorAccountId,
                        BenefactorAccountPreviousBalance = previousAccountBalance,
                        InitiatorId = InitiatorId
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedList<TransactionsDtoForView> GetAccountTransactions(string AccountId, PaginationParameter paginationParameter)
        {
            var transactions = _applicationDbContext.Transactions.Where(t => t.BenefactorAccountId == AccountId).Include(t => t.Benefactor).OrderBy(a => a.TrasactionDate).ToList();
            var transactionsToReturn = _mapper.Map<IEnumerable<TransactionsDtoForView>>(transactions);
            return PagedList<TransactionsDtoForView>.ToPagedList(transactionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public PagedList<TransactionsDtoForView> GetAdmissionTransactions(string AdmissionId, PaginationParameter paginationParameter)
        {
            var transactions = _applicationDbContext.Transactions.Where(t => t.BenefactorAdmissionId == AdmissionId).Include(t => t.BenefactorAdmission).Include(t => t.Initiator).OrderBy(a => a.TrasactionDate).ToList();
            var transactionsToReturn = _mapper.Map<IEnumerable<TransactionsDtoForView>>(transactions);
            return PagedList<TransactionsDtoForView>.ToPagedList(transactionsToReturn.AsQueryable(), paginationParameter.PageNumber, paginationParameter.PageSize);
        }

        public async Task<bool> LogAdmissionTransactionAsync(decimal amount, string transactionType, string invoiceType, string invoiceId, string PaymentMethod, DateTime transactionDate, string BenefactorAdmissionId, string InitiatorId)
        {
            try
            {
                if (amount != 0 && transactionType != null && transactionDate != null)
                {
                    var transaction = new Transactions()
                    {
                        Amount = amount,
                        TransactionType = transactionType,
                        InvoiceType = invoiceType,
                        InvoiceId = invoiceId,
                        PaymentMethod = PaymentMethod,
                        TrasactionDate = transactionDate,
                        BenefactorAdmissionId = BenefactorAdmissionId,
                        InitiatorId = InitiatorId
                    };

                    _applicationDbContext.Transactions.Add(transaction);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    
}

