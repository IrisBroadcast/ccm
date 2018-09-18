using CCM.CodecControl.SR.BaresipRest;
using NUnit.Framework;

namespace CCM.Tests.CodecControlTests.Baresip
{
    [TestFixture]
    public class BaresipApiTests
    {
        private string _ip;

        [SetUp]
        public void SetUp()
        {
            _ip = "134.25.127.231";
        }

        [Test]
        public void IsAvailable()
        {
            var sut = new BaresipRestApi();
            var success = sut.CheckIfAvailableAsync(_ip).Result;
            Assert.AreEqual(true, success);
        }

        [Test]
        public void SetInputEnabled()
        {
            var sut = new BaresipRestApi();
            var success = sut.SetInputEnabledAsync(_ip, 0, true).Result;
            Assert.AreEqual(true, success);
        }

        //[Test]
        //public void GetInputLevel()
        //{
        //    var sut = new IkusNetApi();

        //    sut.SetInputGainLevelAsync(_hostAddress, 0, 6);

        //    var level = sut.GetInputGainLevelAsync(_hostAddress, 0);
        //    Assert.AreEqual(6, level);

        //    sut.SetInputGainLevelAsync(_hostAddress, 0, 4);

        //    level = sut.GetInputGainLevelAsync(_hostAddress, 0);
        //    Assert.AreEqual(4, level);

        //}

        //[Test]
        //public void GetLineStatus()
        //{
        //    var sut = new IkusNetApi();

        //    LineStatus lineStatus = sut.GetLineStatusAsync(_hostAddress, 0).Result;
        //    Assert.AreEqual("", lineStatus.RemoteAddress);
        //    Assert.AreEqual(LineStatusCode.NoPhysicalLine, lineStatus.StatusCode);
        //    Assert.AreEqual(DisconnectReason.None, lineStatus.DisconnectReason);
        //    Assert.AreEqual(IpCallType.Invalid, lineStatus.IpCallType);
        //}

        //[Test, Ignore("To avoid unintentional calling")]
        //public void Call()
        //{
        //    var sut = new IkusNetApi();

        //    var call = new Call()
        //    {
        //        Address = "sto-s17-01@acip.example.com",
        //        CallType = IpCallType.UnicastBidirectional,
        //        Codec = Codec.Program,
        //        Content = CallContent.Audio,
        //        Profile = "Studio"
        //    };

        //    bool result = sut.CallAsync(_hostAddress, call).Result;
        //    Assert.IsTrue(result);
        //}

        //[Test, Ignore("To avoid unintentional hangup")]
        //public void Hangup()
        //{
        //    var sut = new IkusNetApi();
        //    bool result = sut.HangUpAsync(_hostAddress, Codec.Program).Result;
        //    Assert.IsTrue(result);
        //}
    }
}