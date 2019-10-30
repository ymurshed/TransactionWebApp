using System;
using System.Collections.Generic;

namespace TransactionWebApp.DbModels
{
    public partial class Transactions
    {
        public decimal Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
