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
    public partial class StatisticsOverview
    {
        private IEnumerable<Region> Regions { get; set; }
        private IEnumerable<CodecType> CodecTypes { get; set; }
        private IEnumerable<Owner> Owners { get; set; }
        private DateTime StartTime { get; set; } = DateTime.UtcNow.AddMonths(-1);
        private DateTime EndTime { get; set; } = DateTime.UtcNow;
        private Guid RegionId { get; set; }
        private Guid CodecTypeId { get; set; }
        private Guid OwnerId { get; set; }
        private bool visible = false;

        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }

        private LocationStatisticsOverview locationStatisticsOverview { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Regions = (await StatisticsDataService.GetRegions()).ToList();
            CodecTypes = (await StatisticsDataService.GetCodecTypes()).ToList();
            Owners = (await StatisticsDataService.GetOwners()).ToList();

        }
        public async Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid ownerId, DateTime startTime, DateTime endTime)

        {
            locationStatisticsOverview = (await StatisticsDataService.GetLocationNumberOfCallsTable(regionId, ownerId, startTime, endTime));
            visible = true;
            return locationStatisticsOverview;
        }
    }
}
