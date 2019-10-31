using System.Collections.Generic;
using TransactionWebApp.DbModels;

namespace TransactionWebApp.Models
{
    public class TransactionModel
    {
        public IList<Transactions> Transcations { get; set; }
        public IList<string> Errors { get; set; }
    }
}
