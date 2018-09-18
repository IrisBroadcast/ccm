using CCM.Web.Models.ApiExternal;
using Microsoft.AspNet.SignalR;

namespace CCM.Web.Hubs
{
    public class CodecStatusHub : HubBase
    {
        public static void UpdateCodecStatus(CodecStatus codecStatus)
        {
            log.Info("SignalR is sending codec status to clients: {0}", codecStatus);
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CodecStatusHub>();
            hubContext.Clients.All.codecStatus(codecStatus);
        }
        
    }
}