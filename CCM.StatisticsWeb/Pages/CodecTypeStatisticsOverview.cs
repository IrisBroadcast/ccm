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
    public partial class CodecTypeStatisticsOverview
    {
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        private IEnumerable<CodecType> CodecTypes { get; set; }
        private IEnumerable<DateBasedStatistics> codecTypeStatisticsOverview { get; set; }
        private bool visible { get; set; } = false;
        private CodecTypeStatisticsModel codecTypeStatisticsModel { get; set; } = new CodecTypeStatisticsModel();

        protected async override Task OnInitializedAsync()
        {
            CodecTypes = (await StatisticsDataService.GetCodecTypes()).ToList();
        }
        public async Task<IEnumerable<DateBasedStatistics>> GetCodecTypeStatistics(Guid codecTypeId, DateTime startTime, DateTime endTime)
        {
            codecTypeStatisticsOverview = (await StatisticsDataService.GetCodecTypeStatistics(codecTypeId, startTime, endTime));
            visible = true;
            return codecTypeStatisticsOverview;
        }

    }
}
