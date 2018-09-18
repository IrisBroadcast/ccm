using CCM.Core.Kamailio.Messages;
using CCM.Core.Kamailio.Parser;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Kamailio
{
    [TestFixture]
    public class KamailioDataParserTests
    {
        [Test]
        public void should_handle_empty_data()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("");

            Assert.IsNull(data);
        }

        [Test]
        public void should_handle_string_without_divider()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("en sträng utan pipetecken");

            Assert.IsNull(data);
        }

        [Test]
        public void should_handle_string_with_only_header()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|");

            Assert.IsNotNull(data);
            Assert.AreEqual(KamailioMessageType.Dialog, data.MessageType);
        }

        [Test]
        public void should_handle_correct_message()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|dstat::start|hashid::abc|hashent::def|ci::ghi");

            Assert.AreEqual(KamailioMessageType.Dialog, data.MessageType);
            Assert.AreEqual("start", data.Fields["dstat"]);
            Assert.AreEqual("abc", data.Fields["hashid"]);
            Assert.AreEqual("def", data.Fields["hashent"]);
            Assert.AreEqual("ghi", data.Fields["ci"]);
        }

        [Test]
        public void should_exclude_field_with_invalid_form()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|dstat::start|hashid::abc|hashent::apa::banan");
            Assert.IsFalse(data.Fields.ContainsKey("hashent"));
        }

        [Test]
        public void should_exclude_empty_field_key()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|dstat::start|::123|hashent::banan");
            Assert.IsTrue(data.Fields.Count == 2);
        }

        [Test]
        public void should_accept_empty_field_value()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|dstat::start|hashid::|hashent::apa::banan");
            Assert.IsTrue(data.Fields.ContainsKey("hashid"));
        }

        [Test]
        public void should_replace_null_string_with_empty_string()
        {
            var sut = new KamailioDataParser();
            var data = sut.ParseToKamailioData("dialog|dstat::start|hashid::<null>|hashent::");
            Assert.IsTrue(data.Fields.ContainsKey("hashid"));
            Assert.AreEqual(string.Empty, data.Fields["hashid"]);
        }

    }
}