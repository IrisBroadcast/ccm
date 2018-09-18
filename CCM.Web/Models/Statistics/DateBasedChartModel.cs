using System;

namespace CCM.Web.Models.Statistics
{
    public class DateBasedChartModel
    {
        public DateBasedFilterType FilterType { get; set; }
        public DateBasedChartType ChartType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid FilterId { get; set; }
    }

    public enum DateBasedFilterType
    {
        Regions,
        SipAccounts,
        CodecTypes
    }

    public enum DateBasedChartType
    {
        NumberOfCalls,
        TotalTimeForCalls
    }
}