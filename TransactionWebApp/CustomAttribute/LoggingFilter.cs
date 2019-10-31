using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using TransactionWebApp.Constants;
using TransactionWebApp.Models;

namespace TransactionWebApp.CustomAttribute
{
    public class LoggingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Logger.Log.IsDebugEnabled)
            {
                var loggingWatch = Stopwatch.StartNew();
                context.HttpContext.Items.Add(CommonConstant.StopwatchKey, loggingWatch);

                var controllerName = context.ActionDescriptor.RouteValues.ContainsKey("controller")
                    ? context.ActionDescriptor.RouteValues["controller"] : string.Empty;

                var actionName = context.ActionDescriptor.RouteValues.ContainsKey("action")
                    ? context.ActionDescriptor.RouteValues["action"] : string.Empty;

                var message = $"Executing /{controllerName}/{actionName}";
                Logger.Log.Debug(message);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (Logger.Log.IsDebugEnabled)
            {
                if (context.HttpContext.Items[CommonConstant.StopwatchKey] != null)
                {
                    var loggingWatch = (Stopwatch)context.HttpContext.Items[CommonConstant.StopwatchKey];
                    loggingWatch.Stop();

                    var timeSpent = loggingWatch.ElapsedMilliseconds;

                    var controllerName = context.ActionDescriptor.RouteValues.ContainsKey("controller")
                        ? context.ActionDescriptor.RouteValues["controller"] : string.Empty;

                    var actionName = context.ActionDescriptor.RouteValues.ContainsKey("action")
                        ? context.ActionDescriptor.RouteValues["action"] : string.Empty;

                    var message = $"Finished executing /{controllerName}/{actionName} - time spend: {timeSpent} ms";

                    Logger.Log.Debug(message);
                    context.HttpContext.Items.Remove(CommonConstant.StopwatchKey);
                }
            }
        }
    }
}
