using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using NLog;

namespace CCM.Web.Infrastructure.WebApiFilters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var ex = actionExecutedContext.Exception;
            log.Warn(ex, "Exception in API controller method");

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent("An error occured and has been logged.", System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            base.OnException(actionExecutedContext);
        }
    }
}