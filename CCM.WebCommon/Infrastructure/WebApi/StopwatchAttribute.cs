using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NLog;

namespace CCM.WebCommon.Infrastructure.WebApi
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private readonly bool _logStartAction = true;
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        private const string StopwatchKey = "StopwatchKey";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (log.IsTraceEnabled)
            {
                if (_logStartAction)
                {
                    IEnumerable<string> headers = actionContext.Request.Headers.Select(h => string.Format("{0}:{1}", h.Key, string.Join(", ", h.Value)));
                    log.Trace("BEGIN Request to {0} [{1}]", actionContext.Request.RequestUri.AbsolutePath, string.Join("; ", headers));
                }
                var stopWatch = Stopwatch.StartNew();
                actionContext.Request.Properties[StopwatchKey] = stopWatch;
                stopWatch.Reset();
                stopWatch.Start();
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (log.IsTraceEnabled)
            {
                var stopwatch = actionExecutedContext.Request.Properties[StopwatchKey] as Stopwatch;
                if (stopwatch != null)
                {
                    stopwatch.Stop();
                    log.Trace("END Request to {0} took {1} ms", actionExecutedContext.Request.RequestUri.AbsolutePath, stopwatch.ElapsedMilliseconds);
                }
            }

        }
    }

}