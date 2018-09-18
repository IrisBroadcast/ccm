using CCM.CodecControl.Mandozzi.Umac;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.CodecControl.Mandozzi.Umac
{
    [TestFixture]
    public class CallInfoTests
    {
        [Test]
        public void should_parse_call_info_when_connected()
        {
            string s = " CONFIRMED [sip:sto-s17-01@acip.example.com]\r\nremote info : sip:sto-s17-01@acip.example.com\r\ncontact info: \"Studio 17 01\" <sip:sto-s17-01@192.0.2.15:5060;transport=UDP>\r\ncodec tx: opus 48k mono 64k [dsp], rx: opus 48k mono 32k [dsp]\r\n";

            var callInfo = new UmacCallInfo(s);

            Assert.AreEqual("sip:sto-s17-01@acip.example.com", callInfo.ConnectedTo);
            Assert.AreEqual("CONFIRMED", callInfo.State);
            Assert.AreEqual("opus 48k mono 64k [dsp]", callInfo.Tx);
            Assert.AreEqual("opus 48k mono 32k [dsp]", callInfo.Rx);
        }
    }
}