using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using TransactionWebApp.Constants;
using TransactionWebApp.CustomAttribute;
using TransactionWebApp.Helpers;
using TransactionWebApp.Models;
using TransactionWebApp.Services;

namespace TransactionWebApp.Controllers
{
    public class FileUploadController : Controller
    {
        public ITransactionService TransactionService { get; set; }

        public FileUploadController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        [HttpPost("FileUpload")]
        [FileMetadataValidationFilter]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var tempServerFilePath = AppDataModel.Configuration.GetValue<string>("ServerFilePath");

            var uploadedFileName = Path.GetFileName(file.FileName);
            var tempFileId = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(tempServerFilePath, tempFileId);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var transactionModel = new TransactionModel();

            if (System.IO.File.Exists(path) && new FileInfo(path).Length != 0)
            {
                var fileHandler = FileHandler.GetFileHandler(path);
                transactionModel = fileHandler.GetTranscations(path, uploadedFileName);
            }

            // Todo: Need to handle
            //System.IO.File.Delete(path);

            if (transactionModel.Errors.Any())
            {
                var errorMsg = string.Join("!", transactionModel.Errors);
                return BadRequest(errorMsg);
            }

            var isSaved = TransactionService.AddTransactions(transactionModel.Transcations);
            if (isSaved) return Ok();
            return BadRequest("File upload failed!");
        }
    }
}
