using CCM.CodecControl;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Interfaces;
using NUnit.Framework;

namespace CCM.Tests.CodecControlTests.IkusNet
{
    [TestFixture]
    public class IkusnetTests
    {
        private static readonly CodecInformation CodecInformation = new CodecInformation
        {
            SipAddress = "sto-s17-01@acip.example.com",
            Api = "IkusNet",
            Ip = "192.0.2.30" // sto-s17-01
        };

        private ICodecManager GetCodecManager()
        {
            var settingsManager = new DummySettingsManager();
            return new CodecManager(settingsManager);
        }

        [Test]
        public void Ikusnet_GetLineStatus()
        {
            var manager = GetCodecManager();
            var lineStatus = manager.GetLineStatusAsync(CodecInformation, 0);
            Assert.IsNotNull(lineStatus);
        }

    }

}