using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Services;
using CCM.StatisticsWeb.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Pages
{
    public partial class CategoryStatisticsOverview
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected IEnumerable<DateBasedCategoryStatistics> categoryStatisticsOverview { get; set; }
        protected IEnumerable<RegionCategoryStatistics> regionCategoryStatistics { get; set; }
        private CategoryStatisticsModel categoryStatisticsModel { get; set; } = new CategoryStatisticsModel();
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        private DateTime startTime { get; set; } = DateTime.UtcNow.AddYears(-1).AddMonths(-1).AddDays(-20);
        private DateTime endTime { get; set; } = DateTime.UtcNow.AddMonths(-11).AddDays(-7);
        private IEnumerable<Region> Regions { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Regions = (await StatisticsDataService.GetRegions());
            categoryStatisticsOverview = (await StatisticsDataService.GetCategories(startTime, endTime)).ToList();
        }

        protected async Task<IEnumerable<DateBasedCategoryStatistics>> GetCategoryStatistics(DateTime startTime, DateTime endTime)
        {
            categoryStatisticsOverview = new List<DateBasedCategoryStatistics>();
            categoryStatisticsOverview = (await StatisticsDataService.GetCategories(startTime, endTime));
            return categoryStatisticsOverview;
        }

    }
}
