import Tool from "./utils/Tools";
import * as d3 from "d3";

export class StatisticsView {
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
        Tool.$event("categorySearchBtn", "click", this.categorySearch.bind(this));
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

        Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/RegionNumberOfCallsView", queryParamsCalls).then((content) => {
            Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("regionNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("regionSearchBtn").removeAttribute("disabled");
        });
    }

    sipAccountsSearch() {
        Tool.$dom("sipAccountsSearchBtn").setAttribute("disabled", "true");

        const queryParamsCalls = {
            filterType: "SipAccounts",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("sipAccountsAccounts").value
        };

        Tool.$dom("sipAccountsNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/SipAccountNumberOfCallsView", queryParamsCalls).then((content) => {
            Tool.$dom("sipAccountsNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("sipAccountsSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("sipAccountsNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("sipAccountsSearchBtn").removeAttribute("disabled");
        });
    }

    codecTypesSearch() {
        Tool.$dom("codecTypesSearchBtn").setAttribute("disabled", "true");

        const queryParamsCalls = {
            filterType: "CodecTypes",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: Tool.$dom("codecTypesCodecTypes").value
        };

        Tool.$dom("codecTypeNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/CodecTypeNumberOfCallsView", queryParamsCalls).then((content) => {
            Tool.$dom("codecTypeNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("codecTypesSearchBtn").removeAttribute("disabled");
        }).catch((error) => {
            Tool.$dom("codecTypeNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("codecTypesSearchBtn").removeAttribute("disabled");
        });
    }

    async categorySearch() {
        Tool.$dom("categoryNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/CategoryNumberOfCallsView", null).then(async (content) => {
            Tool.$dom("categoryNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("categorySearchBtn").removeAttribute("disabled");

            try {
                const parameters = new URLSearchParams({
                    startTime: Tool.$dom("startDate").value,
                    endTime: Tool.$dom("endDate").value
                });
                const data = await fetch("/Statistics/GetCategoryStatistics?" + parameters.toString(), {
                    method: "POST",
                    body: JSON.stringify(parameters)
                });
                const messenger = await data.json();
                console.log(messenger);
                this.CreateBarChart(messenger);
            } catch(err) {
                console.error(err);
            }
        }).catch((error) => {
            Tool.$dom("categoryNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("categorySearchBtn").removeAttribute("disabled");
        });
    }


    CreatePieChart(message) {

        var svg = d3.select("svg");
        svg.selectAll("*").remove();
        document.getElementById('errormessage').innerHTML = "";

        var data = JSON.parse(message);

        if (data.length === 0) {
            return document.getElementById('errormessage').innerHTML = '<p>' + "No data" + '</p>';
        }
        var width = 450, height = 450, radius = Math.min(width, height) / 2;

        svg = d3.select("svg")
            .attr("width", width)
            .attr("height", height);

        var g = svg.append("g")
            .attr("transform", "translate(" + width / 2 + "," + height / 1.8 + ")");

        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var pie = d3.pie().value(function (d) {
            const objValue = Object.values(d);
            return objValue[1];
        });

        var path = d3.arc()
            .outerRadius(radius - 50)
            .innerRadius(0);

        var label = d3.arc()
            .outerRadius(radius - 60)
            .innerRadius(radius);


        var arc = g.selectAll(".arc")
            .data(pie(data))
            .enter().append("g")
            .attr("class", "arc");

        arc.append("path")
            .attr("d", path)
            .attr("fill", function (d) {
                const first = Object.values(d.data);
                const amount = Object.values(d.data);
                return color(Object.values(first[0]) + " " + amount[1])
            });

        arc.append("text")
            .attr("transform", function (d) {
                return "translate(" + label.centroid(d) + ")";
            })
            .text(function (d) {
                const second = Object.values(d.data)
                const amount = Object.values(d.data);
                return (second[0] + " " + amount[1]);
            })

        let res = (Object.keys(data[0])[1])
        svg.append("g")
            .attr("transform", "translate(" + (width / 2 - 120) + "," + 20 + ")")
            .append("text")
            .text(res)
            .attr("class", "title")
            .style("fill", "green")
            .style("font-size", "23");

    }


    CreateBarChart(message) {
        var svg = d3.select("#bar-chart");
        svg.selectAll("*").remove();
        var dataset = message, //JSON.parse(message),

        svg = d3.select("#bar-chart"),
            margin = {
                top: 20,
                right: 20,
                bottom: 30,
                left: 50
            },
            width = +svg.attr("width") - margin.left - margin.right,
            height = +svg.attr("height") - margin.top - margin.bottom,
            g = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");


        var x = d3.scaleBand()
            .rangeRound([0, width])
            .padding(0.1);

        var y = d3.scaleLinear()
            .rangeRound([height, 0]);

        x.domain(dataset.map(function (d) {
            return d.Date;
        }))

        y.domain([0, d3.max(dataset, function (d) {
            return Number(d.NumberOfCalls);
        })]);

        g.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", "-.55em")
            .attr("transform", "rotate(-90)");


        g.append("g")
            .call(d3.axisLeft(y))
            .append("text")
            .attr("fill", "#000")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", "0.71em")
            .attr("text-anchor", "end")
            .text("NumberOfCalls");

        g.selectAll(".bar")
            .data(dataset)
            .enter().append("rect")
            .attr("class", "bar")
            .attr("x", function (d) {
                return x(d.Date);
            })
            .attr("y", function (d) {
                return y(Number(d.NumberOfCalls));
            })
            .attr("width", x.bandwidth())
            .attr("height", function (d) {
                return height - y(Number(d.NumberOfCalls));
            });
    }
}

export default class Statistics {
    public static load() {
        console.log("Initiated Statistics");
        const app = new StatisticsView();
    }
}

Statistics.load();
