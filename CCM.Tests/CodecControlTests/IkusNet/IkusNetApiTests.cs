using System.Threading.Tasks;
using CCM.CodecControl.Prodys.IkusNet;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Enums;
using NUnit.Framework;

namespace CCM.Tests.CodecControlTests.IkusNet
{
    [TestFixture]
    public class IkusNetApiTests
    {
        private string _hostAddress;

        [SetUp]
        public void SetUp()
        {
            _hostAddress = "192.0.2.237";
        }

        [Test]
        public void GetDeviceName()
        {
            var sut = new IkusNetApi();
            var deviceName = sut.GetDeviceNameAsync(_hostAddress);
            Assert.AreEqual("MTU 25", deviceName);
        }

        [Test]
        public async Task GetInputLevel()
        {
            var sut = new IkusNetApi();

            await sut.SetInputGainLevelAsync(_hostAddress, 0, 6);

            var level = sut.GetInputGainLevelAsync(_hostAddress, 0);
            Assert.AreEqual(6, level);

            await sut.SetInputGainLevelAsync(_hostAddress, 0, 4);

            level = sut.GetInputGainLevelAsync(_hostAddress, 0);
            Assert.AreEqual(4, level);

        }

        [Test]
        public void GetLineStatus()
        {
            var sut = new IkusNetApi();
            
            LineStatus lineStatus = sut.GetLineStatusAsync(_hostAddress, 0).Result;
            Assert.AreEqual("", lineStatus.RemoteAddress);
            Assert.AreEqual(LineStatusCode.NoPhysicalLine, lineStatus.StatusCode);
            Assert.AreEqual(DisconnectReason.None, lineStatus.DisconnectReason);
        }

        [Test, Ignore("To avoid unintentional calling")]
        public void Call()
        {
            var sut = new IkusNetApi();

            var address = "sto-s17-01@acip.example.com";
            var profile = "Studio";

            bool result = sut.CallAsync(_hostAddress, address, profile).Result;
            Assert.IsTrue(result);
        }

        [Test, Ignore("To avoid unintentional hangup")]
        public void Hangup()
        {
            var sut = new IkusNetApi();
            bool result = sut.HangUpAsync(_hostAddress).Result;
            Assert.IsTrue(result);
        }
    }
}