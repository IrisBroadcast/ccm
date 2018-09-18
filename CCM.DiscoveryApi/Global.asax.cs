using System.Web.Http;
using CCM.WebCommon.Infrastructure.WebApi;

namespace CCM.DiscoveryApi
{

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configuration.Filters.Add(new StopwatchAttribute());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
