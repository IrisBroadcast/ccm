using System.Diagnostics;
using System.Web.Mvc;
using NLog; 

namespace CCM.Web.Infrastructure.MvcFilters
{
    public class ElapsedTimeFilter : IActionFilter
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private const string stopwatchKey = "stopwatch";

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (log.IsDebugEnabled)
            {
                var stopWatch = Stopwatch.StartNew();
                filterContext.HttpContext.Items[stopwatchKey] = stopWatch;
                stopWatch.Reset();
                stopWatch.Start();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (log.IsDebugEnabled)
            {
                var stopwatch = (Stopwatch)filterContext.HttpContext.Items[stopwatchKey];
                if (stopwatch != null)
                {
                    var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    var action = filterContext.ActionDescriptor.ActionName;
                    log.Debug("Execution of {0}.{1} took {2} ms", controller, action, stopwatch.ElapsedMilliseconds);
                }
            }
        }
    }

}

