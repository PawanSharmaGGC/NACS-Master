
        // Function to Update Slider Position for Slider 1
    function SliderPosition1() {
            var current1 = $('#CaField111').val();
    var future1 = $('#CaField2').val();
    var output1 = ((parseFloat(future1 - current1) / current1) * 100).toFixed(1);
    $('#Slider1').val(output1);
    $('#value1').val(output1 + '%');
        }

    // Function to Update Slider Position for Slider 2
    function SliderPosition2() {
            var current2 = $('#CaField33').val();
    var future2 = $('#CaField4').val();
    var output2 = ((parseFloat(future2 - current2) / current2) * 100).toFixed(1);
    $('#Slider2').val(output2);
    $('#value2').val(output2 + '%');
        }

    // Function to Update Slider Position for Slider 4
    function SliderPosition4() {
            var current4 = $('#CaField7').text();
    var future4 = $('#CaField8').val();
    var output4 = ((parseFloat(future4 - current4) / current4) * 100).toFixed(1);
    $('#Slider4').val(output4);
    $('#value4').val(output4 + '%');
        }

    // Function to Update Slider Position for Slider 5
    function SliderPosition5() {
            var current5 = $('#CaField999').val();
    var future5 = $('#CaField10').val();
    var output5 = ((parseFloat(future5 - current5) / current5) * 100).toFixed(1);
    $('#Slider5').val(output5);
    $('#value5').val(output5 + '%');
        }

    // Function to Update Slider Position for Slider 6 
    function SliderPosition6() {
            var current6 = $('#CaField1111').val();
    var future6 = $('#CaField12').val();
    var output6 = ((parseFloat(future6 - current6) / current6) * 100).toFixed(1);
    $('#Slider6').val(output6);
    $('#value6').val(output6 + '%');
        }

    // Function to Calculate Future 1
    function CalculateFuture1() {
            var current1 = $('#CaField111').val();
    var future1 = $('#CaField2');
    var minutes1 = $('#Slider1').val();
    var permin1 = ((parseFloat(current1) * ((parseFloat(minutes1) / 100))) + parseFloat(current1)).toFixed(2);
    $('#CaField2').val(parseFloat(permin1).toFixed(2));
        }

    // Function to Calculate Future 2
    function CalculateFuture2() {
            var current2 = $('#CaField33').val();
    var future2 = $('#CaField4').val();
    var minutes2 = $('#Slider2').val();
    var permin2 = ((parseFloat(current2) * ((parseFloat(minutes2) / 100))) + parseFloat(current2)).toFixed(0);
    $('#CaField4').val(parseFloat(permin2).toFixed(0));
        }

    // Function to Calculate Future 4
    function Calculate4() {
            var current4 = $('#CaField7').text();
    var future4 = $('#CaField8').val();
    var minutes4 = $('#Slider4').val();
    var permin4 = ((parseFloat(current4) * ((parseFloat(minutes4) / 100))) + parseFloat(current4)).toFixed(0);
    $('#CaField8').val(parseFloat(permin4).toFixed(0));
        }

    // Function to Calculate 5
    function Calculate5() {
            var current5 = $('#CaField999').val();
    var future5 = $('#CaField10').val();
    var minutes5 = $('#Slider5').val();
    var permin5 = ((parseFloat(current5) * ((parseFloat(minutes5) / 100))) + parseFloat(current5)).toFixed(0);
    $('#CaField10').val(parseFloat(permin5).toFixed(0));
        }

    // Function to Calculate 6
    function Calculate6() {
            var current6 = $('#CaField1111').val();
    var future6 = $('#CaField12').val();
    var minutes6 = $('#Slider6').val();
    var permin6 = ((parseFloat(current6) * ((parseFloat(minutes6) / 100))) + parseFloat(current6)).toFixed(0);
    $('#CaField12').val(parseFloat(permin6).toFixed(0));
        }



    // Sum Employee
    function SumEmployee() {
            var TotalEmp = 0;
    var currentEmp = $('#CaField17').val();
    var addEmp = $('#CaField15').val();
    var sum = $('#CaField16').val();
    if (addEmp === "") addEmp = 0;
    TotalEmp = (parseFloat(currentEmp) + parseFloat(addEmp));
    $('#CaField16').val(parseFloat(TotalEmp));
        }

    // Total P&L Impact 
    function TotalPLImpact() {
            var ReField1 = $('#ReField11').val();
    var ReField2 = $('#ReField22').val();
    var ReField3 = $('#ReField33').val();
    var ReField4 = $('#ReField4');
    var CaField16 = $('#CaField16').val();
    var CaField14 = $('#CaField14').val();
    var Field9C = $('#Text15').val();
    var Field1R = $('#Text1').val();
    var Field4R = $('#Text4').val();

    // If the value is empty, set it to 0
    ReField1 = (ReField1 === "") ? 0 : parseFloat(ReField1);
    ReField2 = (ReField2 === "") ? 0 : parseFloat(ReField2);
    ReField3 = (ReField3 === "") ? 0 : parseFloat(ReField3);
    CaField16 = (CaField16 === "") ? 0 : parseFloat(CaField16);
    CaField14 = (CaField14 === "") ? 0 : parseFloat(CaField14);

    // Calculate total impact
    var TotalImpact = (ReField1 + ReField2 + ReField3).toFixed(0);

    // Calculate net income
    var NetIncome = ((TotalImpact * 100) / (parseFloat(Field1R) * (parseFloat(Field4R) / 100))).toFixed(0);

    // Update the results in the fields
    $('#ReField6').val(parseFloat(NetIncome).toFixed(0));
    $('#ReField66').val(parseFloat(NetIncome).toFixed(0));
    $('#ReField4').val(parseFloat(TotalImpact).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField44').val(parseFloat(TotalImpact).toFixed(0));
        }

    // Function to Calculate Total Estimate
    function CalculateTotalEstimate() {
            var CaField1 = $('#CaField111').val();
    var CaField2 = $('#CaField2').val();
    var S1 = $('#Slider1').val();
    var S2 = $('#Slider2').val();
    var Field3R = $('#Text3').val();
    var Field0R = $('#Text0').val();
    var Field1R = $('#Text1').val();
    var ReField0 = $('#ReField0');
    var ReField1 = $('#ReField1');
    var Field0C = $('#Text7').val();
    var Field6C = $('#Text12').val();

    var FP1 = (parseFloat(CaField2) + (parseFloat(CaField2) * (S1 / 100))).toFixed(0);
    var TotalERU = (parseFloat(CaField1) * parseFloat(S1 / 100) * parseFloat(Field3R) * parseFloat(Field0R) * 365).toFixed(0);
    var TotalERU1 = (parseFloat(Field3R) * parseFloat(S2 / 100) * parseFloat(FP1) * parseFloat(Field0R) * 365).toFixed(0);
    var OtherInformationutputs = (parseFloat(Field1R) - (parseFloat(Field0C) * parseFloat(Field0R))) / parseFloat(Field1R);
    var Total = (parseFloat(TotalERU1) + parseFloat(TotalERU)).toFixed(0);
    var TotalGross = ((parseFloat(TotalERU1) + parseFloat(TotalERU)) * (parseFloat(OtherInformationutputs))).toFixed(0);

    $('#ReField0').val(parseFloat(Total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField00').val(parseFloat(Total).toFixed(0));
    $('#ReField1').val(parseFloat(TotalGross).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField11').val(parseFloat(TotalGross).toFixed(0));
        }

    // Function to Calculate Fuel Profitability
    function CalculateFuelProfitability() {
            var Field5R = $('#Text5').val();
    var Field6R = $('#Text6').val();
    var Field3R = $('#Text3').val();
    var Field0R = $('#Text0').val();
    var CaField6 = $('#CaField6').val();
    var S2 = $('#Slider2').val();

    var FS = (parseFloat(Field5R) * parseFloat(Field6R) * parseFloat(Field3R) * ((parseFloat(CaField6)) / 100) * ((parseFloat(S2) / 100))).toFixed(0);
    var TotalFR = (parseFloat(FS) * parseFloat(Field0R) * parseFloat(365)).toFixed(0);
    $('#ReField3').val(parseFloat(TotalFR).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField33').val(parseFloat(TotalFR).toFixed(0));
        }

    // Function to Calculate Cost Mitigation
    function CostMitigation() {
            var Field1R = $('#Text1').val();
    var Field4C = $('#Text10').val();
    var Field5C = $('#Text11').val();
    var Field6C = $('#Text12').val();
    var Field7C = $('#Text13').val();
    var Field8C = $('#Text14').val();
    var Field11C = $('#Text18').val();

    var CaField9 = $('#CaField999').val();
    var CaField11 = $('#CaField1111').val();
    var CaField16 = $('#CaField16').val();

    var S4 = $('#Slider4').val();
    var S5 = $('#Slider5').val();
    var S6 = $('#Slider6').val();

    var TrunOver = ((-1) * ((parseFloat(Field6C) / 100) * (parseFloat(S4) / 100)) * parseFloat(CaField16)
    * ((parseFloat(Field4C) + parseFloat(Field5C)) / 2)).toFixed(0);
    var reducedshrink = ((-1) * (parseFloat(CaField9) * (parseFloat(S5) / 100))).toFixed(0);
    var OTHrs = ((-1) * parseFloat(CaField11) * (parseFloat(S6) / 100) * ((parseFloat(Field8C) - parseFloat(Field11C)))).toFixed(0);

    var TotalCost = parseFloat(TrunOver) + parseFloat(reducedshrink) + parseFloat(OTHrs);
    $('#ReField2').val(parseFloat(TotalCost).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField22').val(parseFloat(TotalCost).toFixed(0));
        }

    function WageImpact() {
        var ReField1 = $('#ReField11').val();
    var ReField2 = $('#ReField22').val();
    var ReField3 = $('#ReField33').val();
    var CaField14 = $('#CaField14').val();
    var CaField16 = $('#CaField16').val();

    // If the value is empty, set it to 0
    ReField1 = (ReField1 === "") ? 0 : parseFloat(ReField1);
    ReField2 = (ReField2 === "") ? 0 : parseFloat(ReField2);
    ReField3 = (ReField3 === "") ? 0 : parseFloat(ReField3);
    CaField14 = (CaField14 === "") ? 0 : parseFloat(CaField14);
    CaField16 = (CaField16 === "") ? 0 : parseFloat(CaField16);

    // Calculate total impact
    var TotalImpact = (ReField1 + ReField2 + ReField3).toFixed(0);

    // Calculate wages impact
    var WagesImpact = ((TotalImpact * (CaField14 / 100)) / (30 * 52 * CaField16));

    // Update the results in the fields
    $('#ReField5').val(parseFloat(WagesImpact).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $('#ReField55').val(parseFloat(WagesImpact).toFixed(2));
    }


    $(document).ready(function () {

        // Slider 1
        var slider1 = $('#Slider1');
    var output1 = $('#value1');
    output1.html(slider1.val() + '%');
    slider1.on('input', function () {
        output1.html(this.value + '%');
        });

    // Slider 2
    var slider2 = $('#Slider2');
    var output2 = $('#value2');
    output2.html(slider2.val() + '%');
    slider2.on('input', function () {
        output2.html(this.value + '%');
        });

    // Slider 4
    var slider4 = $('#Slider4');
    var output4 = $('#value4');
    output4.html(slider4.val() + '%');
    slider4.on('input', function () {
        output4.html(this.value + '%');
        });

    // Slider 5
    var slider5 = $('#Slider5');
    var output5 = $('#value5');
    output5.html(slider5.val() + '%');
    slider5.on('input', function () {
        output5.html(this.value + '%');
        });

    // Slider 6
    var slider6 = $('#Slider6');
    var output6 = $('#value6');
    output6.html(slider6.val() + '%');
    slider6.on('input', function () {
        output6.html(this.value + '%');
        });

        
    });
