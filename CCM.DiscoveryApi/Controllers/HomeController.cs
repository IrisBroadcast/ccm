using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using CCM.DiscoveryApi.Infrastructure;

namespace CCM.DiscoveryApi.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Index()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var buildDate = ApplicationSettings.BuildDate;
            var html = $"<html><body><pre>SR Discovery. Version {version}, build date {buildDate}<pre></body></html>";
            var htmlContent = new StringContent(html);
            htmlContent.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return new HttpResponseMessage { Content = htmlContent };
        }

    }
}
