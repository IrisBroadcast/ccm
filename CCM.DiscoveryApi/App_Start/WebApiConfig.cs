using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using CCM.WebCommon.Infrastructure;
using CCM.WebCommon.Infrastructure.WebApi;

namespace CCM.DiscoveryApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Formatters.XmlFormatter.WriterSettings.OmitXmlDeclaration = false;
            config.Formatters.XmlFormatter.Indent = true;

            config.EnableCors(new EnableCorsAttribute("*","*","*"));
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());
            config.Filters.Add(new StopwatchAttribute());
        }
    }
}
    