using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NLog;

namespace CCM.Web.Hubs
{
    public abstract class HubBase : Hub
    {
        protected static readonly Logger log = LogManager.GetCurrentClassLogger();
        protected string Referer => Context?.Headers?["Referer"] ?? string.Empty;
        protected string RemoteIp => Context?.Request?.Environment?["server.RemoteIpAddress"] as string ?? string.Empty;

        public override Task OnConnected()
        {
            if (log.IsInfoEnabled)
            {
                log.Info("SignalR client on {0} connected to {1}. Id={2} ({3})", RemoteIp, GetType().Name, Context.ConnectionId, Referer);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                if (log.IsInfoEnabled)
                {
                    log.Info("SignalR client on {0} disconnected gracefully from {1}. Connection id={2} ({3})",
                    RemoteIp, GetType().Name, Context.ConnectionId, Referer);
                }
            }
            else
            {
                log.Warn("SignalR client disconnected ungracefully from {0}. Connection id={1}  ({2})",
                    GetType().Name, Context.ConnectionId, Referer);
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (log.IsInfoEnabled)
            {
                log.Info("SignalR client on {0} reconnected to {1}. Connection id={2} ({3})", 
                    RemoteIp, GetType().Name, Context.ConnectionId, Referer);
            }
            return base.OnReconnected();
        }

    }
}