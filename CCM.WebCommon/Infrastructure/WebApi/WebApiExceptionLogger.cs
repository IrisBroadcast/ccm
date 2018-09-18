using System.Web.Http.ExceptionHandling;
using NLog;

namespace CCM.WebCommon.Infrastructure.WebApi
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            if (context.Exception != null)
            {
                log.Error(context.Exception, "Error on api call to " + context.Request.RequestUri);
            }

        }
    }
}