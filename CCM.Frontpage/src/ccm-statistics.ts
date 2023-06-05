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

        Tool.$event("locationSearchBtn", "click", this.NAV_locationSearch.bind(this));
        Tool.$event("regionSearchBtn", "click", this.NAV_regionSearch.bind(this));
        Tool.$event("sipAccountsSearchBtn", "click", this.NAV_sipAccountsSearch.bind(this));
        Tool.$event("codecTypesSearchBtn", "click", this.NAV_codecTypesSearch.bind(this));
        Tool.$event("categorySearchBtn", "click", this.NAV_categorySearch.bind(this));

        const tabs = Array.from(document.querySelectorAll('[data-target-tab]'));
        if (!tabs || tabs.length === 0) {
            console.warn("No tabs found on this page");
            return;
        }
    }

    setDatePickers() {
        let endDate = new Date();
        endDate.setHours(0);
        endDate.setMinutes(0);
        endDate.setSeconds(0);
        endDate.setMilliseconds(0);

        let startDate = new Date(endDate.toISOString());
        startDate.setDate(startDate.getDate() - 30);

        let adjustedEndDate = endDate.toString().split(/\+|-/)[0];

        Tool.$dom("startDate").value = startDate.toISOString().split('T')[0];
        Tool.$dom("endDate").value = new Date(adjustedEndDate).toISOString().split('T')[0];

        Tool.$event("startDate", "change", (event) => {
            console.log({event})
        });
    }

    NAV_locationSearch() {
        Tool.$dom("locationSearchBtn").setAttribute("disabled", "true");

        this._fetch__location();
    }

    private get _qp__location() {
        return {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            ownerId: Tool.$dom("Owners").value,
            codecTypeId: Tool.$dom("CodecTypes").value
        };
    };

    private _fetch__location() {
        Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationNumberOfCallsView", this._qp__location).then((content) => {
            Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .catch((error) => {
            Tool.$dom("locationNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            this._fetch__location1();
        });
    }

    private _fetch__location1() {
        Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationTotalTimeForCallsView", this._qp__location).then((content) => {
            Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .catch((error) => {
            Tool.$dom("locationTotalTimeForCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            this._fetch__location2();
        });
    }

    private _fetch__location2() {
        Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/LocationMaxSimultaneousCallsView", this._qp__location).then((content) => {
            Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .catch((error) => {
            Tool.$dom("locationMaxSimultaneousCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            this._fetch__location3();
        });
    }

    private _fetch__location3() {
        const sim24HourParams = {
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            regionId: Tool.$dom("Regions").value,
            locationId: "00000000-0000-0000-0000-000000000000"
        };

        Tool.$dom("locationSim24HourChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$dom("locationSim24HourChartDataDiv").innerHTML = "";
        Tool.$fetchView("/Statistics/LocationSim24HourView", sim24HourParams).then((content) => {
            Tool.$dom("locationSim24HourChartDiv").innerHTML = content;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");

            Tool.$event("locationSim24HourSelect", "change", this.sim24HourSearch.bind(this));
        }).catch((error) => {
            Tool.$dom("locationSim24HourChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("locationSearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            console.log(`Finally ran everything`);
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

    NAV_regionSearch() {
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

    NAV_sipAccountsSearch() {
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

    NAV_codecTypesSearch() {
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

    private get _qp__category() {
        return {
            filterType: "Categories",
            chartType: "NumberOfCalls",
            startDate: Tool.$dom("startDate").value,
            endDate: Tool.$dom("endDate").value,
            filterId: ""
        };
    };

    async NAV_categorySearch() {
        this._fetch__category();
    }

    private _fetch__category() {
        // Number of call combinations per category itneraction
        Tool.$dom("categoryNumberOfCallsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/CategoryCallNumberOfCallsView", this._qp__category).then(async (content) => {
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
                //this.CreatePieChart(messenger);
                //this.CreateTreeMap(messenger);
            } catch(err) {
                console.error(err);
            }
        })
        .catch((error) => {
            Tool.$dom("categoryNumberOfCallsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("categorySearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            this._fetch__category1();
        });
    }

    private _fetch__category1() {
        // Separated category items
        Tool.$dom("categoryNumberOfItemsChartDiv").innerHTML = `<div class="loading"></div>`;
        Tool.$fetchView("/Statistics/CategoryNumberOfCallsView", this._qp__category).then(async (content) => {
            Tool.$dom("categoryNumberOfItemsChartDiv").innerHTML = content;
            Tool.$dom("categorySearchBtn").removeAttribute("disabled");

            try {
                const parameters = new URLSearchParams({
                    startTime: Tool.$dom("startDate").value,
                    endTime: Tool.$dom("endDate").value
                });
                // const data = await fetch("/Statistics/GetCategoryStatistics?" + parameters.toString(), {
                //     method: "POST",
                //     body: JSON.stringify(parameters)
                // });
                // const messenger = await data.json();
                // console.log(messenger);
                // // this.CreateBarChart(messenger);
                // this.CreatePieChart(messenger);
            } catch(err) {
                console.error(err);
            }
        })
        .catch((error) => {
            Tool.$dom("categoryNumberOfItemsChartDiv").innerHTML = `<div class="error">${error}</div>`;
            Tool.$dom("categorySearchBtn").removeAttribute("disabled");
        })
        .finally(() => {
            console.log(`Finally last category`);
        });
    }

    CreatePieChart(data) {
        var svg = d3.select("#bar-chart");
            svg.selectAll("*").remove();
        document.getElementById('errormessage').innerHTML = "";

        if (data.length === 0) {
            return document.getElementById('errormessage').innerHTML = '<p>' + "No data" + '</p>';
        }
        var width = 450, height = 450, radius = Math.min(width, height) / 2;

        svg = d3.select("#bar-chart")
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
            .enter()
            .append("g")
            .attr("class", "arc");

        arc.append("path")
            .attr("d", path)
            .attr("fill", function (d) {
                console.log(d)
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

    CreateTreeMap(data) {

        // set the dimensions and margins of the graph
        var margin = {top: 10, right: 10, bottom: 10, left: 10},
        width = 445 - margin.left - margin.right,
        height = 445 - margin.top - margin.bottom;

        // append the svg object to the body of the page
        var svg = d3.select("#bar-chart")
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform",
                "translate(" + margin.left + "," + margin.top + ")");

        // Give the data to this cluster layout:
        var root = d3.hierarchy(data).sum(function(d){ return d.numberOfCalls });
        // Here the size of each leave is given in the 'value' field in input data

        // Then d3.treemap computes the position of each element of the hierarchy
        d3.treemap()
            .size([width, height])
            .padding(2)
            (root)

        // use this information to add rectangles:
        svg
            .selectAll("rect")
            .data(root.leaves())
            .enter()
            .append("rect")
            .attr('x', function (d) { return d.x0; })
            .attr('y', function (d) { return d.y0; })
            .attr('width', function (d) { return d.x1 - d.x0; })
            .attr('height', function (d) { return d.y1 - d.y0; })
            .style("stroke", "black")
            .style("fill", "slateblue")

        // and to add the text labels
        svg
            .selectAll("text")
            .data(root.leaves())
            .enter()
            .append("text")
            .attr("x", function(d){ return d.x0+5})    // +10 to adjust position (more right)
            .attr("y", function(d){ return d.y0+20})    // +20 to adjust position (lower)
            .text(function(d){ return d.data.name })
            .attr("font-size", "15px")
            .attr("fill", "white")

    }

    CreateBarChart(dataset) {
        var svg = d3.select("#bar-chart");

        const margin = {
                top: 20,
                right: 50,
                bottom: 80,
                left: 50
            };

        let width = +svg.attr("width") - margin.left - margin.right;
        let height = +svg.attr("height") - margin.top - margin.bottom;

        svg.attr("preserveAspectRatio", "xMinYMin meet")
            .attr("viewBox", "0 0 600 400")
            .classed("svg-content-responsive", true)

        var g = svg
            .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        var x = d3.scaleBand()
            .rangeRound([0, width])
            .padding(0.1);

        var y = d3.scaleLinear()
            .rangeRound([height, 0]);

        x.domain(dataset.map(function (d) {
            console.log("x", d);
            return d.category;
        }))

        y.domain([0, d3.max(dataset, function (d) {
            console.log("Num", d);
            return Number(d.numberOfCalls);
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
            .enter()
            .append("rect")
            .attr("class", "bar")
            .attr("x", function (d) {
                return x(d.category);
            })
            .attr("y", function (d) {
                return y(Number(d.numberOfCalls));
            })
            .attr("width", x.bandwidth())
            .attr("height", function (d) {
                console.log("ddddddd", height, "#", d, "¤", y(Number(d.numberOfCalls)) )
                return height - y(Number(d.numberOfCalls));
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
