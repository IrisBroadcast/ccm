using System.ComponentModel;

namespace CCM.Core.Enums
{
    public enum SipAccountType
    {
        [Description("Vanligt SIP-konto")] SipAccount = 0,
        [Description("SIP-alias")] SipAlias = 1,
        [Description("Pool-kodare")] PoolCodec = 2
    }
}