using System;
using System.Collections.Generic;
using System.Linq;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Services
{
    public class TransactionService: ITransactionService
    {
        public TransactionDbContext DbContext { get; set; }

        public TransactionService(TransactionDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool AddTransactions(IList<Transactions> transactions)
        {
            try
            {
                DbContext.Transactions.AddRange(transactions);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //Todo: Add log
                return false;
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByCurrency(string currencyCode)
        {
            try
            {
                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where string.Equals(t.CurrencyCode, currencyCode, StringComparison.CurrentCultureIgnoreCase)
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = t.Amount + " " + t.CurrencyCode,
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                //Todo: Add log
                return null;
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where t.TransactionDate >= startDate && t.TransactionDate <= endDate
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = t.Amount + " " + t.CurrencyCode,
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                //Todo: Add log
                return null;
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByStatus(string status)
        {
            try
            {
                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where string.Equals(t.Status, status, StringComparison.CurrentCultureIgnoreCase)
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = t.Amount + " " + t.CurrencyCode,
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                //Todo: Add log
                return null;
            }
        }
    }
}
