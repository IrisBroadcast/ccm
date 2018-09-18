using CCM.Core.Entities;
using CCM.Core.Helpers;
using NUnit.Framework;

namespace CCM.Tests
{
    [TestFixture]
    public class MetadataHelperTests
    {
        [Test]
        public void should_get_property_value()
        {
            var obj = new Location() { ShortName = "kort namn" };
            var s = MetadataHelper.GetPropertyValue(obj, "ShortName");
            Assert.AreEqual("kort namn", s);
        }

        [Test]
        public void should_handle_properties_with_null_value()
        {
            var obj = new Location() { ShortName = null };
            var s = MetadataHelper.GetPropertyValue(obj, "ShortName");
            Assert.AreEqual(string.Empty, s);
        }

        [Test]
        public void should_handle_properties_with_null_value_in_object_hierarcy()
        {
            var obj = new RegisteredSip() {Location = null };
            var s = MetadataHelper.GetPropertyValue(obj, "Location.ShortName");
            Assert.AreEqual(string.Empty, s);
        }

        [Test]
        public void should_handle_object_hierarchy()
        {
            var obj = new RegisteredSip() { Location = new Location() { ShortName = "kort namn" } };
            var s = MetadataHelper.GetPropertyValue(obj, "Location.ShortName");
            Assert.AreEqual("kort namn", s);
        }

        [Test]
        public void should_handle_invalid_path()
        {
            var obj = new RegisteredSip() { Location = new Location() { ShortName = "kort namn" } };
            var s = MetadataHelper.GetPropertyValue(obj, "Location.SkjortName");
            Assert.AreEqual("", s);
        }

        [Test]
        public void should_handle_null_object()
        {
            var s = MetadataHelper.GetPropertyValue(null, "Location.SkjortName");
            Assert.AreEqual("", s);
        }

        [Test]
        public void should_handle_null_path()
        {
            var obj = new RegisteredSip() { Location = new Location() { ShortName = "kort namn" } };
            var s = MetadataHelper.GetPropertyValue(obj, null);
            Assert.AreEqual("", s);
        }

        [Test]
        public void should_handle_empty_path()
        {
            var obj = new RegisteredSip() { Location = new Location() { ShortName = "kort namn" } };
            var s = MetadataHelper.GetPropertyValue(obj, string.Empty);
            Assert.AreEqual("", s);
        }
    }
}
