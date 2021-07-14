import Tool from "../utils/Tools";

export default class StatisticsView {
    constructor() {

        // $('#startdatetimepicker').datetimepicker({ format: 'YYYY-MM-DD', date: startDate.toISOString() });
        // $('#enddatetimepicker').datetimepicker({ format: 'YYYY-MM-DD', date: endDate.toISOString() });

        // $("#startdatetimepicker").on("dp.change", function (e) {
        //     $('#enddatetimepicker').data("DateTimePicker").minDate(e.date);
        // });
        // $("#enddatetimepicker").on("dp.change", function (e) {
        //     $('#startdatetimepicker').data("DateTimePicker").maxDate(e.date);
        // });

        this.setDatePickers();

        Tool.$event("locationSearchBtn", "click", this.locationSearch.bind(this));
        Tool.$event("regionSearchBtn", "click", this.regionSearch.bind(this));
        Tool.$event("sipAccountsSearchBtn", "click", this.sipAccountsSearch.bind(this));
        Tool.$event("codecTypesSearchBtn", "click", this.codecTypesSearch.bind(this));
    }

    setDatePickers() {
        let endDate = new Date();
        endDate.setHours(0);
        endDate.setMinutes(0);
        endDate.setSeconds(0);
        endDate.setMilliseconds(0);

        let startDate = new Date(endDate.toISOString());
        startDate.setDate(startDate.getDate() - 30);

        Tool.$dom("startDate").value = startDate.toISOString().split('T')[0];
        Tool.$dom("endDate").value = endDate.toISOString().split('T')[0];

        Tool.$event("startDate", "change", (event) => {
            console.log({event})
        });
    }

    locationSearch() {
        Tool.$dom("locationSearchBtn").setAttribute("disabled", "true");

        const queryParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            ownerId: Tool.$dom("Owners").value,
            codecTypeId: Tool.$dom("CodecTypes").value
        };

        const sim24HourParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            locationId: "00000000-0000-0000-0000-000000000000"
        };

        Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationNumberOfCallsView", queryParams).then((content) => {
            Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        });

        Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationTotalTimeForCallsView", queryParams).then((content) => {
            Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        });

        Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationMaxSimultaneousCallsView", queryParams).then((content) => {
            Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        });

        Tool.$dom("locationSim24HourChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$dom("locationSim24HourChartDataDiv").innerHTML = "";
        Tool.$fetchView("/Statistics/LocationSim24HourView", sim24HourParams).then((content) => {
            Tool.$dom("locationSim24HourChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");

            Tool.$event("locationSim24HourSelect", "change", this.sim24HourSearch.bind(this));
        }).catch((error) => {
            Tool.$dom("locationSim24HourChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        });
    }

    sim24HourSearch() {
        Tool.$dom("locationSim24HourSelect").setAttribute("disabled", "true");

        const sim24HourParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            locationId: Tool.$dom("locationSim24HourSelect").value
        };

        Tool.$dom("locationSim24HourChartDataDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationSim24HourViewData", sim24HourParams).then((content) => {
            Tool.$dom("locationSim24HourChartDataDiv").innerHTML = content;
            Tool.$dom("locationSim24HourSelect").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("locationSim24HourChartDataDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSim24HourSelect").removeAttribute("disabled");
        });
    }

    regionSearch() {
        Tool.$dom("regionSearchBtn").setAttribute("disabled", "true");

        const queryParamsCalls = {
            filterType: "Regions",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("regionRegions").value
        };

        const queryParamsTime = {
            filterType: "Regions",
            chartType: "TotalTimeForCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("regionRegions").value
        };

        Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/RegionNumberOfCallsView", queryParamsCalls).then((content) => {
            console.log(content, "##################")
            Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        });

        Tool.$dom("regionTimeChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/GetDateBasedChart", queryParamsTime).then((content) => {
            Tool.$dom("regionTimeChartDiv").innerHTML = content;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("regionTimeChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        });
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