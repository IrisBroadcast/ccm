using System;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace CCM.DiscoveryApi.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DiscoveryControllerConfigAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            // Only allow XML in the discovery controllers
            XmlMediaTypeFormatter globalXmlFormatterInstance = controllerSettings.Formatters.XmlFormatter;
            controllerSettings.Formatters.Clear();
            controllerSettings.Formatters.Add(globalXmlFormatterInstance);
        }
    }
}