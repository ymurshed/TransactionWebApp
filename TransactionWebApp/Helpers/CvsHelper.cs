using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TransactionWebApp.Constants;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Helpers
{
    public static class CvsHelper
    {
        public static string IsValid(this CsvModel csvModel, int index, string uploadedFileName)
        {
            var invalidItems = new List<string>();

            if (string.IsNullOrWhiteSpace(csvModel.TransactionIdentificator))
            {
                invalidItems.Add(CommonConstant.TransactionId);
            }

            if (string.IsNullOrWhiteSpace(csvModel.Status))
            {
                invalidItems.Add(CommonConstant.Status);
            }

            if (string.IsNullOrWhiteSpace(csvModel.Amount) ||
                !decimal.TryParse(csvModel.Amount, out var amount))
            {
                invalidItems.Add(CommonConstant.Amount);
            }

            if (string.IsNullOrWhiteSpace(csvModel.CurrencyCode))
            {
                invalidItems.Add(CommonConstant.CurrencyCode);
            }

            if (!string.IsNullOrWhiteSpace(csvModel.TransactionDate))
            {
                try
                {
                    var dt = DateTime.ParseExact(csvModel.TransactionDate, CommonConstant.DateTimeFormatForCsv, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    invalidItems.Add(CommonConstant.TransactionDate);
                }
            }
            else if (string.IsNullOrWhiteSpace(csvModel.TransactionDate))
            {
                invalidItems.Add(CommonConstant.TransactionDate);
            }

            if (!invalidItems.Any()) return string.Empty;

            var msg = $"In File: {uploadedFileName}, Transaction #{index}, {CommonConstant.InvalidItems}" + string.Join(", ", invalidItems);
            return msg;
        }

        public static Transactions ToTransaction(this CsvModel csvModel)
        {
            var transactionHistory = new Transactions { TransactionId = csvModel.TransactionIdentificator };
            var dateTime = DateTime.ParseExact(csvModel.TransactionDate, CommonConstant.DateTimeFormatForCsv, CultureInfo.InvariantCulture);
            transactionHistory.TransactionDate = dateTime;
            decimal.TryParse(csvModel.Amount, out var amount);
            transactionHistory.Amount = amount;
            transactionHistory.CurrencyCode = csvModel.CurrencyCode;
            transactionHistory.Status = csvModel.Status;
            return transactionHistory;
        }
    }
}
