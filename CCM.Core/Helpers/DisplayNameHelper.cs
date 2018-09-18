using System;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Extensions;

namespace CCM.Core.Helpers
{
    public class DisplayNameHelper
    {
        public static string GetDisplayName(RegisteredSip registeredSip, string sipDomain)
        {
            return GetDisplayName(registeredSip.DisplayName, registeredSip.User != null ? registeredSip.User.DisplayName : string.Empty, string.Empty, registeredSip.Username, registeredSip.SIP, "", sipDomain);
        }

        public static string GetDisplayName(RegisteredSipDto registeredSip, string sipDomain)
        {
            return GetDisplayName(registeredSip.DisplayName, registeredSip.UserDisplayName, string.Empty, registeredSip.UserName, registeredSip.Sip, "", sipDomain);
        }

        public static string GetDisplayName(string primaryDisplayName, string secondaryDisplayName, string tertiaryDisplayName, string primaryUserName, string secondaryUserName, string tertiaryUserName,
            string homeDomain, string defaultDisplayName = "")
        {
            if (!string.IsNullOrWhiteSpace(primaryDisplayName)) { return primaryDisplayName; }
            if (!string.IsNullOrWhiteSpace(secondaryDisplayName)) { return secondaryDisplayName; }
            if (!string.IsNullOrEmpty(tertiaryDisplayName)) { return tertiaryDisplayName; }

            if (!string.IsNullOrEmpty(primaryUserName)) { return GetUserNameWithoutHomeDomain(primaryUserName, homeDomain); }
            if (!string.IsNullOrEmpty(secondaryUserName)) { return GetUserNameWithoutHomeDomain(secondaryUserName, homeDomain); }
            if (!string.IsNullOrEmpty(tertiaryUserName)) { return GetUserNameWithoutHomeDomain(tertiaryUserName, homeDomain); }

            return defaultDisplayName;
        }

        private static string GetUserNameWithoutHomeDomain(string userName, string homeDomain)
        {
            var domainIndex = userName.IndexOf(string.Format("@{0}", homeDomain), StringComparison.CurrentCulture);
            if (domainIndex > 0)
            {
                return userName.Remove(domainIndex);
            }
            return userName;
        }

        public static string AnonymizePhonenumber(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = s.Trim();

            var username = s.LeftOf("@");

            if (username.IsNumeric())
            {
                // Telefonnummer
                if (username.Length <= 6)
                {
                    // Intern anknytning
                    return username;
                }

                return "Externt nummer";
            }
            
            return s;
        }


        public static string AnonymizeDisplayName(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = s.Trim();

            var username = s.LeftOf("@");

            if (username.IsNumeric())
            {
                // Telefonnummer
                if (username.Length <= 6)
                {
                    // Intern anknytning
                    return string.Format("Ank {0}", username);
                }

                return "Externt nummer";
            }

            return s;
        }

        
    }
}