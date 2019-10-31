using System.Collections.Generic;
using TransactionWebApp.DbModels;
using TransactionWebApp.Models;

namespace TransactionWebApp.Helpers
{
    public interface IFileHandler
    {
        TransactionModel GetTranscations(string path, string uploadedFileName);
    }
}
