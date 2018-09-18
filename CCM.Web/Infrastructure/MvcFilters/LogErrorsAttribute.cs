using System.Web.Mvc;
using NLog;

namespace CCM.Web.Infrastructure.MvcFilters
{
    public class LogErrorsAttribute : HandleErrorAttribute 
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                object controller = filterContext.RouteData.Values["controller"];
                object action = filterContext.RouteData.Values["action"];
                string loggerName = string.Format("{0}Controller.{1}", controller, action);

                log.Error(filterContext.Exception, "Exception in " + loggerName);
            }

            base.OnException(filterContext);
        }

    }
}