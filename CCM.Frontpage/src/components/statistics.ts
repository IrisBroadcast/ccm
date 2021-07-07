import Tool from '../utils/Tools';

export default class StatisticsView {
    constructor() {
        let endDate = new Date();
        endDate.setHours(0);
        endDate.setMinutes(0);
        endDate.setSeconds(0);
        endDate.setMilliseconds(0);

        let startDate = new Date(endDate.toISOString());
        startDate.setDate(startDate.getDate() - 30);

        Tool.$dom("startDate").value = startDate.toISOString().split('T')[0];
        Tool.$dom("endDate").value = endDate.toISOString().split('T')[0];

        // $('#startdatetimepicker').datetimepicker({ format: 'YYYY-MM-DD', date: startDate.toISOString() });
        // $('#enddatetimepicker').datetimepicker({ format: 'YYYY-MM-DD', date: endDate.toISOString() });

        // $("#startdatetimepicker").on("dp.change", function (e) {
        //     $('#enddatetimepicker').data("DateTimePicker").minDate(e.date);
        // });
        // $("#enddatetimepicker").on("dp.change", function (e) {
        //     $('#startdatetimepicker').data("DateTimePicker").maxDate(e.date);
        // });

        Tool.$event("locationSearchBtn", "click", this.locationSearch.bind(this));
        Tool.$event("regionSearchBtn", "click", this.regionSearch.bind(this));
        Tool.$event("sipAccountsSearchBtn", "click", this.sipAccountsSearch.bind(this));
        Tool.$event("codecTypesSearchBtn", "click", this.codecTypesSearch.bind(this));
    }

    locationSearch() {
        Tool.$dom("locationSearchBtn").setAttribute("disabled", "true");

        var queryParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            ownerId: Tool.$dom("Owners").value,
            codecTypeId: Tool.$dom("CodecTypes").value
        };

        var sim24HourParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            locationId: '00000000-0000-0000-0000-000000000000'
        };

        Tool.$dom("errorInfo").innerHTML = "";
        Tool.$fetchView("/Statistics/GetLocationNumberOfCallsTable", queryParams).then((content) => {
            Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + error;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        });
        // $('#locationNumberOfCallsChartDiv').load('/Statistics/GetLocationNumberOfCallsTable', queryParams, function (response, status, xhr) {
        //     if (status === 'error') {
        //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        //     } else {
        //         // $('#locationTotalTimeForCallsChartDiv').load('/Statistics/GetLocationTotaltTimeForCallsTable', queryParams, function (response, status, xhr) {
        //         //     if (status === 'error') {
        //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //         Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        //         //     } else {
        //         //         // $('#locationMaxSimultaneousCallsChartDiv').load('/Statistics/GetLocationMaxSimultaneousCallsTable', queryParams, function (response, status, xhr) {
        //         //         //     if (status === 'error') {
        //         //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //         //         Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        //         //         //     } else {
        //         //         //         // $('#locationSim24HourChartDiv').load('/Statistics/GetLocationSim24HourChart', sim24HourParams, function (response, status, xhr) {
        //         //         //         //     if (status === 'error') {
        //         //         //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //         //         //         Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        //         //         //         //     } else {
        //         //         //         //         Tool.$event("locationSim24HourSelect", "change", this.sim24HourSearch);
        //         //         //         //         Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        //         //         //         //     }
        //         //         //         // });
        //         //         //     }
        //         //         // });
        //         //     }
        //         // });
        //     }
        // });
    }

    sim24HourSearch() {
        Tool.$dom("locationSim24HourSelect").setAttribute("disabled", "true");

        var sim24HourParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            locationId: Tool.$dom("locationSim24HourSelect").value
        };
        // $('#locationSim24HourChartDiv').load('/Statistics/GetLocationSim24HourChart', sim24HourParams, function (response, status, xhr) {
        //     if (status === 'error') {
        //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //     } else {
        //         Tool.$event("locationSim24HourSelect", "change", this.sim24HourSearch);
        //     }
        // });
    }

    regionSearch() {
        Tool.$dom("regionSearchBtn").setAttribute("disabled", "true");

        var queryParamsCalls = {
            filterType: "Regions",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("regionRegions").value
        };

        var queryParamsTime = {
            filterType: "Regions",
            chartType: "TotalTimeForCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("regionRegions").value
        };

        Tool.$dom("errorInfo").innerHTML = "";
        // $('#regionNumberOfCallsChartDiv').load('/Statistics/GetDateBasedChart', queryParamsCalls, function (response, status, xhr) {
        //     if (status === 'error') {
        //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        //     } else {
        //         // $('#regionTimeChartDiv').load('/Statistics/GetDateBasedChart', queryParamsTime, function (response, status, xhr) {
        //         //     if (status === 'error') {
        //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //     }
        //         //     Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        //         // });
        //     }
        // });
    }

    sipAccountsSearch() {
        Tool.$dom("sipAccountsSearchBtn").setAttribute("disabled", "true");

        var queryParamsCalls = {
            filterType: "SipAccounts",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("sipAccountsAccounts").value
        };

        var queryParamsTime = {
            filterType: "SipAccounts",
            chartType: "TotalTimeForCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("sipAccountsAccounts").value
        };

        Tool.$dom("errorInfo").innerHTML = "";
        // $('#sipAccountsNumberOfCallsChartDiv').load('/Statistics/GetDateBasedChart', queryParamsCalls, function (response, status, xhr) {
        //     if (status === 'error') {
        //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         Tool.$dom("sipAccountsSearchBtn").removeAttribute("disabled");
        //     } else {
        //         // $('#sipAccountsTimeChartDiv').load('/Statistics/GetDateBasedChart', queryParamsTime, function (response, status, xhr) {
        //         //     if (status === 'error') {
        //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //     }
        //         //     Tool.$dom("sipAccountsSearchBtn").removeAttribute("disabled");
        //         // });
        //     }
        // });
    }

    codecTypesSearch() {
        Tool.$dom("codecTypesSearchBtn").setAttribute("disabled", "true");

        var queryParamsCalls = {
            filterType: "CodecTypes",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("codecTypesCodecTypes").value
        };

        var queryParamsTime = {
            filterType: "CodecTypes",
            chartType: "TotalTimeForCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("codecTypesCodecTypes").value
        };

        Tool.$dom("errorInfo").innerHTML = "";
        // $('#codecTypeNumberOfCallsChartDiv').load('/Statistics/GetDateBasedChart', queryParamsCalls, function (response, status, xhr) {
        //     if (status === "error") {
        //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         Tool.$dom("codecTypesSearchBtn").removeAttribute("disabled");
        //     } else {
        //         // $('#codecTypeTimeChartDiv').load('/Statistics/GetDateBasedChart', queryParamsTime, function (response, status, xhr) {
        //         //     if (status === 'error') {
        //         //         Tool.$dom("errorInfo").innerHTML = "Fel vid laddning av statistik: " + xhr.status + " " + xhr.statusText;
        //         //     }
        //         //     Tool.$dom("codecTypesSearchBtn").removeAttribute("disabled");
        //         // });
        //     }
        // });
    }

}