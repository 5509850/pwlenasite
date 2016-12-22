
function getAllUrlParams(url) {        
    //diagram.html?per=15000&pri=19000&fee=3600
    //chart.html?a=15000&y=5&i=24&f=10&t=a 

    // get query string from url (optional) or window
    var queryString = url ? url.split('?')[1] : window.location.search.slice(1);

    // we'll store the parameters here
    var obj = {};

    // if query string exists
    if (queryString) {

        // stuff after # is not part of query string, so get rid of it
        queryString = queryString.split('#')[0];

        // split our query string into its component parts
        var arr = queryString.split('&');

        for (var i = 0; i < arr.length; i++) {
            // separate the keys and the values
            var a = arr[i].split('=');

            // in case params look like: list[]=thing1&list[]=thing2
            var paramNum = undefined;
            var paramName = a[0].replace(/\[\d*\]/, function (v) {
                paramNum = v.slice(1, -1);
                return '';
            });

            // set parameter value (use 'true' if empty)
            var paramValue = typeof (a[1]) === 'undefined' ? true : a[1];

            // (optional) keep case consistent
            paramName = paramName.toLowerCase();
            paramValue = paramValue.toLowerCase();

            // if parameter name already exists
            if (obj[paramName]) {
                // convert value to array (if still string)
                if (typeof obj[paramName] === 'string') {
                    obj[paramName] = [obj[paramName]];
                }
                // if no array index number specified...
                if (typeof paramNum === 'undefined') {
                    // put the value on the end of the array
                    obj[paramName].push(paramValue);
                }
                    // if array index number specified...
                else {
                    // put the value at that index number
                    obj[paramName][paramNum] = paramValue;
                }
            }
                // if param name doesn't exist yet, set it
            else {
                obj[paramName] = paramValue;
            }
        }
    }

    return obj;
}

function mydiagram() {  
    //diagram.html?per=15000&pri=19000&fee=3600
    if (getAllUrlParams().per == undefined) {
        return;
    }
    var perc = parseInt(getAllUrlParams().per);
    if (getAllUrlParams().pri == undefined || getAllUrlParams().fee == undefined) {
        return;
    }
    var amount = parseInt(getAllUrlParams().pri);
    var fee = parseInt(getAllUrlParams().fee);
    var total = amount + perc + fee;
    var title = '';        
    
    title = 'Total payments: $' + total.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
    
    document.getElementById("diagram").innerHTML = "";
    amount = Math.round(amount);
    
    fee = Math.round(fee)
    if (fee == 0) {
        chart = anychart.pie3d([
        ['Total Principal Paid', amount],
        ['Total Interest Paid', perc]
        ]);
    }
    else {
        chart = anychart.pie3d([
        ['Total Principal Paid', amount],
        ['Total Interest Paid', perc],
        ['Total Maintenance Fee', fee]
        ]);
    }


    // set container id for the chart
    chart.container('diagram');

    // set chart title text settings
    chart.title(title);

    chart.legend().position('bottom');
    chart.legend().itemsLayout('horizontal');
    chart.legend().align('center');
    //set chart radius
    chart.radius('60%');

    // create empty area in pie chart
    chart.innerRadius('40%');

   // chart.labels().position('outside');

    // initiate chart drawing
    chart.draw();
}

function mychart()
{  //chart.html?a=15000&y=5&i=24&f=10&t=a  
    var gr = document.getElementById("graph");    
    gr.setAttribute("style", "display:block;height:400px");    
    if (getAllUrlParams().i == undefined) {
        return;
    }    
    if (getAllUrlParams().a == undefined || getAllUrlParams().y == undefined || getAllUrlParams().f == undefined || getAllUrlParams().t == undefined) {
        return;
    }
    var amount = parseInt(getAllUrlParams().a);
    var years = parseInt(getAllUrlParams().y);
    var month = years * 12;
    var interest = parseInt(getAllUrlParams().i) / 100;
    var percent = parseFloat(amount * interest / 12);
    var fee = parseInt(getAllUrlParams().f);
    var t = getAllUrlParams().t;

    var title = 'title';
    var setGraph = [['1996', 300, 162, 242]];
    setGraph.length = 0;
    
    if (t == 'a') { //Аннуитетные--------  
        var monthly = (amount * (interest / 12)) / (1 - (1 / Math.pow(1 + (interest / 12), (years * 12)))) + fee; // fixed        
        var mainloan = monthly - percent - fee;
        var amountRest = amount;
        title = 'Even Total Payment Schedule';
        for (i = 1; i < month + 1; i++) {                      
            setGraph.push([
                i + '',
                monthly,
                mainloan,
                percent + fee
            ]);

            amountRest -= mainloan;
            percent = parseFloat(amountRest * interest / 12);
            mainloan = monthly - percent - fee;
        }
    }
    else { //diff

        var amountRest = amount; //остаток основного долга            
        var mainloan = parseFloat(amount / (years * 12)); //fixed
        var percent = parseFloat(amountRest * interest / 12);
        var monthly = mainloan + percent + fee;
        title = 'Even Principal Payment Schedule';

        for (i = 1; i < month + 1; i++) {
            
            setGraph.push([
               i + '',
               monthly,
               mainloan,
               percent + fee]);                     
          
            amountRest -= mainloan;                                   
            lastm = monthly;
            percent = parseFloat((amount - (i * (amount / (years * 12)))) * (interest / 12));
            monthly = mainloan + percent + fee;
        }        
    }
    var dataSet = anychart.data.set(setGraph);

    //--------------------------------------------
    // map data for the first series, take x from the zero column and value from the first column of data set
    var seriesData_1 = dataSet.mapAs({ x: [0], value: [1] });

    // map data for the second series, take x from the zero column and value from the second column of data set
    var seriesData_2 = dataSet.mapAs({ x: [0], value: [2] });

    // map data for the third series, take x from the zero column and value from the third column of data set
    var seriesData_3 = dataSet.mapAs({ x: [0], value: [3] });

    // create line chart
    chart = anychart.line();

    // turn on chart animation
    chart.animation(true);

    // turn on the crosshair
    chart.crosshair().enabled(true).yLabel().enabled(false);
    chart.crosshair().yStroke(null);

    // set tooltip mode to point
    chart.tooltip().positionMode('point');

    // set chart title text settings
    chart.title(title);
    chart.title().padding([0, 0, 5, 0]);

    // set yAxis title
    chart.yAxis().title('Payments in Dollars');
    chart.xAxis().labels().padding([5]);

    // create first series with mapped data
    var series_1 = chart.line(seriesData_1);
    series_1.name('Payment =');
    series_1.hoverMarkers().enabled(true).type('circle').size(4);
    series_1.tooltip().position('right').anchor('left').offsetX(5).offsetY(5);

    // create second series with mapped data
    var series_2 = chart.line(seriesData_2);
    series_2.name('Principal +');
    series_2.hoverMarkers().enabled(true).type('circle').size(4);
    series_2.tooltip().position('right').anchor('left').offsetX(5).offsetY(5);

    // create third series with mapped data
    var series_3 = chart.line(seriesData_3);
    series_3.name('Interest');
    series_3.hoverMarkers().enabled(true).type('circle').size(4);
    series_3.tooltip().position('right').anchor('left').offsetX(5).offsetY(5);

    // turn the legend on
    chart.legend().enabled(true).fontSize(13).padding([0, 0, 10, 0]);

    // set container id for the chart and set up paddings
    chart.container('graph');
    chart.padding([10, 20, 5, 20]);

    // initiate chart drawing
    chart.draw();
}