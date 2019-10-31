using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files");
            var uploadedFileName = Path.GetFileName(file.FileName);
            var tempFileId = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(rootPath, tempFileId);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                await file.CopyToAsync(stream);
            }
            
            var transactionModel = new TransactionModel();

            try
            {
                if (System.IO.File.Exists(path))
                {
                    var fileHandler = FileHandler.GetFileHandler(path);
                    transactionModel = fileHandler.GetTranscations(path, uploadedFileName);
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception e)
            {
                // Todo: Add log
            }
            
            if (transactionModel.Errors.Any())
            {
                var errorMsg = string.Join("\n", transactionModel.Errors);
                return BadRequest(errorMsg);
            }

            var isSaved = TransactionService.AddTransactions(transactionModel.Transcations);
            if (isSaved) return Ok("Success!");
            return BadRequest(ErrorConstant.FileUploadFailed);
        }
    }
}
