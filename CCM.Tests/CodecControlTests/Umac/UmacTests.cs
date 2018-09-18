using CCM.CodecControl.Mandozzi.Umac;
using CCM.Core.CodecControl.Entities;
using NUnit.Framework;

namespace CCM.Tests.CodecControlTests.Umac
{
    [TestFixture]
    public class UmacTests
    {
        private string ip = "192.0.2.107";
        private UmacApi _sut; // System Under Test

        [SetUp]
        public void SetUp()
        {
            _sut = new UmacApi();
        }
        
        [Test]
        public void UMAC_GetLineStatus()
        {
            var lineStatus = _sut.GetLineStatusAsync(ip, 0);
            Assert.IsNotNull(lineStatus);

        }

        [Test]
        public void UMAC_Call_Telefon()
        {
            var address = "sto-s17-01@acip.example.com";
            var profile = "Telefon";

            var result = _sut.CallAsync(ip, address, profile).Result;
            Assert.IsTrue(result);
        }

        [Test]
        public void UMAC_HangUp() 
        {
            var result = _sut.HangUpAsync(ip).Result;
            Assert.IsTrue(result);
        }
    }
}