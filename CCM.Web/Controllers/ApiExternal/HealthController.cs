using Microsoft.AspNetCore.Mvc;

namespace CCM.Web.Controllers.ApiExternal
{
    /// <summary>
    /// For health checks (used for example during deploy process)
    /// </summary>
    public class HealthController: ControllerBase
    {
        public IActionResult Get() => Ok();
    }
}