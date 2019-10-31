using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using TransactionWebApp.Models;
using TransactionWebApp.Utility.Constants;

namespace TransactionWebApp.Filters
{
    public class FileMetadataValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.Any())
            {
                context.Result = new BadRequestObjectResult(LogConstant.NoFileSelected);
                return;
            }

            if (context.ActionArguments.ContainsKey(CommonConstant.File) &&
                context.ActionArguments[CommonConstant.File] is FormFile file)
            {
                var maxSize = Config.Configuration.GetValue<int>("MaxFileSizeInMb"); 
                var totalSize = maxSize * CommonConstant.BytesFor1Mb;
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension != CommonConstant.Csv && fileExtension != CommonConstant.Xml)
                {
                    context.Result = new BadRequestObjectResult(LogConstant.UnknownFileFormat);
                    return;
                }

                if (file.Length == 0)
                {
                    context.Result = new BadRequestObjectResult(LogConstant.EmptyFile);
                    return;
                }

                if (file.Length > totalSize)
                {
                    context.Result = new BadRequestObjectResult($"{LogConstant.FileSizeExceeds}{maxSize}Mb");
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
