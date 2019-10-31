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
            var error = LogException(context);
            var routeValues = new RouteValueDictionary(new
            {
                action = "Error",
                controller = "Home",
                logMessage = error
            });
            context.Result = new RedirectToRouteResult(routeValues);
        }

        private static string LogException(ExceptionContext context)
        {
            var error = $"Error Origin: {context.ActionDescriptor.DisplayName} ---> Exception: {context.Exception}";
            Logger.Log.Error(error);
            return error;
        }
    }
}
