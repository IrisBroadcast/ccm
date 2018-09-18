using CCM.Core.Kamailio.Messages;
using CCM.Core.Kamailio.Parser;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Kamailio
{
    [TestFixture]
    public class KamailioMessageParserTests_Registration
    {
        [Test]
        public void should_parse_correct_registration()
        {
            var msg = "request|rm::REGISTER|fu::sip:vaxjo-04@acip.example.com|si::192.0.2.86|sp::5060|ru::sip:acip.example.net|Ri::192.0.2.200|Rp::5060|ua::ME-UMAC2-M/0.255|rU::|fU::vaxjo-04|ci::2NfTcc9Z25ZRlyZfmnWUOcMeqEHBvidA|tn::\"Växjö 04\"|TS::1433012272|Au::vaxjo-04@acip.example.com|Expires::0";

            var sut = new KamailioMessageParser(new KamailioDataParser());
            var message = sut.Parse(msg);

            var requestMessage = message as KamailioRegistrationMessage;
            Assert.IsNotNull(requestMessage);
            Assert.AreEqual("vaxjo-04@acip.example.com", requestMessage.Sip.UserAtHost);
            Assert.AreEqual("192.0.2.86", requestMessage.Ip);
            Assert.AreEqual("Växjö 04", requestMessage.ToDisplayName);
            Assert.AreEqual("ME-UMAC2-M/0.255", requestMessage.UserAgent);
        }

        [Test]
        public void should_unescape_display_name()
        {
            var msg = "request|rm::REGISTER|fn::\"Växjö 007\"|fu::sip:vaxjo-06@acip.example.com|si::192.0.2.87|sp::5060|ru::sip:acip.example.net|Ri::192.0.2.79|Rp::5060|ua::QuantumST/3.5.0|rU::<null>|fU::vaxjo-06|ci::5JBihczcH-3aF5U6XKu32JHn1Q7M7dRt|tn::\"Växjö 06\"|tu::sip:vaxjo-06@acip.example.com|TS::1461077866|Au::vaxjo-06@acip.example.com|Expires::60";

            var sut = new KamailioMessageParser(new KamailioDataParser());
            var message = sut.Parse(msg);

            var requestMessage = message as KamailioRegistrationMessage;
            Assert.IsNotNull(requestMessage);
            Assert.AreEqual("Växjö 06", requestMessage.ToDisplayName);
            Assert.AreEqual("Växjö 007", requestMessage.FromDisplayName);
        }

        [Test]
        public void should_unescape_display_name2()
        {
            var msg = "\\\"Växjö 007\\\"";
            var unescaped = KamailioMessageParser.ParseDisplayName(msg);

            Assert.AreEqual("Växjö 007", unescaped);
        }

        [Test]
        public void should_use_default_expire_value_when_missing()
        {
            var msg = "request|rm::REGISTER|fu::sip:vaxjo-04@acip.example.com|si::192.0.2.86|sp::5060|ru::sip:acip.example.net|Ri::192.0.2.200|Rp::5060|ua::ME-UMAC2-M/0.255|rU::|fU::vaxjo-04|ci::2NfTcc9Z25ZRlyZfmnWUOcMeqEHBvidA|tn::\"Växjö 04\"|TS::1433012272|Au::vaxjo-04@acip.example.com|Expires::";

            var sut = new KamailioMessageParser(new KamailioDataParser());
            var message = sut.Parse(msg);

            var requestMessage = message as KamailioRegistrationMessage;
            Assert.IsNotNull(requestMessage);
            Assert.AreEqual(120, requestMessage.Expires);
        }

        [Test]
        public void should_use_default_expire_value_when_not_integer()
        {
            var msg = "request|rm::REGISTER|fu::sip:vaxjo-04@acip.example.com|si::192.0.2.86|sp::5060|ru::sip:acip.example.net|Ri::192.0.2.200|Rp::5060|ua::ME-UMAC2-M/0.255|rU::|fU::vaxjo-04|ci::2NfTcc9Z25ZRlyZfmnWUOcMeqEHBvidA|tn::\"Växjö 04\"|TS::1433012272|Au::vaxjo-04@acip.example.com|Expires::<NULL>";

            var sut = CreateKamailioMessageParser();
            var message = sut.Parse(msg);

            var requestMessage = message as KamailioRegistrationMessage;
            Assert.IsNotNull(requestMessage);
            Assert.AreEqual(120, requestMessage.Expires);
        }

        private KamailioMessageParser CreateKamailioMessageParser()
        {
            return new KamailioMessageParser(new KamailioDataParser());
        }
    }
}