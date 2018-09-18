using System;
using System.Configuration;

namespace CCM.DiscoveryApi.Infrastructure
{
    public static class ApplicationSettings
    {
        // URL to CCM Web
        public static Uri CcmHost => new Uri(ConfigurationManager.AppSettings["CCMHost"]);
        public static string DiscoveryUsername => ConfigurationManager.AppSettings["DiscoveryUsername"];
        public static string DiscoveryPassword => ConfigurationManager.AppSettings["DiscoveryPassword"];
        public static string BuildDate => ConfigurationManager.AppSettings["BuildDate"];

    }
}