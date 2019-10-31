using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TransactionWebApp.Models;

namespace TransactionWebApp.Controllers
{
    public class ErrorController : Controller
    {
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
