using CCM.Core.Kamailio;

namespace CCM.Web.Infrastructure.SignalR
{
    public interface IGuiHubUpdater
    {
        void Update(KamailioMessageHandlerResult updateResult);
    }
}