using CCM.Core.Kamailio.Messages;
using CCM.Core.Kamailio.Parser;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Kamailio
{
    [TestFixture]
    public class KamailioMessageParserTests_Dialog
    {
      
        [Test]
        public void should_parse_correct_dialog_start()
        {
            var msg = "dialog|dstat::start|hashid::9987|hashent::1646|ci::2129973808|tot::|fot::611081311|fu::sip:username@acip.example.com|ru::sip:sto-s17-01@acip.example.com:5060|ms::";

            var sut = CreateKamailioMessageParser();
            var message = sut.Parse(msg);

            var dialogMessage = message as KamailioDialogMessage;
            Assert.IsNotNull(dialogMessage);
            Assert.AreEqual("2129973808", dialogMessage.CallId);
            Assert.AreEqual("username@acip.example.com", dialogMessage.FromSipUri.UserAtHost);
            Assert.AreEqual("sto-s17-01@acip.example.com", dialogMessage.ToSipUri.UserAtHost);
            Assert.AreEqual("611081311", dialogMessage.FromTag);
            Assert.AreEqual("", dialogMessage.ToTag);
        }

        [Test]
        public void should_unescape_display_name2()
        {
            var msg = "\\\"Växjö 007\\\"";
            var unescaped = KamailioMessageParser.ParseDisplayName(msg);

            Assert.AreEqual("Växjö 007", unescaped);
        }

        [Test]
        public void should_use_tu_if_ru_is_empty()
        {
            var msg = "dialog|dstat::start|hashid::3091|hashent::3275|ci::GExUyxXKGrCRIHTbJW7xQmAOA43sDDD0|tot::00fc17233983d37c88de|fot::-RC1xwiQQjNToWkDcYhrMmqqa.uu.RkC|from_uri::sip:vaxjo-06@acip.example.com|to_uri::sip:vaxjo-10@acip.example.com|ms::|ru::<null>|fn::\"Växjö 06\"|fu ::sip:vaxjo-06@acip.example.com|tn::\"Vaxjo 10\"|tu::sip:vaxjo-10@acip.example.com|sdp::v=0 o=vaxjo-10 3670076026 3670076027 IN IP4 acip.example.net s=Vaxjo10 c=IN IP4 192.0.2.93 t=0 0 m=audio 5004 RTP/AVP 96 a=rtpmap:96 L24/48000/2 a=ebuacip:plength 96 5 a=ebuacip:jb 0 a=ebuacip:jbdef 0 fixed 6 a=ebuacip:qosrec 136";

            var sut = CreateKamailioMessageParser();
            var message = sut.Parse(msg);

            var dialogMessage = message as KamailioDialogMessage;
            Assert.IsNotNull(dialogMessage);
            Assert.AreEqual("vaxjo-10@acip.example.com", dialogMessage.ToSipUri.UserAtHost);
        }

        private KamailioMessageParser CreateKamailioMessageParser()
        {
            return new KamailioMessageParser(new KamailioDataParser());
        }
 
    }
}