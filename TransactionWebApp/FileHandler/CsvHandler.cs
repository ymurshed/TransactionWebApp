using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TransactionWebApp.DbModels;
using TransactionWebApp.FileHandler;
using TransactionWebApp.Models;
using TransactionWebApp.Utility.Constants;
using TransactionWebApp.Utility.Helpers;

namespace TransactionWebApp.Helpers
{
    public class CsvHandler: IFileHandler
    {
        public TransactionModel GetTranscations(string path, string uploadedFileName)
        {
            var csvModels = Parse(path);
            if (!csvModels.Any())
            {
                Logger.Log.Debug(LogConstant.NoCsvModel);
                return null;
            }

            var transcations = new List<Transactions>();
            var errors = GetErrors(csvModels, uploadedFileName);

            if (!errors.Any())
            {
                foreach (var csvModel in csvModels)
                {
                    transcations.Add(csvModel.ToTransaction());
                }
            }

            var transactionModel = new TransactionModel { Transcations = transcations, Errors = errors };
            return transactionModel;
        }

        private static IList<string> GetErrors(IList<CsvModel> csvModels, string uploadedFileName)
        {
            var errors = new List<string>();
            for (var i = 0; i < csvModels.Count; i++)
            {
                var error = csvModels[i].IsValid(i + 1, uploadedFileName);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    errors.Add(error);
                }
            }
            return errors;
        }

        private static IList<CsvModel> Parse(string path)
        {
            var splitChar = new[] { ',', '"', '“', '”', '\n', '\r', '\t' };
            var csvModels = new List<CsvModel>();

            try
            {
                var lines = File.ReadAllLines(path, Encoding.Default);
                foreach (var line in lines)
                {
                    var items = line.Split(splitChar);
                    var lineItems = new List<string>(items);
                    lineItems.RemoveAll(string.IsNullOrWhiteSpace);

                    var i = 1;
                    var amount = lineItems[i];

                    if (lineItems.Count > 5)
                    {
                        amount = string.Empty;
                        var amountLength = lineItems.Count - 5;
                        for (; i <= amountLength + 1; i++) amount += lineItems[i];
                    }
                    else
                    {
                        i++;
                    }

                    var csvModel = new CsvModel
                    {
                        TransactionIdentificator = lineItems[0].Trim(),
                        Amount = amount.Trim(),
                        CurrencyCode = lineItems[i++].Trim(),
                        TransactionDate = lineItems[i++].Trim(),
                        Status = lineItems[i].Trim()
                    };
                    csvModels.Add(csvModel);
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(LogConstant.CsvParseError + e.Message + e.InnerException);
            }
            return csvModels;
        }
    }
}
