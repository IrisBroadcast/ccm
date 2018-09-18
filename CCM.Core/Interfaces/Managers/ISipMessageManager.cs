using CCM.Core.Kamailio;

namespace CCM.Core.Interfaces.Managers
{
    public interface ISipMessageManager
    {
        KamailioMessageHandlerResult HandleMessage(string message);
    }
}
