
function getAllUrlParams(url) {    
    //diagram.html?per=15000&pri=19000&mon=403&fee=3600&t=1

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
    //diagram.html?per=15000&pri=19000&mon=403&fee=3600&t=1  
    var percTotal = getAllUrlParams().per;
    if (percTotal == undefined)
    {
        return;
    }
    var amount = getAllUrlParams().pri;
    var fee = getAllUrlParams().fee;
    var title = '';
    var month = getAllUrlParams().mon;
    if (amount == undefined || fee == undefined || month == undefined || getAllUrlParams().t == undefined) {
        return;
    }
    if (getAllUrlParams().t == '1') {
        title = 'Monthly payments: $' + month;
    }
    else {
        title = 'Payment in first month: $' + month;
    }
    //'100.00';
    // diagram(percTotal, $scope.amount, $scope.commis * $scope.years * 12, monthfordiagram);
    //  diagram(percTotal, $scope.amount, $scope.commis * $scope.years * 12, $scope.paymentMonth + ' $' + monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    //''  '';
    //   percTotal += percent + $scope.commis;
    document.getElementById("diagram").innerHTML = "";
    amount = Math.round(amount);
    var perc = Math.round(percTotal - fee);
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

    chart.legend().position('right');
    chart.legend().itemsLayout('vertical');
    chart.legend().align('center');
    //set chart radius
    chart.radius('60%');

    // create empty area in pie chart
    chart.innerRadius('40%');

    chart.labels().position('outside');

    // initiate chart drawing
    chart.draw();
}