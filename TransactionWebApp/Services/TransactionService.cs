using System;
using System.Collections.Generic;
using System.Linq;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;
using TransactionWebApp.Utility.Constants;

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
                Logger.Log.Debug(LogConstant.SaveTransaction);

                DbContext.Transactions.AddRange(transactions);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                var error = LogConstant.SaveTransactionError + e.Message + e.InnerException; 
                Logger.Log.Error(error);
                return false;
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByCurrency(string currencyCode)
        {
            try
            {
                Logger.Log.Debug(LogConstant.GetTransactionByCode);

                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where string.Equals(t.CurrencyCode, currencyCode, StringComparison.CurrentCultureIgnoreCase)
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = string.Concat(t.Amount, " ", t.CurrencyCode),
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                var error = LogConstant.GetTransactionErrorByCode + e.Message + e.InnerException;
                throw new Exception(error);
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                Logger.Log.Debug(LogConstant.GetTransactionByDateRange);

                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where t.TransactionDate >= startDate && t.TransactionDate <= endDate
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = string.Concat(t.Amount, " " , t.CurrencyCode),
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                var error = LogConstant.GetTransactionErrorByDateRange + e.Message + e.InnerException; 
                throw new Exception(error);
            }
        }

        public IQueryable<TransactionResponseModel> GetTransactionsByStatus(string status)
        {
            try
            {
                Logger.Log.Debug(LogConstant.GetTransactionByStatus);

                var ctx = DbContext;
                var response = from t in ctx.Transactions join ts in ctx.TransactionStatus
                                on t.Status equals ts.Status
                                where string.Equals(t.Status, status, StringComparison.CurrentCultureIgnoreCase)
                                select new TransactionResponseModel
                                {
                                    Id = t.TransactionId,
                                    Payment = string.Concat(t.Amount, " ", t.CurrencyCode),
                                    Status = ts.Symbol
                                };
                return response;
            }
            catch (Exception e)
            {
                var error = LogConstant.GetTransactionErrorByStatus + e.Message + e.InnerException;
                throw new Exception(error);
            }
        }
    }
}
