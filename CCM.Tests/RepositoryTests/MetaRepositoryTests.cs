using CCM.Core.Cache;
using CCM.Data.Repositories;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.RepositoryTests
{
    [TestFixture, Ignore("")]
    public class MetaRepositoryTests
    {
        [Test]
        public void GetAll()
        {
            Assert.DoesNotThrow(() =>
            {
                var metaTypes = new MetaRepository(new CachingService()).GetAll();
                Assert.IsNotNull(metaTypes);
            });
        }

        
    }
}