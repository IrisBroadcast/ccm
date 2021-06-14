using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Services;
using CCM.StatisticsWeb.Statistics;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Pages
{
    public partial class RegionStatisticsOverview
    {
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        private IEnumerable<DateBasedStatistics> regionStatisticsOverview { get; set; }

        private IEnumerable<Region> Regions { get; set; }
        private bool visible { get; set; } = false;
        private RegionStatisticsModel regionStatisticsModel { get; set; } = new RegionStatisticsModel();

        protected async override Task OnInitializedAsync()
        {
            Regions = await StatisticsDataService.GetRegions();
        }
        
        protected async Task<IEnumerable<DateBasedStatistics>> GetRegionStatistics(Guid regionId, DateTime startTime, DateTime endTime)
        {
            regionStatisticsOverview = (await StatisticsDataService.GetRegionStatistics(regionId, startTime, endTime)); 
            visible = true;
            return regionStatisticsOverview;
        }
    }
}
