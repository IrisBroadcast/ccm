using System;
using CCM.Core.Helpers;
using CCM.Data.Repositories;
using NUnit.Framework;

namespace CCM.UnitTests
{
    [TestFixture()]
    public class JustSomeTests
    {

        [Test]
        public void test_assignment_in_two_levels()
        {
            // Just to verify that language behaves as I expect.
            int i1 = 24;
            Assert.AreEqual(24, i1);

            int i2 = i1 = 42;
            Assert.AreEqual(42, i2);
            Assert.AreEqual(42, i1);
        }

        [Test]
        public void nullable_tostring()
        {
            int? i = 5;
            var s = i.ToString();
            Assert.AreEqual(5, i);
            Assert.AreEqual("5", s);

            i = null;
            var s2 = i.ToString();
            Assert.IsNull(i);
            Assert.AreEqual(string.Empty, s2);
        }

        [Test]
        public void Test_AsHexString()
        {
            var bytes = CryptoHelper.GenerateSaltBytes();
            var s = CcmUserRepository.AsHexString(bytes);
            Console.WriteLine(s);
        }
    }
}