using CCM.Core.Kamailio;
using CCM.Data.Repositories;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.ServiceTests.SipMessageHandlerTests
{
    [TestFixture, Ignore("")]
    public class KamailioEventTests : SipMessageHandlerTestsBase
    {

        [Test]
        public void should_register_växjö_10()
        {
            var sipMessageManager = kernel.Get<KamailioMessageManager>();

            var sipMessage = CreateSipMessage("192.0.2.82", "ProntoNet LC v6.8.1", "vaxjo-10@acip.example.com", "Växjö 10");
            sipMessageManager.RegisterSip(sipMessage);

            var sipRep = kernel.Get<RegisteredSipRepository>();
            var sip = sipRep.Single(rs => rs.SIP == "vaxjo-10@acip.example.com");

            Assert.IsNotNull(sip);
            Assert.AreEqual("192.0.2.82", sip.IP);
            Assert.AreEqual("RH Växjö", sip.Location.Name);
            Assert.AreEqual("ProntoNet", sip.UserAgent.Name);
            Assert.AreEqual("vaxjo-10@acip.example.com", sip.User.UserName);
        }

       

        [Test]
        public void should_only_update_timestamp_when_same_registered_sip_and_ccm_is_unchanged()
        {
            // TODO: Implement this
        }


    }
}
