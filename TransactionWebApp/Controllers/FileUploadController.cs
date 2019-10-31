using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using TransactionWebApp.Filters;
using TransactionWebApp.Helpers;
using TransactionWebApp.Models;
using TransactionWebApp.Services;
using TransactionWebApp.Utility.Constants;

namespace TransactionWebApp.Controllers
{
    public class FileUploadController : Controller
    {
        public ITransactionService TransactionService { get; set; }

        public FileUploadController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("FileUpload")]
        [FileMetadataValidationFilter]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            Logger.Log.Debug(LogConstant.FileUploadBeginning);

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files");
            var uploadedFileName = Path.GetFileName(file.FileName);
            var tempFileId = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(rootPath, tempFileId);

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                await file.CopyToAsync(stream);
                Logger.Log.Debug(LogConstant.TempFileSaved);
            }

            TransactionModel transactionModel;
            if (System.IO.File.Exists(path))
            {
                var fileHandler = FileHandlerFactory.GetFileHandler(path);
                transactionModel = fileHandler.GetTranscations(path, uploadedFileName);
            }
            else
            {
                Logger.Log.Debug(LogConstant.TempFileNotSaved);
                return BadRequest(LogConstant.FileUploadFailed);
            }

            try
            {
                System.IO.File.Delete(path);
                Logger.Log.Debug(LogConstant.DeletedTemporarilyFile);
            }
            catch (Exception e)
            {
                Logger.Log.Warn(LogConstant.DeleteFileError + e.Message + e.InnerException);
            }
            
            if (transactionModel == null)
            {
                throw new Exception(LogConstant.ErrorProcessingFile);
            }
            Logger.Log.Debug(LogConstant.ReceivedTransactionModel);

            if (transactionModel.Errors.Any())
            {
                var errorMsg = string.Join("\n", transactionModel.Errors);
                Logger.Log.Error(errorMsg); // Invalid items
                return BadRequest(errorMsg);
            }

            var isSaved = TransactionService.AddTransactions(transactionModel.Transcations);
            if (isSaved)
            {
                Logger.Log.Info(LogConstant.FileUploadSuccessful);
                return Ok(LogConstant.FileUploadSuccessful);
            }

            Logger.Log.Error(LogConstant.FileUploadFailed);
            return BadRequest(LogConstant.FileUploadFailed);
        }
    }
}
