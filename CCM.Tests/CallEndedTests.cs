using System;
using System.Linq;
using CCM.Core.Cache;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using CCM.Core.Kamailio.Messages;
using CCM.Core.Kamailio.Parser;
using CCM.Core.Managers;
using CCM.Data.Repositories;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests
{
    [TestFixture]
    public class CallEndedTests
    {
        private readonly KamailioMessageManager _kamailioMessageManager;
        private readonly IRegisteredSipRepository _registeredSipRepository;
        private ICallRepository _callRepository;

        public CallEndedTests()
        {

            var settingsManager = new SettingsManager(new SettingsRepository(new CachingService()));
            var locationManager = new LocationManager(new LocationRepository(new CachingService()));
            var metaRepository = new MetaRepository(new CachingService());
            var registeredSipRepository = new RegisteredSipRepository(settingsManager, locationManager, metaRepository, new CachingService());
            _registeredSipRepository = new CachedRegisteredSipRepository(new CachingService(), registeredSipRepository);

            _kamailioMessageManager = new KamailioMessageManager(
                _registeredSipRepository,
                new CachedCallRepository(
                    new CachingService(), 
                    new CallRepository(new CallHistoryRepository(new CachingService()), settingsManager, new CachingService())
                    ),
                new KamailioMessageParser(new KamailioDataParser())
            );

            _callRepository = new CallRepository(new CallHistoryRepository(new CachingService()), settingsManager, new CachingService());

            _callRepository = new CachedCallRepository(
                new CachingService(), 
                new CallRepository(
                    new CallHistoryRepository(new CachingService()), 
                    settingsManager, 
                    new CachingService()
                    )
            );

            //kamailioMessageManager = new KamailioMessageManager(
            //    GetRegisteredSipRepository(),
            //    new CallRepository(new CallHistoryRepository(), new SettingsManager(new SettingsRepository())),
            //    new KamailioMessageParser(new KamailioDataParser())
            //);
        }

        [Test, Explicit]
        public void when_call_ended_registeredsip_should_be_updated_in_cache()
        {
            try
            {
                // Lägg till registrerade kodare
                var codec1 = "mtu-01@acip.example.com";
                var codec2 = "mtu-02@acip.example.com";

                RegisterSip(codec1, "192.0.2.236", "MTU 01", "QuantumST/3.5.3g");
                RegisterSip(codec2, "192.0.2.4", "MTU 02", "QuantumST/3.5.3g");
                RegisterSip("mtu-31@acip.example.com", "192.0.2.215", "MTU 31", "QuantumST/3.5.3g");

                //PrintCodecStatus(codec1, codec2);

                for (int i = 0; i < 10; i++)
                {
                    // Starta samtal
                    var dialogMessage = new KamailioDialogMessage
                    {
                        FromSipUri = new SipUri(codec1),
                        ToSipUri = new SipUri(codec2),
                        CallId = Guid.NewGuid().ToString(),
                        HashId = "0",
                        HashEntry = "0"
                    };

                    var result = _kamailioMessageManager.RegisterCall(dialogMessage);
                    Console.WriteLine("Call from {0} to {1} started with status {2}", codec1, codec2, result.ChangeStatus);

                    // Läs ut kodare-status
                    PrintCodecStatus(codec1, codec2);

                    // Läs ut samtal
                    var call = _callRepository.GetCallInfo(dialogMessage.CallId, dialogMessage.HashId, dialogMessage.HashId);
                    Console.WriteLine("Call from {0} to {1} started {2}", call.FromSipAddress, call.ToSipAddress, call.Started);

                    // Avsluta samtal
                    var hangupResult = _kamailioMessageManager.CloseCall(dialogMessage);
                    Console.WriteLine("Call from  {0} to {1} ended with result: {2}", codec1, codec2, hangupResult.ChangeStatus);

                    // Läs ut kodare-status
                    PrintCodecStatus(codec1, codec2);
                }
            }
            finally
            {
                TearDown();
            }
        }

        private void PrintCodecStatus(string from, string to)
        {
            var sipRespository = _registeredSipRepository;
            var allRegisteredSipsOnline = sipRespository.GetCachedRegisteredSips();
            var fromCodec = allRegisteredSipsOnline.SingleOrDefault(r => r.Sip == from);
            Console.WriteLine("Codec {0} is in call with {1}", fromCodec.Sip, fromCodec.InCallWithSip);
            var toCodec = allRegisteredSipsOnline.SingleOrDefault(r => r.Sip == to);
            Console.WriteLine("Codec {0} is in call with {1}", toCodec.Sip, toCodec.InCallWithSip);
        }

        public void RegisterSip(string sipAddress, string ip, string displayName, string userAgent)
        {
            var sip = new RegisteredSip
            {
                IP = ip,
                Port = 5060,
                ServerTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                SIP = sipAddress,
                UserAgentHead = userAgent,
                Username = sipAddress,
                DisplayName = displayName,
                Expires = 60
            };

            var changeStatus = _registeredSipRepository.UpdateRegisteredSip(sip);
            Console.WriteLine("Registered {0} with result: {1}", sip.SIP, changeStatus);
        }

        public void TearDown()
        {

        }
    }
}