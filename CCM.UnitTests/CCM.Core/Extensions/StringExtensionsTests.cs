using CCM.Core.Extensions;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Core.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void LeftOf_At()
        {
            var s = "apanFlyger@acip.example.com";
            var r = s.LeftOf("@");
            Assert.AreEqual("apanFlyger", r);
        }

        [Test]
        public void LeftOf_String()
        {
            var s = "apanFlyger@acip.example.com";
            var r = s.LeftOf(".com");
            Assert.AreEqual("apanFlyger@acip.example", r);
        }

        [Test]
        public void LeftOf_String2()
        {
            var s = "grodlår i låda";
            var r = s.LeftOf("r i");
            Assert.AreEqual("grodlå", r);
        }

        [Test]
        public void LeftOf_EmptyUntilString()
        {
            var s = "grodlår i låda";
            var r = s.LeftOf("");
            Assert.AreEqual("grodlår i låda", r);
        }

        [Test]
        public void LeftOf_EmptyString()
        {
            var s = "";
            var r = s.LeftOf("@");
            Assert.AreEqual("", r);
        }

        [Test]
        public void LeftOf_NoMatch()
        {
            var s = "Gummiboll";
            var r = s.LeftOf("@");
            Assert.AreEqual("Gummiboll", r);
        }


        [Test]
        public void IsNumeric_123()
        {
            var s = "123";
            var r = s.IsNumeric();
            Assert.IsTrue(r);
        }

        [Test]
        public void IsNumeric_StringWithLetters()
        {
            var s = "12Mössa3";
            var r = s.IsNumeric();
            Assert.IsFalse(r);
        }

        [Test]
        public void IsNumeric_Numeric_With_Comma()
        {
            var s = "123,4";
            var r = s.IsNumeric();
            Assert.IsFalse(r);
        }

        [Test]
        public void IsNumeric_VeryLongNumericString()
        {
            var s = "12323427389237497654678684623784623786427836423423423423423423";
            var r = s.IsNumeric();
            Assert.IsTrue(r);
        }
    }
}