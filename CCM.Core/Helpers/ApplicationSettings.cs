using System;
using System.Configuration;

namespace CCM.Core.Helpers
{
    public static class ApplicationSettings
    {

        public static Uri DiscoveryHost => new Uri(ConfigurationManager.AppSettings["DiscoveryHost"]);
        public static string DiscoveryLogLevelUrl => new Uri(DiscoveryHost, "api/loglevel").ToString();

        // URL to CCM web, used in Discovery service
        public static Uri CcmHost => new Uri(ConfigurationManager.AppSettings["CCMHost"]);

        public static int CacheTimeLiveData => Int32.Parse(ConfigurationManager.AppSettings["CacheTimeLiveData"]);
        public static int CacheTimeConfigData => Int32.Parse(ConfigurationManager.AppSettings["CacheTimeConfigData"]);

    }
}