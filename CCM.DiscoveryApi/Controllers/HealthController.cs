using Microsoft.AspNetCore.Mvc;

namespace CCM.DiscoveryApi.Controllers
{
    /// <summary>
    /// For health checks (used for example during deploy process)
    /// </summary>
    public class HealthController : Controller
    {
        [HttpGet]
        [Route("/health")]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}