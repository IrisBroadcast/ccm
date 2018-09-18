using System;
using System.Text.RegularExpressions;

namespace CCM.Core.Kamailio
{
    public class SipUri
    {
        private readonly string pattern = @"^((?<scheme>[a-zA-Z]*):)?" // scheme
                          + @"((?<user>[a-zA-Z0-9\-\.\!\~\*\'\(\)&=\+\$,;\?\/\%_]+)@)?" // user
                          + @"(?<host>[^;\?:]*)(:(?<port>[\d]+))?" // host, port
                          + @"(;(?<params>[\S]*))?$"; // parameters

        public string DisplayName { get; set; }
        public string Schema { get; set; }
        public string User { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Parameters { get; set; }
        public string UserAtHost { get { return string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Host) ? string.Empty : string.Format("{0}@{1}", User, Host); } }

        private readonly string _sipString; // Orginalsträngen

        public SipUri(string sipAddress)
        {
            try
            {
                // Handle display name. TODO: Hur gör man detta med RegExp?
                string displayName = "";
                if (sipAddress.Contains("<"))
                {
                    var indexOfLessThan = sipAddress.IndexOf('<');
                    var indexOfMoreThan = sipAddress.IndexOf('>');
                    displayName = sipAddress.Substring(0, indexOfLessThan).Trim();
                    sipAddress = sipAddress.Substring(indexOfLessThan + 1, indexOfMoreThan - indexOfLessThan - 1);
                }

                _sipString = sipAddress;
                var regExp = new Regex(pattern, RegexOptions.IgnoreCase);

                Match match = regExp.Match(sipAddress);
                if (match.Success)
                {
                    DisplayName = displayName;
                    Schema = match.Groups["scheme"].Value;
                    User = match.Groups["user"].Value;
                    Host = match.Groups["host"].Value;
                    Port = match.Groups["port"].Value;
                    Parameters = match.Groups["params"].Value;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid SIP address", ex);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", UserAtHost, _sipString);
        }
    }
}