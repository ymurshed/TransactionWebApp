using System;
using Microsoft.AspNetCore.Mvc;
using TransactionWebApp.Services;

namespace TransactionWebApp.Controllers
{
    public class TransactionController : Controller
    {
        public ITransactionService TransactionService { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public TransactionController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        public IActionResult TransactionsByCurrency(string currency)
        {
            var response = TransactionService.GetTransactionsByCurrency(currency);
            return Ok(response);
        }

        public IActionResult TransactionsByDateRange(string startDate, string endDate)
        {
            var sd = Convert.ToDateTime(startDate);
            var ed = Convert.ToDateTime(endDate);

            var response = TransactionService.GetTransactionsByDateRange(sd, ed);
            return Ok(response);
        }

        public IActionResult TransactionsByStatus(string status)
        {
            var response = TransactionService.GetTransactionsByStatus(status);
            return Ok(response);
        }
    }
}
