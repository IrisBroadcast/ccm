using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace CCM.Web.Controllers.Api
{
    public class StudioMonitorExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}