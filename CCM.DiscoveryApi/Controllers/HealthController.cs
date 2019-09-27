using System.Web.Http;

namespace CCM.Web.Controllers.ApiExternal
{
    /// <summary>
    /// For health checks (used for example during deploy process)
    /// </summary>
    public class HealthController : ApiController
    {
        public IHttpActionResult Get() => Ok();
    }
}