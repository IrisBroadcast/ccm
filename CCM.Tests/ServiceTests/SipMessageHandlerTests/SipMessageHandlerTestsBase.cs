using System;
using System.Linq;
using CCM.Core.Interfaces.Managers;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using CCM.Core.Kamailio.Messages;
using CCM.Core.Managers;
using CCM.Core.Service;
using CCM.Data;
using CCM.Data.Repositories;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.ServiceTests.SipMessageHandlerTests
{
    public class SipMessageHandlerTestsBase
    {
        #region Plumbing
        protected StandardKernel kernel;
        protected DiscoveryService discoveryService;
        protected KamailioMessageManager _sipMessageManager;
        protected RegisteredSipRepository _sipRep;

        protected static StandardKernel GetKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ISipMessageManager>().To<KamailioMessageManager>();
            kernel.Bind<IRegisteredSipRepository>().To<RegisteredSipRepository>();
            kernel.Bind<ICallRepository>().To<CallRepository>();
            kernel.Bind<ICallHistoryRepository>().To<CallHistoryRepository>();
            kernel.Bind<ISettingsRepository>().To<SettingsRepository>();
            kernel.Bind<ISettingsManager>().To<SettingsManager>();
            kernel.Bind<ILocationManager>().To<LocationManager>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            kernel.Bind<IMetaRepository>().To<MetaRepository>();
            return kernel;
        }

        [OneTimeSetUp]
        public void SetupFixture()
        {
            kernel = GetKernel();

        }
        #endregion

        public void DeleteExisting(string userName)
        {
            var existingSip = _sipRep.Single(rs => rs.SIP == userName);
            if (existingSip != null)
            {
                _sipRep.DeleteRegisteredSip(existingSip.Id);
            }
        }

        public KamailioRegistrationMessage CreateSipMessage(string ip, string userAgent, string sip, string displayName)
        {
            return new KamailioRegistrationMessage
            {
                Ip = ip,
                Port = 5060,
                UnixTimeStamp = GetUnixTimeStamp(DateTime.Now),
                Sip = new SipUri(sip),
                UserAgent = userAgent,
                Username = sip,
                ToDisplayName = displayName,
                Expires = 60
            };
        }

        public static long GetUnixTimeStamp(DateTime dateTime)
        {
            return (long)dateTime.Subtract(DateTime.Parse("1970-01-01")).TotalSeconds;
        }

        public static string GetRandomUserName()
        {
            var users = new CcmDbContext(null).SipAccounts.Select(u => u.UserName).ToList();
            int randomIndex = new Random().Next(0, users.Count);
            var userName = users[randomIndex];
            return userName;
        }

        public static string GetRandomLocationIpAddress()
        {
            var locations = new CcmDbContext(null).Locations.Select(l => l.Net_Address_v4).ToList();
            int randomIndex = new Random().Next(0, locations.Count);
            var locationAddress = locations[randomIndex];
            return locationAddress;
        }

    }
}