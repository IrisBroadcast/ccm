using System;
using System.Linq;
using CCM.Core.Cache;
using CCM.Core.Managers;
using CCM.Data.Repositories;
using LazyCache;
using NUnit.Framework;

namespace CCM.Tests.RepositoryTests
{
    [TestFixture]
    public class StatisticsManagerTests
    {
        [Test, Explicit]
        public void GetLocationStatistics()
        {
            var manager = new StatisticsManager(new CallHistoryRepository(new CachingService()), 
                new CodecTypeRepository(new CachingService()), 
                new OwnersRepository(new CachingService()), 
                new RegionRepository(new CachingService()), 
                new LocationRepository(new CachingService()), 
                new SimpleRegisteredSipRepository(new SettingsManager(new SettingsRepository(new CachingService())), new CachingService()), 
                new SipAccountRepository(new CachingService()));

            var result = manager.GetLocationStatistics(DateTime.Parse("2016-06-10 00:00:00"),
                DateTime.Parse("2016-06-17 00:00:00"), Guid.Empty, Guid.Empty, Guid.Empty);

            foreach (var item in result.Where(i => i.NumberOfCalls > 0))
            {
                Console.WriteLine("{1} {2} {3:0.00} [{0}]", item.LocationName, item.NumberOfCalls, item.MaxSimultaneousCalls, item.TotaltTimeForCalls);
            }
        }
    }
}