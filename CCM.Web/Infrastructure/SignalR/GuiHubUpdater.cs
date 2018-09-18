using CCM.Core.Kamailio;
using CCM.Web.Hubs;
using NLog;

namespace CCM.Web.Infrastructure.SignalR
{
    /// <summary>
    /// Uppdaterar klienter via SignalR
    /// </summary>
    public class GuiHubUpdater : IGuiHubUpdater
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Update(KamailioMessageHandlerResult updateResult)
        {
            log.Debug("GuiHubUpdater is updating. status={0}, id={1}", updateResult.ChangeStatus, updateResult.ChangedObjectId);

            if (updateResult.ChangeStatus == KamailioMessageChangeStatus.CallStarted)
            {
                WebGuiHub.ThrottlingUpdateOngoingCalls();
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }

            if (updateResult.ChangeStatus == KamailioMessageChangeStatus.CallClosed)
            {
                WebGuiHub.ThrottlingUpdateOngoingCalls();
                WebGuiHub.ThrottlingUpdateOldCalls();
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }

            if (updateResult.ChangeStatus == KamailioMessageChangeStatus.CodecAdded || 
                updateResult.ChangeStatus == KamailioMessageChangeStatus.CodecUpdated ||
                updateResult.ChangeStatus == KamailioMessageChangeStatus.CodecRemoved)
            {
                WebGuiHub.ThrottlingUpdateCodecsOnline();
            }
        }
    }
}