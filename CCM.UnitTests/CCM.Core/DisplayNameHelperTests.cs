using CCM.Core.Helpers;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core
{
    [TestFixture]
    public class DisplayNameHelperTests
    {
        [Test]
        public void should_hide_external_numbers()
        { 
            var s = "0090510@acip.example.com";
            var result = DisplayNameHelper.AnonymizePhonenumber(s);
            Assert.AreEqual("Externt nummer", result);
        }

        [Test]
        public void should_show_internal_number_without_host()
        {
            var s = "840200@acip.example.com";
            var result = DisplayNameHelper.AnonymizePhonenumber(s);
            Assert.AreEqual("840200", result);
        }

        [Test]
        public void should_not_anonymize_standard_sipaddresses()
        {
            var s = "sto-s17-01@acip.example.com";
            var result = DisplayNameHelper.AnonymizePhonenumber(s);
            Assert.AreEqual(s, result);
        }

        [Test]
        public void should_handle_empty_string()
        {
            var s = string.Empty;
            var result = DisplayNameHelper.AnonymizePhonenumber(s);
            Assert.AreEqual(s, result);
        }

    }
}