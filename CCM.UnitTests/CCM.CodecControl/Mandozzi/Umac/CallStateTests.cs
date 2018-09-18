using CCM.CodecControl.Mandozzi.Umac;
using CCM.Core.CodecControl.Enums;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.CodecControl.Mandozzi.Umac
{
    [TestFixture]
    public class CallStateTests
    {
        [Test]
        public void should_parse_call_state_when_idle()
        {
            string s = " CALL_STATE disconnected [tx: idle, rx: ödla]\r\n";

            var callInfo = new UmacCallState(s);

            Assert.AreEqual(LineStatusCode.Disconnected, callInfo.State);
            Assert.AreEqual("idle", callInfo.TxProtocol);
            Assert.AreEqual("ödla", callInfo.RxProtocol);
        }

        [Test]
        public void should_parse_call_state_when_connected()
        {
            string s = " CALL_STATE connected [tx: sip, rx: sup]";

            var callInfo = new UmacCallState(s);

            Assert.AreEqual(LineStatusCode.ConnectedCalled, callInfo.State);
            Assert.AreEqual("sip", callInfo.TxProtocol);
            Assert.AreEqual("sup", callInfo.RxProtocol);
        }
    }
}

