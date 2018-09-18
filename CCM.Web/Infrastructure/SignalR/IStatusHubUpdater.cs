using CCM.Core.Kamailio;

namespace CCM.Web.Infrastructure.SignalR
{
    public interface IStatusHubUpdater
    {
        void Update(KamailioMessageHandlerResult updateResult);
    }
}