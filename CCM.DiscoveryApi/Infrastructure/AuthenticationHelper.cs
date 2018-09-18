using System;
using System.Text;

namespace CCM.DiscoveryApi.Infrastructure
{
    public class AuthenticationHelper
    {
        public static string GetBasicAuthorizationString(string userName, string password)
        {
            return $"Basic {GetAuthString(userName, password)}";
        }

        public static string GetAuthString(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
        }
    }
}