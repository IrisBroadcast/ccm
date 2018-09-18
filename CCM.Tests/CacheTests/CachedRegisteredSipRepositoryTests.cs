using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using CCM.Core.Cache;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Core.Kamailio;
using CCM.Core.Managers;
using CCM.Data.Repositories;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.CacheTests
{
    [TestFixture]
    public class CachedRegisteredSipRepositoryTests
    {
        [Test, Explicit]
        public void ReloadOnUpdate()
        {
            var repo = GetCachedRegisteredSipRepository();
            var sipsOnline = repo.GetCachedRegisteredSips();
            var sipOnline = sipsOnline.First();
            var updated = sipOnline.Updated;

            var repo2 = GetSimpleRegisteredSipRepository();
            RegisteredSip sip = repo2.GetRegisteredSipById(sipOnline.Id);
            sip.DisplayName += "¤";
            repo.UpdateRegisteredSip(sip);

            sipOnline = repo.GetCachedRegisteredSips().First(rs => rs.Id == sip.Id);
            Assert.AreNotEqual(updated, sipOnline.Updated);
        }

        [Test, Explicit]
        public void getRegisteredSipsOnline_should_be_updated_by_codec_registration()
        {
            var repo = GetCachedRegisteredSipRepository();
            var sipsOnline = repo.GetCachedRegisteredSips();
            var sipOnline = sipsOnline.First();
            var updated = sipOnline.Updated;

            var repo2 = GetSimpleRegisteredSipRepository();
            var sip = repo2.GetRegisteredSipById(sipOnline.Id);

            Thread.Sleep(1200);

            var newSip = new RegisteredSip
            {
                IP = sip.IP,
                Port = sip.Port,
                ServerTimeStamp = sip.ServerTimeStamp,
                SIP = sip.SIP,
                UserAgentHead = sip.UserAgentHead,
                Username = sip.Username,
                DisplayName = sip.DisplayName,
                Expires = sip.Expires
            };

            var changeStatus = repo.UpdateRegisteredSip(newSip);
            Assert.AreEqual(KamailioMessageChangeStatus.NothingChanged, changeStatus);

            sipOnline = repo.GetCachedRegisteredSips().First();
            var updated2 = sipOnline.Updated;

            Assert.AreNotEqual(updated, updated2);
        }

        [Test, Explicit]
        public void GetCachedRegisteredSips()
        {
            var repo = GetCachedRegisteredSipRepository();
            var sipsOnline = repo.GetCachedRegisteredSips();

            Assert.IsNotNull(sipsOnline);
            Assert.IsTrue(sipsOnline.Any());
        }

        [Test, Explicit]
        public void GetRegisteredSipsOnline()
        {
            var repo = GetCachedRegisteredSipRepository();
            var sipsOnline = repo.GetCachedRegisteredSips();

            Assert.IsNotNull(sipsOnline);
            Assert.IsTrue(sipsOnline.Any());
        }

        private IRegisteredSipRepository GetCachedRegisteredSipRepository()
        {
            return new CachedRegisteredSipRepository(
                new CachingService(), 
                new RegisteredSipRepository(
                    new SettingsManager(new SettingsRepository(new CachingService())),
                    new LocationManager(new LocationRepository(new CachingService())),
                    new MetaRepository(new CachingService()),
                    new CachingService())
                );
        }

        private ISimpleRegisteredSipRepository GetSimpleRegisteredSipRepository()
        {
            return new SimpleRegisteredSipRepository(new SettingsManager(new SettingsRepository(new CachingService())), new CachingService());
        }
    }
}