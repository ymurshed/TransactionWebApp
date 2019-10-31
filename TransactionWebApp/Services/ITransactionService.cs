using System;
using System.Collections.Generic;
using System.Linq;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Services
{
    public interface ITransactionService
    {
        bool AddTransactions(IList<Transactions> transactions);
        IQueryable<TransactionResponseModel> GetTransactionsByCurrency(string currencyCode);
        IQueryable<TransactionResponseModel> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        IQueryable<TransactionResponseModel> GetTransactionsByStatus(string status);
    }
}
