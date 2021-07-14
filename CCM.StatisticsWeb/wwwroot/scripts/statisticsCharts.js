
function CreatePieChart(message) {

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


function CreateBarChart(message) {
    var svg = d3.select("#bar-chart");
    svg.selectAll("*").remove();
    var dataset = JSON.parse(message);

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
