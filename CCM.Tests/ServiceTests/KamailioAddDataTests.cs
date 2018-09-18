using System;
using CCM.Core.Entities;
using CCM.Core.Kamailio;
using CCM.Core.Kamailio.Messages;
using CCM.Core.Managers;
using CCM.Tests.ServiceTests.SipMessageHandlerTests;
using Ninject;
using NUnit.Framework;

namespace CCM.Tests.ServiceTests
{
    [TestFixture, Ignore("")]
    public class KamailioAddDataTests : SipMessageHandlerTestsBase
    {

        [Test, Explicit]
        public void register_växjö_10()
        {
            var sipMessageManager = kernel.Get<KamailioMessageManager>();
            var sipMessage = CreateSipMessage("192.0.2.82", "ProntoNet LC v6.8.1", "vaxjo-10@acip.example.com", "Växjö 10");
            sipMessageManager.RegisterSip(sipMessage);
        }

        [Test, Explicit]
        public void register_växjö_11()
        {
            var sipMessageManager = kernel.Get<KamailioMessageManager>();
            var sipMessage = CreateSipMessage("192.0.2.83", "ProntoNet LC v6.8.1", "vaxjo-11@acip.example.com", "Växjö 11");
            sipMessageManager.RegisterSip(sipMessage);
        }

        [Test, Explicit]
        public void StartCall()
        {
            var sipMessageManager = kernel.Get<KamailioMessageManager>();
            var sipMessage = CreateCallStartMessage("ob142254@acip.example.com", "sto-04@acip.example.com");
            sipMessageManager.RegisterCall(sipMessage);
        }

        public KamailioDialogMessage CreateCallStartMessage(string sip, string requestedSip)
        {
            return new KamailioDialogMessage
            {
                FromSipUri = new SipUri(sip),
                ToSipUri = new SipUri(requestedSip),
                CallId = "mycallid",
                ToTag = "ToTagTest",
                FromTag = "FromTagTest"
            };
        }


    }
}
