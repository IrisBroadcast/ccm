using CCM.Core.Helpers;

namespace CCM.Data.Helpers
{
    public class CallDisplayNameHelper
    {
        public static string GetDisplayName(Entities.RegisteredSipEntity regSip, string callDisplayName, string callUserName, string sipDomain)
        {
            return DisplayNameHelper.GetDisplayName(
                regSip != null ? regSip.DisplayName : string.Empty,
                regSip != null && regSip.User != null ? regSip.User.DisplayName : string.Empty,
                callDisplayName,
                regSip != null ? regSip.Username : string.Empty,
                regSip != null ? regSip.SIP : string.Empty,
                callUserName,
                sipDomain);
        }
    }
}
