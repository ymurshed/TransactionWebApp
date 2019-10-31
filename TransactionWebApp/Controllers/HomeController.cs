using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TransactionWebApp.Models;
using TransactionWebApp.Services;

namespace TransactionWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ITransactionService TransactionService { get; set; }

        public HomeController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Transaction()
        {
            return View();
        }

        public IActionResult Error(string logMessage)
        {
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorDetails = logMessage
            };

            return View(viewModel);
        }
    }
}
