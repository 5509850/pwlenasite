//Модель
var model = {
    payments: [{ month: '0 month', amount: '$0', perc: '$0', main: '$0', pay: '$0', passed: false, style: 'well', font: 'inherit' }]
};

//Модуль

var myApp = angular.module("App", ['AngularPrint'], function ($locationProvider) {
    $locationProvider.html5Mode(true);
});

//var myApp = angular.module("App", ['AngularPrint', '...']);
//Контоллер      
myApp.controller("CourceListCtrl", CourceList);

function CourceList($scope) {
    $scope.data = model;
    $scope.paymentMonth = 'Monthly payments:';


    var am = parseFloat(localStorage.loan_amount);
    var ap = parseFloat(localStorage.loan_apr);
    var ye = parseFloat(localStorage.loan_years);
    var co = parseFloat(localStorage.loan_commis);
    var an = (localStorage.loan_annu);
    if (isNaN(am)) {
        am = 10000;
    }
    if (isNaN(ap)) {
        ap = 10;
    }
    if (isNaN(ye)) {
        ye = 2;
    }
    if (isNaN(co)) {
        co = 0;
    }
    if (!isNaN(an)) {
        if (an == 1)
            $scope.payment = {
                type: 'Annu'
            };
        else {
            $scope.payment = {
                type: 'Diff'
            };
        }
    }
    else {
        $scope.payment = {
            type: 'Annu'
        };
    }
    $scope.amount = am;
    $scope.apr = ap;
    $scope.years = ye;
    $scope.commis = co;

    $scope.amountText = $scope.amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');

    //обработчик нажатия по кнопке "Печать"          
   

    $scope.clear = function () {
        $scope.data.payments.length = 0; // clear array
        $scope.amountText = $scope.amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
        $scope.monthly = '';
        $scope.percent = '';
        $scope.total = '';
        $scope.count = '';
        $scope.coin = false;
        //$scope.evenTotal = false;
        //$scope.evenPrincipal = false;
        document.getElementById("diagram").innerHTML = "";
        document.getElementById("graph").innerHTML = "";                
        document.getElementById("graph").setAttribute("style", "display:block;width:540px;height:0");
        $scope.fee = '';       
    }

    //обработчик нажатия по кнопке "Расчитать"  
    $scope.calculateByMonth = function () {
        var setGraph = [['1996', 300, 162, 242]];
        var an = 0;
        if ($scope.payment.type == 'Annu') {
            an = 1;
        }
        save($scope.amount, $scope.apr, $scope.years, $scope.commis, an);
        $scope.commisShow = $scope.commis > 0;
        $scope.data.payments.length = 0; // clear array
        setGraph.length = 0;// clear array
        $scope.count = $scope.years * 12;
        if ($scope.commis > 0)
        {
            $scope.fee = '+ Maintenance Fee';
        }

        if ($scope.payment.type == 'Annu') { //Аннуитетные-------------------------      
            var interest = parseFloat($scope.apr / 100);
            var percTotal = 0;
            var mainTotal = 0;
            var monthlyTotal = 0;
            $scope.paymentMonth = 'Monthly payments:';

            $scope.coin = true;
            //$scope.evenTotal = true;
            //$scope.evenPrincipal = false;

            var amountRest = $scope.amount; //остаток основного долга
            var percent = parseFloat(amountRest * interest / 12);
            var monthly = ($scope.amount * (interest / 12)) / (1 - (1 / Math.pow(1 + (interest / 12), ($scope.years * 12)))) + $scope.commis; // fixed
            $scope.monthly = monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
            var mainloan = monthly - percent - $scope.commis;

            for (i = 1; i < ($scope.years * 12) + 1; i++) {
                var mystyle = 'success';
                if (i % 2 == 0) {
                    mystyle = 'success';
                }
                else {
                    mystyle = 'warning';
                }
                $scope.data.payments.push({
                    month: i + ' month',
                    amount: amountRest.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    perc: (percent + $scope.commis).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    main: mainloan.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    pay: monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    passed: false,
                    style: mystyle,
                    font: 'inherit'
                });

                setGraph.push([
                    i + '',
                    monthly,
                    percent + $scope.commis,
                    mainloan]);

                amountRest -= mainloan;
                mainTotal += mainloan;
                percTotal += percent + $scope.commis;
                monthlyTotal += monthly;

                percent = parseFloat(amountRest * interest / 12);
                mainloan = monthly - percent - $scope.commis;
            }
            //Итого:
            $scope.data.payments.push({
                month: "Total:",
                amount: "0",
                perc: percTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                main: mainTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                pay: monthlyTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                passed: true,
                style: 'danger',
                font: 'large'
            });
            $scope.total = monthlyTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
            $scope.percent = percTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');                    
          
            diagram(percTotal, $scope.amount, $scope.commis * $scope.years * 12, $scope.paymentMonth + ' $' + monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));         
            var titlegraph = 'Even Total Payment Schedule';   
            graph(setGraph, titlegraph);
        }
        else { //Дифференцированные платежи           --------------------------------------------------------------------------------------        
            var interest = parseFloat($scope.apr / 100);
            var percTotal = 0;
            var mainTotal = 0;
            var monthlyTotal = 0;

            $scope.coin = true;
            //$scope.evenTotal = false;
            //$scope.evenPrincipal = true;

            $scope.paymentMonth = 'Payment in first month:';

            var amountRest = $scope.amount; //остаток основного долга            
            var mainloan = parseFloat($scope.amount / ($scope.years * 12)); //fixed
            var percent = parseFloat(amountRest * interest / 12);
            var monthly = mainloan + percent + $scope.commis;

            $scope.monthly = monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
            var monthfordiagram = $scope.paymentMonth + ' $' + monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');            

            for (i = 1; i < ($scope.years * 12) + 1; i++) {
                var mystyle = 'success';
                if (i % 2 == 0) {
                    mystyle = 'success';
                }
                else {
                    mystyle = 'warning';
                }
                $scope.data.payments.push({
                    month: i + ' month',
                    amount: amountRest.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    perc: (percent + $scope.commis).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    main: mainloan.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    pay: monthly.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                    passed: false,
                    style: mystyle,
                    font: 'inherit'
                });

                setGraph.push([
                   i + '',
                   monthly,
                   percent + $scope.commis,
                   mainloan]);
                amountRest -= mainloan;
                mainTotal += mainloan;
                percTotal += percent + $scope.commis;
                monthlyTotal += monthly;

                percent = parseFloat(($scope.amount - (i * ($scope.amount / ($scope.years * 12)))) * (interest / 12));
                monthly = mainloan + percent + $scope.commis;
            }
            //Итого:
            $scope.data.payments.push({
                month: "Total:",
                amount: "0",
                perc: percTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                main: mainTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                pay: monthlyTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'),
                passed: true,
                style: 'danger',
                font: 'large'
            });
            $scope.total = monthlyTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
            $scope.percent = percTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');            
                    
            diagram(percTotal, $scope.amount, $scope.commis * $scope.years * 12, monthfordiagram);            
            var titlegraph = 'Even Principal Payment Schedule';     
            graph(setGraph, titlegraph);
        }
    }

    $scope.showText = function (passed) {
        return passed ? "Yes" : "No";
    }
    $scope.setStyle = function (passed) {
        return passed ? "color:green" : "color:red; font-weight: bold";
    }
    $scope.newPaymentTypeValue = function (value) {
        $scope.log = value;
    }
}

function save(amount, apr, years, commis, annu) {
    if (window.localStorage) {  // Only do this if the browser supports it
        localStorage.loan_amount = amount;
        localStorage.loan_apr = apr;
        localStorage.loan_years = years;
        localStorage.loan_commis = commis;
        localStorage.loan_annu = annu;
    }
}

function diagram(percTotal, amount, fee, title) {
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
    chart.radius('110%');

    // create empty area in pie chart
    chart.innerRadius('30%');

    chart.labels().position('outside');

    // initiate chart drawing
    chart.draw();
}

function graph(set, title) {
    var gr = document.getElementById("graph");
    gr.innerHTML = "";
    gr.setAttribute("style", "display:block;width:540px;height:374px");    
    var dataSet = anychart.data.set(set);

    // map data for the first series, take x from the zero area and value from the first area of data set
    var seriesData_1 = dataSet.mapAs({ x: [0], value: [1] });

    // map data for the second series, take x from the zero area and value from the second area of data set
    var seriesData_2 = dataSet.mapAs({ x: [0], value: [3] });

    // map data for the third series, take x from the zero area and value from the third area of data set
    var seriesData_3 = dataSet.mapAs({ x: [0], value: [2] });

    // create area chart
    chart = anychart.area3d();

    // set container id for the chart
    chart.container('graph');

    // turn on chart animation
    chart.animation(true);

    // turn off the crosshair
    chart.crosshair(true);

    // set chart title text settings
    chart.title(title);
    chart.title().padding([0, 0, 5, 0]);

    // set interactivity and tooltips settings
    chart.interactivity().hoverMode('byX');
    chart.tooltip().displayMode('union');


    chart.yAxis().title('Payments in Dollars');
    chart.yAxis().labels().textFormatter(function () {
        if (this.value == 0) return this.value;
        return '$' + this.value;
    });

    // create zero line
    var zeroLine = chart.lineMarker(0);
    zeroLine.stroke("#ddd");
    zeroLine.scale(chart.yScale());
    zeroLine.value(0);

    // helper function to setup label settings for all series
    var setupSeries = function (series, name) {
        series.name(name);
        series.markers(false);
        series.hoverMarkers(false);
    };

    // temp variable to store series instance
    var series;

    // create first series with mapped data
    series = chart.area(seriesData_1);
    setupSeries(series, 'Payment =');

    // create second series with mapped data
    series = chart.area(seriesData_2);
    setupSeries(series, 'Principal +');

    // create third series with mapped data
    series = chart.area(seriesData_3);
    setupSeries(series, 'Interest');

    // turn on legend
    chart.legend().enabled(true).fontSize(13).padding([0, 0, 20, 0]);

    chart.grid();
    chart.grid(1).layout('vertical');

    chart.zAspect('90%');
    chart.zPadding(10);
    chart.zAngle(75);

    // initiate chart drawing
    chart.draw();
}
