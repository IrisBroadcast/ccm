using CCM.Core.CodecControl.Enums;
using CCM.Web.Extensions;
using NUnit.Framework;

namespace CCM.UnitTests.CCM.Web
{
    [TestFixture]
    public class EnumDtoTests
    {
        [Test]
        public void should_create_enum_dto()
        {
            var dto = EnumDto.Create(LineStatusCode.ConnectedCalled);
            Assert.AreEqual(5, dto.Value);
            Assert.AreEqual("ConnectedCalled", dto.Name);
            Assert.AreEqual("Samtal uppringt", dto.Description);
        }
    }
}