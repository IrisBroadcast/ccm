using CCM.Core.Kamailio.Messages;

namespace CCM.Core.Interfaces
{
    public interface IKamailioMessageParser
    {
        KamailioMessageBase Parse(string message);
    }
}