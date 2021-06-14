using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Services;
using CCM.StatisticsWeb.Statistics;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Pages
{
    public partial class LocationStatisticsOverview
    {
        private static readonly CultureInfo SvCulture = CultureInfo.CreateSpecificCulture("sv-SE");

        private LocationStatisticsMode Mode { get; set; }

        private IEnumerable<Region> Regions { get; set; }
        private IEnumerable<CodecType> CodecTypes { get; set; }
        private IEnumerable<Owner> Owners { get; set; }

        public IEnumerable<LocationBasedStatistics> Statistics { get; set; }
        public LocationStatisticsModel locationStatisticsModel { get; set; } = new LocationStatisticsModel();

        private LocationStatisticsOverview locationStatisticsOverview { get; set; }
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        private bool visible = false;

        protected async override Task OnInitializedAsync()
        {
            Regions = (await StatisticsDataService.GetRegions()).ToList();
            CodecTypes = (await StatisticsDataService.GetCodecTypes()).ToList();
            Owners = (await StatisticsDataService.GetOwners()).ToList();

        }
        //public async Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId, Guid codecTypeId, Guid ownerId, DateTime startTime, DateTime endTime)
        //{
        //    locationStatisticsOverview = (await StatisticsDataService.GetLocationNumberOfCallsTable(regionId, ownerId, codecTypeId, startTime, endTime));
        //    visible = true;
        //    return locationStatisticsOverview;
        //}
        public async Task<LocationStatisticsOverview> GetLocationNumberOfCallsTable(Guid regionId,/* Guid codecTypeId,*/ Guid ownerId, DateTime startTime, DateTime endTime)
        {
            locationStatisticsOverview = (await StatisticsDataService.GetLocationNumberOfCallsTable(regionId, ownerId, /*codecTypeId,*/ startTime, endTime));
            visible = true;
            return locationStatisticsOverview;
        }


        public string ColumnLabel { get; set; }

        //public string ColumnLabel
        //{
        //    get
        //    {
        //        //if (Mode == LocationStatisticsMode.MaxSimultaneousCalls) return Resources.Stats_Number_Of_Simultaneous_Calls;
        //        if (Mode == LocationStatisticsMode.MaxSimultaneousCalls) return "Number of simultaneous calls";

        //        //if (Mode == LocationStatisticsMode.TotaltTimeForCalls) return Resources.Stats_Total_Call_Time_In_Minutes;
        //        if (Mode == LocationStatisticsMode.TotaltTimeForCalls) return "Total call time in minutes";

        //        return "Number of calls";
        //    }
        //}

        public IEnumerable<LocationStatisticsRow> GetRows()
        {
            if (Statistics == null || Statistics.Count() == 0)
                yield break;
            var maxValue = Math.Max(GetMaxValue(), 1.0);
            var multiplier = 1.0 / maxValue;
            foreach (var stats in Statistics)
            {
                yield return new LocationStatisticsRow
                {
                    Label = string.IsNullOrWhiteSpace(stats.LocationName) ? "-" : stats.LocationName,
                    Width = GetWidth(Mode, stats, multiplier),
                    Value = GetValue(Mode, stats),
                    //ToolTip = GetToolTip(Mode, stats)
                };
            }
        }

        private double GetMaxValue()
        {
            if (Statistics == null || Statistics.Count() == 0) return 0;
            if (Mode == LocationStatisticsMode.MaxSimultaneousCalls)
                return Statistics.Max(s => s.MaxSimultaneousCalls);
            if (Mode == LocationStatisticsMode.TotaltTimeForCalls)
                return Statistics.Max(s => s.TotaltTimeForCalls);
            return Statistics.Max(s => s.NumberOfCalls);
        }

        private static string GetWidth(LocationStatisticsMode mode, LocationBasedStatistics stats, double multiplier)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.#}",
                Math.Max(0.1, GetRawValue(mode, stats) * multiplier * 50));
        }

        private static string GetValue(LocationStatisticsMode mode, LocationBasedStatistics stats)
        {
            return string.Format(SvCulture, "{0}", Math.Round(GetRawValue(mode, stats), MidpointRounding.ToEven));
        }

        private static double GetRawValue(LocationStatisticsMode mode, LocationBasedStatistics stats)
        {
            if (mode == LocationStatisticsMode.MaxSimultaneousCalls) return stats.MaxSimultaneousCalls;
            if (mode == LocationStatisticsMode.TotaltTimeForCalls) return stats.TotaltTimeForCalls;
            return stats.NumberOfCalls;
        }

        //private static string GetToolTip(LocationStatisticsMode mode, LocationBasedStatistics stats)
        //{
        //    if (mode != LocationStatisticsMode.MaxSimultaneousCalls) return string.Empty;
        //    if (stats.MaxSimultaneousCalls <= 1) return string.Empty;

        //    var dates = stats.MaxSimultaneousEventDates.ToList();
        //    var sb = new StringBuilder();
        //    var format = dates.Count > 1
        //        ? Resources.Stats_Simultaneous_Calls_At_X_Occasions_Tool_Tip
        //        : Resources.Stats_Simultaneous_Calls_At_One_Occasion_Tool_Tip;
        //    sb.AppendFormat(format, stats.MaxSimultaneousCalls, dates.Count);
        //    dates.ForEach(d => sb.AppendLine().AppendFormat("{0:yyyy-MM-dd}", d));
        //    return sb.ToString();
        //}
    }

    public class LocationStatisticsRow
    {
        public string Label { get; set; }
        public string Width { get; set; }
        public string Value { get; set; }
        public string ToolTip { get; set; }
    }

    public enum LocationStatisticsMode
    {
        NumberOfCalls,
        TotaltTimeForCalls,
        MaxSimultaneousCalls
    }
}

