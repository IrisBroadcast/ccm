using System;
using CCM.Core.Kamailio;
using CCM.Core.Kamailio.Messages;
using CCM.Data.Repositories;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.ServiceTests.SipMessageHandlerTests
{

    [TestFixture, Ignore("")]
    public class SipMessageManagerTests : SipMessageHandlerTestsBase
    {
        [SetUp]
        public void Setup()
        {
            _sipMessageManager = kernel.Get<KamailioMessageManager>();
            _sipRep = kernel.Get<RegisteredSipRepository>();
        }

        [Test]
        public void test_codec_registration()
        {
            var userName = "patpet2@acip.example.com";

            DeleteExisting(userName);

            // Add new
            var sipMessage = new KamailioRegistrationMessage()
            {
                Ip = GetRandomLocationIpAddress(),
                Port = 5060,
                UnixTimeStamp = GetUnixTimeStamp(DateTime.Now),
                Sip = new SipUri(userName),
                UserAgent = "ProntoNet LC v6.8.1",
                Username = userName,
                ToDisplayName = "Test",
                Expires = 60
            };

            // Assert
            var result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.CodecAdded, result.ChangeStatus);

            // Update only timestamp. Should return nothing changed.
            // Act
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(1));
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.NothingChanged, result.ChangeStatus);

            // Update location. Should return status updated.
            // Act
            sipMessage.Ip = GetRandomLocationIpAddress();
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.CodecUpdated, result.ChangeStatus);

            // Only timestamp
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(2));
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.NothingChanged, result.ChangeStatus);

            // Update display name. Should return status updated.
            // Act
            sipMessage.ToDisplayName = "New display name";
            // Assert
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.CodecUpdated, result.ChangeStatus);

            // Update user agent
            sipMessage.UserAgent = "Quantum/3.4.3";
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.CodecUpdated, result.ChangeStatus);

            // Only timestamp
            sipMessage.UnixTimeStamp = GetUnixTimeStamp(DateTime.Now.AddSeconds(3));
            result = _sipMessageManager.RegisterSip(sipMessage);
            Assert.AreEqual(KamailioMessageChangeStatus.NothingChanged, result.ChangeStatus);

        }

      
    }
}
