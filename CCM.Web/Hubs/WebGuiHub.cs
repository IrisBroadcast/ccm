using System.Linq;
using System.Web.Mvc;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Web.Mappers;
using NLog;
using Microsoft.AspNet.SignalR;

namespace CCM.Web.Hubs
{
    public class WebGuiHub : HubBase
    {
        private static readonly Throttler CodecsOnlineThrottler;
        private static readonly Throttler OngoingCallsThrottler;
        private static readonly Throttler OldCallsThrottler;

        static WebGuiHub()
        {
            CodecsOnlineThrottler = new Throttler("RegisteredCodecs", 200, UpdateCodecsOnline);
            OngoingCallsThrottler = new Throttler("OnGoingCalls", 300, UpdateOngoingCalls);
            OldCallsThrottler = new Throttler("OldCalls", 400, UpdateOldCalls);
        }

        public static void ThrottlingUpdateCodecsOnline()
        {
            CodecsOnlineThrottler.Trigger();
        }

        private static void UpdateCodecsOnline()
        {
            var registeredSipRepository = (IRegisteredSipRepository)DependencyResolver.Current.GetService(typeof(IRegisteredSipRepository));
            var settingsManager = (ISettingsManager)DependencyResolver.Current.GetService(typeof(ISettingsManager));

            var registeredSipsOnline = registeredSipRepository.GetCachedRegisteredSips();
            var sipDomain = settingsManager.SipDomain;

            var codecsOnline = registeredSipsOnline.Select(sip => RegisteredSipOverviewDtoMapper.MapToDto(sip, sipDomain)).ToList();

            log.Debug("SignalR updating list of codecs online on web gui clients. #Codecs=" + codecsOnline.Count);
            var webGuiHubContext = GlobalHost.ConnectionManager.GetHubContext<WebGuiHub>();
            webGuiHubContext.Clients.All.codecsOnline(codecsOnline);
        }

        public static void ThrottlingUpdateOldCalls()
        {
            OldCallsThrottler.Trigger();
        }

        private static void UpdateOldCalls()
        {
            var callHistoryRepository = (ICallHistoryRepository)DependencyResolver.Current.GetService(typeof(ICallHistoryRepository));
            var settingsManager = (ISettingsManager)DependencyResolver.Current.GetService(typeof(ISettingsManager));
            var oldCalls = callHistoryRepository.GetOldCalls(settingsManager.LatestCallCount, true);

            log.Debug("SignalR updating list of old calls on web gui clients. #Old calls=" + oldCalls.Count);
            var webGuiHubContext = GlobalHost.ConnectionManager.GetHubContext<WebGuiHub>();
            webGuiHubContext.Clients.All.oldCalls(oldCalls);
        }

        public static void ThrottlingUpdateOngoingCalls()
        {
            OngoingCallsThrottler.Trigger();
        }

        private static void UpdateOngoingCalls()
        {
            var callRepository = (ICallRepository)DependencyResolver.Current.GetService(typeof(ICallRepository));
            var onGoingCalls = callRepository.GetOngoingCalls(true);

            log.Debug("SignalR updating list of ongoing calls on web gui clients. #Ongoing calls=" + onGoingCalls.Count);
            var webGuiHubContext = GlobalHost.ConnectionManager.GetHubContext<WebGuiHub>();
            webGuiHubContext.Clients.All.ongoingCalls(onGoingCalls);
        }

    }
}