using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using TransactionWebApp.Models;

namespace TransactionWebApp.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            LogException(context);
            var routeValues = new RouteValueDictionary(new
            {
                action = "Error",
                controller = "Error",
                logMessage = $"{context.Exception.Message} \nSee log for details..."
            });
            context.Result = new RedirectToRouteResult(routeValues);
        }

        private static void LogException(ExceptionContext context)
        {
            var error = $"Error Origin: {context.ActionDescriptor.DisplayName} ---> Exception: {context.Exception}";
            Logger.Log.Error(error);
        }
    }
}
