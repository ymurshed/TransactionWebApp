using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionWebApp.Constants;
using TransactionWebApp.CustomAttribute;
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
            if (file.Length > 0)
            {
                var uploadedFileName = Path.GetFileName(file.FileName);
                var tempFileId = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(CommonConstant.UploadedFiledLocation, tempFileId);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                if (System.IO.File.Exists(path) && new FileInfo(path).Length != 0)
                {
                   // Todo: Create file handler
                }
            }

            return Ok(new { count = 1, file.Length, file });
        }
    }
}
