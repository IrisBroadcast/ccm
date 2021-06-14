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
    public partial class SipAccountStatisticsOverview
    {
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }

        private IEnumerable<SipAccount> SipAccounts { get; set; }
        private bool visible { get; set; } = false;
        private SipAccountStatisticsModel sipAccountStatisticsModel { get; set; } = new SipAccountStatisticsModel();
        private IEnumerable<DateBasedStatistics> sipAccountStatisticsOverview { get; set; }
        protected async override Task OnInitializedAsync()
        {
            SipAccounts = (await StatisticsDataService.GetSipAccounts()).ToList();
        }
        public async Task<IEnumerable<DateBasedStatistics>> GetSipStatistics(Guid sipId, DateTime startTime, DateTime endTime)
        {
            sipAccountStatisticsOverview = (await StatisticsDataService.GetSipStatistics(sipId, startTime, endTime));
            visible = true;
            return sipAccountStatisticsOverview;
        }
    }
}
