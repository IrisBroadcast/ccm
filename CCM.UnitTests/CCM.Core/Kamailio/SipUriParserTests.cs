using CCM.Core.Kamailio;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Kamailio
{
    [TestFixture]
    public class SipUriParserTests
    {
        [Test]
        public void should_parse_sip_address()
        {
            var sipUri = "sip:840202@gw.acip.example.com:5060;user=phone;transport=udp";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("840202", parser.User);
            Assert.AreEqual("gw.acip.example.com", parser.Host);
            Assert.AreEqual("840202@gw.acip.example.com", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }

        [Test]
        public void should_handle_missing_schema()
        {
            var sipUri = "840202@gw.acip.example.com:5060;user=phone;transport=udp";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("", parser.Schema);
            Assert.AreEqual("840202", parser.User);
            Assert.AreEqual("gw.acip.example.com", parser.Host);
            Assert.AreEqual("840202@gw.acip.example.com", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }

        [Test]
        public void should_handle_missing_port()
        {
            var sipUri = "sip:840202@gw.acip.example.com;user=phone;transport=udp";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("840202", parser.User);
            Assert.AreEqual("gw.acip.example.com", parser.Host);
            Assert.AreEqual("840202@gw.acip.example.com", parser.UserAtHost);
            Assert.AreEqual("", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }

        [Test]
        public void should_parse_sip_address_without_user()
        {
            var sipUri = "sip:gw.acip.example.com:5060;user=phone;transport=udp";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("", parser.User);
            Assert.AreEqual("gw.acip.example.com", parser.Host);
            Assert.AreEqual("", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }

        [Test]
        public void should_handle_underscore_address()
        {
            var sipUri = "sip:carl_magnus.von_der_pahlen@acip.example.com";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("carl_magnus.von_der_pahlen", parser.User);
            Assert.AreEqual("acip.example.com", parser.Host);
            Assert.AreEqual("carl_magnus.von_der_pahlen@acip.example.com", parser.UserAtHost);
            Assert.AreEqual("", parser.Port);
            Assert.AreEqual("", parser.Parameters);
        }

        [Test]
        public void should_handle_missing_parameters()
        {
            var sipUri = "sip:840202@gw.acip.example.com:5060";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("840202", parser.User);
            Assert.AreEqual("gw.acip.example.com", parser.Host);
            Assert.AreEqual("840202@gw.acip.example.com", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("", parser.Parameters);
        }

        [Test]
        public void should_handle_displayname()
        {
            var sipUri = "Anders Stenberg <sip:andste@acip.example.com:5060;user=phone;transport=udp>";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("Anders Stenberg", parser.DisplayName);
            Assert.AreEqual("sip", parser.Schema);
            Assert.AreEqual("andste", parser.User);
            Assert.AreEqual("acip.example.com", parser.Host);
            Assert.AreEqual("andste@acip.example.com", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }

        [Test]
        public void should_handle_displayname_without_schema()
        {
            var sipUri = "Anders Stenberg <andste@acip.example.com:5060;user=phone;transport=udp>";
            var parser = new SipUri(sipUri);
            Assert.AreEqual("Anders Stenberg", parser.DisplayName);
            Assert.AreEqual("", parser.Schema);
            Assert.AreEqual("andste", parser.User);
            Assert.AreEqual("acip.example.com", parser.Host);
            Assert.AreEqual("andste@acip.example.com", parser.UserAtHost);
            Assert.AreEqual("5060", parser.Port);
            Assert.AreEqual("user=phone;transport=udp", parser.Parameters);
        }
    }
}