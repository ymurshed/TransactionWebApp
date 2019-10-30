using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using TransactionWebApp.Constants;
using TransactionWebApp.Models;

namespace TransactionWebApp.CustomAttribute
{
    public class FileMetadataValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey(CommonConstant.File) &&
                context.ActionArguments[CommonConstant.File] is FormFile file)
            {
                var maxSize = AppDataModel.Configuration.GetValue<int>("MaxFileSizeInMb"); 
                var totalSize = maxSize * CommonConstant.BytesFor1Mb;
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension != CommonConstant.Csv && fileExtension != CommonConstant.Xml)
                {
                    context.Result = new BadRequestObjectResult(ExceptionConstant.UnknownFileFormat);
                    return;
                }

                if (file.Length > totalSize)
                {
                    var errorMsg = string.Format(ExceptionConstant.FileSizeExceeds, maxSize);
                    context.Result = new BadRequestObjectResult(errorMsg);
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
