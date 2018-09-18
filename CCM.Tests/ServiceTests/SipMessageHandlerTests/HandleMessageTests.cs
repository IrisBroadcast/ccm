using CCM.Core.Kamailio;
using CCM.Data.Repositories;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.ServiceTests.SipMessageHandlerTests
{
    [TestFixture, Ignore("Integration tests")]
    public class HandleMessageTests : SipMessageHandlerTestsBase
    {
        [Test]
        public void should_register_new_codec()
        {
            var sipMessageManager = kernel.Get<KamailioMessageManager>();
            var sipRep = kernel.Get<RegisteredSipRepository>();
            
            // ASSIGN
            var userName = "patpet2@acip.example.com";

            // Delete any already registered codec
            var existingSip = sipRep.Single(rs => rs.SIP == userName);
            if (existingSip != null)
            {
                sipRep.DeleteRegisteredSip(existingSip.Id);
            }
            
            var ipAddress = GetRandomLocationIpAddress();
            var displayName = "Test";

            var sipMessage = CreateSipMessage(ipAddress, "ME-UMAC2-M/0.255", userName, displayName);

            // ACT
            KamailioMessageHandlerResult result = sipMessageManager.DoHandleMessage(sipMessage);

            // ASSERT
            Assert.AreEqual(KamailioMessageChangeStatus.CodecAdded, result.ChangeStatus);

            var sip = sipRep.Single(rs => rs.SIP == userName);
            Assert.AreEqual(ipAddress, sip.IP);
            Assert.AreEqual(userName, sip.User.UserName);

            // Clean up
            sipRep.DeleteRegisteredSip(sip.Id);
        }
    }
}