using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransactionWebApp.Models;
using TransactionWebApp.Services;

namespace TransactionWebApp.Controllers
{
    public class TransactionController : Controller
    {
        public ITransactionService TransactionService { get; set; }
        
        public TransactionController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        [HttpGet("/Transaction/{currencyCode}")]
        public IActionResult GetTransactionsByCurrency(string currencyCode)
        {
            var response = TransactionService.GetTransactionsByCurrency(currencyCode);
            return Ok(response);
        }

        [HttpGet("/Transaction/{startDate}/{endDate}")]
        public IActionResult GetTransactionsByDateRange(string startDate, string endDate)
        {
            var sd = Convert.ToDateTime(startDate);
            var ed = Convert.ToDateTime(endDate);

            var response = TransactionService.GetTransactionsByDateRange(sd, ed);
            return Ok(response);
        }

        [HttpGet("/Transaction/{status}")]
        public IActionResult GetTransactionsByStatus(string status)
        {
            var response = TransactionService.GetTransactionsByStatus(status);
            return Ok(response);
        }
    }
}
