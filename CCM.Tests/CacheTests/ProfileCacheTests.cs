using System.Collections.Generic;
using CCM.Core.Cache;
using CCM.Core.Entities.Specific;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.CacheTests
{
    [TestFixture]
    public class ProfileCacheTests
    {
        [Test]
        public void GetFromCache()
        {
            IAppCache cache = new CachingService();

            var profiles = cache.GetProfiles(
                () => new List<ProfileNameAndSdp>()
                {
                    new ProfileNameAndSdp() {Name = "Profile 1", Sdp = "Sdp A"},
                    new ProfileNameAndSdp() {Name = "Profile 2", Sdp = "Sdp B"},
                    new ProfileNameAndSdp() {Name = "Profile 3", Sdp = "Sdp C"}
                });

            Assert.IsNotNull(profiles);
            Assert.AreEqual(3, profiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);

            var sameProfiles = cache.GetProfiles(null);
            Assert.IsNotNull(sameProfiles);
            Assert.AreEqual(3, sameProfiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);
        }

        [Test]
        public void DeleteFromCache()
        {
            IAppCache cache = new CachingService();

            var profiles = cache.GetProfiles(
                () => new List<ProfileNameAndSdp>()
                {
                    new ProfileNameAndSdp() {Name = "Profile 1", Sdp = "Sdp A"},
                    new ProfileNameAndSdp() {Name = "Profile 2", Sdp = "Sdp B"},
                    new ProfileNameAndSdp() {Name = "Profile 3", Sdp = "Sdp C"}
                });

            Assert.IsNotNull(profiles);
            Assert.AreEqual(3, profiles.Count);
            Assert.AreEqual("Profile 3", profiles[2].Name);

            cache.ResetProfiles();

            var sameProfiles = cache.GetProfiles(null);
            Assert.IsNull(sameProfiles);
        }

    }
}