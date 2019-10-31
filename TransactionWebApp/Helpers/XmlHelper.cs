using System;
using System.Collections.Generic;
using System.Linq;
using TransactionWebApp.Constants;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Helpers
{
    public static class XmlHelper
    {
        public static string IsValid(this XmlModel xmlModel, int index, string uploadedFileName)
        {
            var invalidItems = new List<string>();
            var paymentDetails = new List<string> { CommonConstant.Amount, CommonConstant.CurrencyCode };

            if (string.IsNullOrWhiteSpace(xmlModel.Id))
            {
                invalidItems.Add(CommonConstant.TransactionId);
            }

            if (string.IsNullOrWhiteSpace(xmlModel.TransactionDate) ||
                !DateTime.TryParse(xmlModel.TransactionDate, out var transactionDate))
            {
                invalidItems.Add(CommonConstant.TransactionDate);
            }

            if (string.IsNullOrWhiteSpace(xmlModel.Status))
            {
                invalidItems.Add(CommonConstant.Status);
            }

            if (xmlModel.PaymentDetails == null)
            {
                invalidItems.AddRange(paymentDetails);
            }
            else if (string.IsNullOrWhiteSpace(xmlModel.PaymentDetails.Amount) ||
                     !decimal.TryParse(xmlModel.PaymentDetails.Amount, out var amount))
            {
                invalidItems.Add(paymentDetails[0]);
            }
            else if (string.IsNullOrWhiteSpace(xmlModel.PaymentDetails.CurrencyCode))
            {
                invalidItems.Add(paymentDetails[1]);
            }

            if (!invalidItems.Any()) return string.Empty;

            var errorMsg = $"In File: {uploadedFileName}, Transaction #{index}, {CommonConstant.InvalidItems}" +
                            string.Join(", ", invalidItems);
            return errorMsg;
        }

        public static Transactions ToTransaction(this XmlModel xmlModel)
        {
            var transactionHistory = new Transactions { TransactionId = xmlModel.Id };
            DateTime.TryParse(xmlModel.TransactionDate, out var dateTime);
            transactionHistory.TransactionDate = dateTime;
            decimal.TryParse(xmlModel.PaymentDetails.Amount, out var amount);
            transactionHistory.Amount = amount;
            transactionHistory.CurrencyCode = xmlModel.PaymentDetails.CurrencyCode;
            transactionHistory.Status = xmlModel.Status;
            return transactionHistory;
        }
    }
}
