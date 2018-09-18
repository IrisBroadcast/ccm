using System.Collections.Generic;
using System.Diagnostics;
using CCM.Core.Entities;
using CCM.Core.Managers;
using NUnit.Framework;

namespace CCM.Tests
{
    [TestFixture]
    public class AvailableFilterTests
    {
        [Test]
        public void should_get_all_available_filters()
        {
            List<AvailableFilter> availableFilters = new FilterManager(null).GetFilterProperties();

            foreach (var availableFilter in availableFilters)
            {
                Debug.WriteLine(string.Format("FilteringName: {0}", availableFilter.FilteringName));
                Debug.WriteLine("---");
            }

        }

    }
}