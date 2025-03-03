//Total base c-store utility costs
function CalcBaseeCStoreUtilityCosts() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var TotalutilityCost = 0;
    var BaseCStoreEnergyCosts = $('#_txtBaseCStoreEnergyCosts').val();
    var BaseCStoreDemandCharges = $('#_txtBaseCStoreDemandCharges').val();
    var UtilityCost = $('#_txtTotalBaseCStoreUtilityCosts').val();

    if (BaseCStoreEnergyCosts == "") BaseCStoreEnergyCosts = 0;
    if (BaseCStoreDemandCharges == "") BaseCStoreDemandCharges = 0;
    if (UtilityCost == "") UtilityCost = 0;

    TotalutilityCost = (parseFloat(BaseCStoreEnergyCosts) + parseFloat(BaseCStoreDemandCharges));

    $('#_hdTotalBaseCStoreUtilityCosts').val(parseFloat(TotalutilityCost).toFixed(2));
    $('#_txtTotalBaseCStoreUtilityCosts').val(parseFloat(TotalutilityCost).toFixed(2));
}

//Demand Charge
function CalcBaseeCStoreDemandCharges() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var TotalutilityCost = 0;
    var BaseCStoreEnergyCosts = $('#_txtBaseCStoreEnergyCosts').val();
    var BaseCStoreDemandCharges = $('#_txtBaseCStoreDemandCharges').val();
    var UtilityCost = $('#_txtTotalBaseCStoreUtilityCosts').val();

    if (BaseCStoreEnergyCosts == "") BaseCStoreEnergyCosts = 0;
    if (BaseCStoreDemandCharges == "") BaseCStoreDemandCharges = 0;
    if (UtilityCost == "") UtilityCost = 0;

    TotalutilityCost = (parseFloat(BaseCStoreDemandCharges) + parseFloat(BaseCStoreEnergyCosts));

    $('#_hdTotalBaseCStoreUtilityCosts').val(parseFloat(TotalutilityCost).toFixed(2));
    $('#_txtTotalBaseCStoreUtilityCosts').val(formatter.format(TotalutilityCost));
}

//Total energy Charge
function CalculateTotalCharges() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var total = 0;
    var EnergyCharges = $('#_txtEnergyCharges').val();
    var DemandCharges = $('#_txtDemandCharges').val();
    var _hdTotalCharges = $('#_hdTotalCharges').val();
    var _txtTotalCharges = $('#_txtTotalCharges').val();

    if (EnergyCharges == "") EnergyCharges = 0;
    if (DemandCharges == "") DemandCharges = 0;
    if (_hdTotalCharges == "") _hdTotalCharges = 0;

    total = (parseFloat(EnergyCharges) + parseFloat(DemandCharges));

    // $('# _hdTotalCharges.ClientID %>').val(parseFloat(total).toFixed(2));
    // $('# _txtTotalCharges.ClientID %>').val(formatter.format(total));
}

//Energy Charges
function CalculateEnergyCharges() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var EnergyCharges = 0;
    var DemandkW = 0;
    var MonthlyCharge = 0;
    var TotalCharge = 0;

    var _txtNumberOfCharger = $('#Text0').val();
    var _txtNumberChargePerChargerPerDay = $('#Text1').val();
    var _txtkWhCharge = $('#Text15').val();
    var _txtkWCharger = $('#Text3').val();
    var _txtBaseCStoreEnergyCosts = $('#Text4').val();
    var _txtEnergyCostkwh = $('#Text7').val();
    var _txtMonthlyChargesPerMonth = $('#_txtMonthlyChargesPerMonth').val();
    var _txtChargers = $('#_txtChargers').val();
    var _txtChargesChargerDay = $('#_txtChargesChargerDay').val();
    var _txtEnergyCharges = $('#_txtEnergyCharges').val();
    var _txtDemandCharges = $('#_txtDemandCharges').val();
    var _txtTotalCharges = $('#_hdTotalCharges').val();
    var _txtTotalDemandkW = $('#Text14').val();
    var _txtDemandCostkW = $('#Text8').val();
    var _txtDemandBasekW = $('#_txtDemandBasekW').val();
    var _txtDaysMonth = $('#Text2').val();

    // Default to 0 if the input fields are empty
    if (_txtNumberOfCharger == "") _txtNumberOfCharger = 0;
    if (_txtNumberChargePerChargerPerDay == "") _txtNumberChargePerChargerPerDay = 0;
    if (_txtkWhCharge == "") _txtkWhCharge = 0;
    if (_txtBaseCStoreEnergyCosts == "") _txtBaseCStoreEnergyCosts = 0;
    if (_txtMonthlyChargesPerMonth == "") _txtMonthlyChargesPerMonth = 0;
    if (_txtEnergyCostkwh == "") _txtEnergyCostkwh = 0;
    if (_txtEnergyCharges == "") _txtEnergyCharges = 0;
    if (_txtDemandCharges == "") _txtDemandCharges = 0;
    if (_txtTotalCharges == "") _txtTotalCharges = 0;
    if (_txtTotalDemandkW == "") _txtTotalDemandkW = 0;
    if (_txtDemandCostkW == "") _txtDemandCostkW = 0;
    if (_txtDemandBasekW == "") _txtDemandBasekW = 0;
    if (_txtChargesChargerDay == "") _txtChargesChargerDay = 0;
    if (_txtkWCharger == "") _txtkWCharger = 0;
    if (_txtChargers == "") _txtChargers = 0;
    if (_txtDaysMonth == "") _txtDaysMonth = 0;

    // Energy Charges
    var ChargesPerMonth = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));
    var IncrementalMonthlykWh = parseFloat(ChargesPerMonth * ((parseFloat(_txtkWhCharge).toFixed(2))));
    var BaseMonthlykWh = ((parseFloat(_txtBaseCStoreEnergyCosts).toFixed(2)) / (parseFloat(_txtEnergyCostkwh).toFixed(2)));
    var TotalMonthlykWhl = parseFloat(BaseMonthlykWh + IncrementalMonthlykWh).toFixed(2);

    MonthlyCharge = parseFloat(ChargesPerMonth).toFixed(0);
    $('#_hdMonthlyChargesPerMonth').val(parseFloat(MonthlyCharge).toFixed(0));
    $('#_txtMonthlyChargesPerMonth').val(parseFloat(MonthlyCharge).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ","));

    EnergyCharges = parseFloat(TotalMonthlykWhl * (parseFloat(_txtEnergyCostkwh).toFixed(2)));
    $('#_hdEnergyCharges').val(parseFloat(EnergyCharges).toFixed(2));
    $('#_txtEnergyCharges').val(formatter.format(EnergyCharges));

    // Demand Charges
    var DemandkW = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtkWCharger).toFixed(2));
    var TotalDemandkW = parseFloat(DemandkW + ((parseFloat(_txtDemandBasekW)))).toFixed(2);
    var DemandkWCharge = parseFloat(TotalDemandkW).toFixed(2) * (parseFloat(_txtDemandCostkW).toFixed(2));
    $('#_hdDemandCharges').val(parseFloat(DemandkWCharge).toFixed(2));
    $('#_txtDemandCharges').val(formatter.format(DemandkWCharge));

    // Total Charges
    TotalCharge = parseFloat(EnergyCharges + DemandkWCharge).toFixed(2);
    $('#_hdTotalCharges').val(parseFloat(TotalCharge).toFixed(2));
    $('#_txtTotalCharges').val(formatter.format(TotalCharge));
}

//Incremental Charges
function CalculateIncremental() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    // Calculate Increment
    var Incremental = 0;
    var TotalCharge = $('#_hdTotalCharges').val();
    var TotalbaseCStoreUtilityCosts = $('#Text6').val();
    var _txtIncrementOverTotalBase = $('#_hdIncrementOverTotalBase').val();

    if (TotalCharge == "") TotalCharge = 0;
    if (TotalbaseCStoreUtilityCosts == "") TotalbaseCStoreUtilityCosts = 0;
    if (_txtIncrementOverTotalBase == "") _txtIncrementOverTotalBase = 0;

    Incremental = (parseFloat(TotalCharge) - parseFloat(TotalbaseCStoreUtilityCosts));

    $('#_hdIncrementOverTotalBase').val(parseFloat(Incremental).toFixed(2));
    $('#_txtIncrementOverTotalBase').val(formatter.format(Incremental));
}

//Cost Per Charge
function CalculateCostPerCharge() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    // Calculate Cost Per Charge
    var CostPerCharge = 0;

    var IncrementOverTotalBase = $('#_hdIncrementOverTotalBase').val();
    var MonthlyChargesPerMonth = $('#_txtMonthlyChargesPerMonth').val();
    var _txtCostPercharges = $('#_hdCostPercharges').val();

    if (IncrementOverTotalBase == "") IncrementOverTotalBase = 0;
    if (MonthlyChargesPerMonth == "") MonthlyChargesPerMonth = 0;
    if (_txtCostPercharges == "") _txtCostPercharges = 0;

    CostPerCharge = (parseFloat(IncrementOverTotalBase).toFixed(2)) / (parseFloat(MonthlyChargesPerMonth).toFixed(2));

    $('#_hdCostPercharges').val(parseFloat(CostPerCharge).toFixed(2));
    $('#_txtCostPercharges').val(formatter.format(CostPerCharge));
}

//Cost Per kWh
function CalculateCostPerkWh() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var EnergyCharges = 0;
    var DemandkW = 0;
    var MonthlyCharge = 0;

    var _txtNumberOfCharger = $('#Text0').val();
    var _txtNumberChargePerChargerPerDay = $('#Text1').val();
    var _txtkWhCharge = $('#Text15').val();
    var _txtkWCharger = $('#Text3').val();
    var _txtBaseCStoreEnergyCosts = $('#Text4').val();
    var _txtEnergyCostkwh = $('#Text7').val();
    var _txtMonthlyChargesPerMonth = $('#_txtMonthlyChargesPerMonth').val();
    var _txtChargers = $('#_txtChargers').val();
    var _txtChargesChargerDay = $('#_txtChargesChargerDay').val();
    var _txtEnergyCharges = $('#_txtEnergyCharges').val();
    var _txtDemandCharges = $('#_txtDemandCharges').val();
    var _txtTotalCharges = $('#_txtTotalCharges').val();
    var _txtTotalDemandkW = $('#Text14').val();
    var _txtDemandCostkW = $('#Text8').val();
    var _txtDemandBasekW = $('#_txtDemandBasekW').val();
    var _txtDaysMonth = $('#Text2').val();

    if (_txtNumberOfCharger == "") _txtNumberOfCharger = 0;
    if (_txtNumberChargePerChargerPerDay == "") _txtNumberChargePerChargerPerDay = 0;
    if (_txtkWhCharge == "") _txtkWhCharge = 0;
    if (_txtBaseCStoreEnergyCosts == "") _txtBaseCStoreEnergyCosts = 0;
    if (_txtMonthlyChargesPerMonth == "") _txtMonthlyChargesPerMonth = 0;
    if (_txtEnergyCostkwh == "") _txtEnergyCostkwh = 0;
    if (_txtEnergyCharges == "") _txtEnergyCharges = 0;
    if (_txtDemandCharges == "") _txtDemandCharges = 0;
    if (_txtTotalCharges == "") _txtTotalCharges = 0;
    if (_txtTotalDemandkW == "") _txtTotalDemandkW = 0;
    if (_txtDemandCostkW == "") _txtDemandCostkW = 0;
    if (_txtDemandBasekW == "") _txtDemandBasekW = 0;
    if (_txtChargesChargerDay == "") _txtChargesChargerDay = 0;
    if (_txtTotalDemandkW == "") _txtTotalDemandkW = 0;
    if (_txtkWCharger == "") _txtkWCharger = 0;
    if (_txtChargers == "") _txtChargers = 0;
    if (_txtDaysMonth == "") _txtDaysMonth = 0;

    // Energy Charges
    var ChargesPerMonth = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));
    var IncrementalMonthlykWh = parseFloat(ChargesPerMonth * ((parseFloat(_txtkWhCharge).toFixed(2))));
    var BaseMonthlykWh = ((parseFloat(_txtBaseCStoreEnergyCosts).toFixed(2)) / (parseFloat(_txtEnergyCostkwh).toFixed(2)));
    var TotalMonthlykWhl = parseFloat(BaseMonthlykWh + IncrementalMonthlykWh).toFixed(2);

    MonthlyCharge = parseFloat(ChargesPerMonth).toFixed(0);

    EnergyCharges = parseFloat(TotalMonthlykWhl * (parseFloat(_txtEnergyCostkwh).toFixed(2)));

    // Demand Charges
    var DemandkW = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtkWCharger).toFixed(2));
    var TotalDemandkW = parseFloat(DemandkW + ((parseFloat(_txtDemandBasekW)))).toFixed(2);
    var DemandkWCharge = parseFloat(TotalDemandkW).toFixed(2) * (parseFloat(_txtDemandCostkW).toFixed(2));

    // Total Energy + Demand Charges
    var total = 0;
    total = parseFloat(EnergyCharges + ((parseFloat(DemandkWCharge)))).toFixed(2);

    // Calculate Increment
    var Incremental = 0;
    var TotalCharge = $('#_txtTotalCharges').val();
    var TotalbaseCStoreUtilityCosts = $('#Text6').val();
    var _txtIncrementOverTotalBase = $('#_txtIncrementOverTotalBase');

    if (total == "") total = 0;
    if (TotalbaseCStoreUtilityCosts == "") TotalbaseCStoreUtilityCosts = 0;
    if (_txtIncrementOverTotalBase == "") _txtIncrementOverTotalBase = 0;

    Incremental = (parseFloat(total - parseFloat(TotalbaseCStoreUtilityCosts).toFixed(2)));

    // Calculate Cost Per Charge
    var CostPerCharge = 0;
    var IncrementOverTotalBase = $('#_txtIncrementOverTotalBase').val();
    var MonthlyChargesPerMonth = $('#_txtMonthlyChargesPerMonth').val();
    var _txtCostPercharges = $('#_txtCostPercharges').val();

    if (IncrementOverTotalBase == "") IncrementOverTotalBase = 0;
    if (MonthlyChargesPerMonth == "") MonthlyChargesPerMonth = 0;
    if (_txtCostPercharges == "") _txtCostPercharges = 0;

    var PerMonth = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));

    CostPerCharge = ((parseFloat(Incremental).toFixed(2)) / (parseFloat(PerMonth).toFixed(2)));

    // Calculate Cost Per kWh
    var CostPerkWh = 0;
    CostPerkWh = ((parseFloat(CostPerCharge).toFixed(2)) / (parseFloat(_txtkWhCharge).toFixed(2)));

    $('#_hdCostPerkWh').val(parseFloat(CostPerkWh).toFixed(2));
    $('#_txtCostPerkWh').val(formatter.format(CostPerkWh));

    $('#_hdMonthlyChargesPerMonth').val(parseFloat(MonthlyCharge).toFixed(0));
    $('#_txtMonthlyChargesPerMonth').val(parseFloat(MonthlyCharge).toLocaleString('en'));
}

// Indirect Profile Calculations
function CalculateConversionRateSensisivityAnalysis() {
    
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    var MonthlyChargesPerMonth = 0;
    var ConversionRate = 0;
    var BasketSize = 0;
    var GrossMargin = 0;
    var IndBSIS = 0;
    var IndGMT = 0;
    var CRGPS1 = 0;
    var CRGPS2 = 0;
    var CRGPS3 = 0;

    var $IndChargers = $('#_txtIndChargers');
    var $IndChargesChargerDay = $('#_txtIndChargesChargerDay');
    var $txtDaysMonth = $('#Text21');
    var $IndMonthlyChargesPerMonth = $('#_txtIndMonthlyChargesPerMonth');
    var $IndConvRateStoreTrans = $('#_txtIndConvRateStoreTrans');
    var $IndBasketSizeInSale = $('#_txtIndBasketSizeInSale');
    var $IndGrossMarginTotal = $('#_txtIndGrossMarginTotal');
    var $BSIS = $('#_txtIndbsis');
    var $GMT = $('#_txtIndGMT');
    var $txtCRGPS1 = $('#_txtCRGPS1');
    var $txtCRGPS2 = $('#_txtCRGPS2');
    var $txtCRGPS3 = $('#_txtCRGPS3');

    // Get values or set defaults
    var IndChargers = $IndChargers.val() || 0;
    var IndChargesChargerDay = $IndChargesChargerDay.val() || 0;
    var _txtDaysMonth = $txtDaysMonth.val() || 0;
    var IndMonthlyChargesPerMonth = $IndMonthlyChargesPerMonth.val() || 0;
    var IndConvRateStoreTrans = $IndConvRateStoreTrans.val() || 0;
    var IndBasketSizeInSale = $IndBasketSizeInSale.val() || 0;
    var IndGrossMarginTotal = $IndGrossMarginTotal.val() || 0;
    var BSIS = $BSIS.val() || 0;
    var GMT = $GMT.val() || 0;
    var _txtCRGPS1 = $txtCRGPS1.val() || 0;
    var _txtCRGPS2 = $txtCRGPS2.val() || 0;
    var _txtCRGPS3 = $txtCRGPS3.val() || 0;

    // Conversion rate calculations
    MonthlyChargesPerMonth = (parseFloat(IndChargers).toFixed(2)) * (parseFloat(IndChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));
    $IndMonthlyChargesPerMonth.val(parseFloat(MonthlyChargesPerMonth).toFixed(2));
    $IndMonthlyChargesPerMonth.val(parseFloat(MonthlyChargesPerMonth).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ","));

    ConversionRate = (parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(20)) / (parseFloat(100))).toFixed(2);
    $IndConvRateStoreTrans.val(parseFloat(ConversionRate).toFixed(2));
    $('#_hdIndConvRateStoreTrans').val(parseFloat(ConversionRate).toFixed(0));
    $IndConvRateStoreTrans.val(parseFloat(ConversionRate).toFixed(0));

    BasketSize = (parseFloat(ConversionRate).toFixed(2)) * (parseFloat(BSIS).toFixed(2));
    $IndBasketSizeInSale.val(parseFloat(BasketSize).toFixed(2));
    $('#_hdIndBasketSizeInSale').val(parseFloat(BasketSize).toFixed(2));
    $IndBasketSizeInSale.val(formatter.format(BasketSize));

    IndBSIS = (parseFloat(BasketSize).toFixed(2)) / (parseFloat(ConversionRate).toFixed(2));

    GrossMargin = (parseFloat(BasketSize).toFixed(2)) * ((parseFloat(GMT).toFixed(2)) / (parseFloat(100))).toFixed(2);
    $IndGrossMarginTotal.val(parseFloat(GrossMargin).toFixed(2));
    $('#_hdIndGrossMarginTotal').val(parseFloat(GrossMargin).toFixed(2));
    $IndGrossMarginTotal.val(formatter.format(GrossMargin));

    IndGMT = (parseFloat(GrossMargin).toFixed(2)) * (parseFloat(100).toFixed(2));
    var IndGMT1 = (parseFloat(IndGMT).toFixed(2)) / (parseFloat(BasketSize).toFixed(2));

    // 40% Conversion rate
    CRGPS1 = ((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(40)) / (parseFloat(100))).toFixed(2)) * (parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(IndGMT1).toFixed(2)) / (parseFloat(100))).toFixed(2);
    $txtCRGPS1.val(parseFloat(CRGPS1).toFixed(2));
    $('#_hdCRGPS1').val(parseFloat(CRGPS1).toFixed(2));
    $txtCRGPS1.val(formatter.format(CRGPS1));

    // 60% Conversion rate
    CRGPS2 = ((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(60)) / (parseFloat(100))).toFixed(2)) * (parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(IndGMT1).toFixed(2)) / (parseFloat(100))).toFixed(2);
    $txtCRGPS2.val(parseFloat(CRGPS2).toFixed(2));
    $('#_hdCRGPS2').val(parseFloat(CRGPS2).toFixed(2));
    $txtCRGPS2.val(formatter.format(CRGPS2));

    // 80% Conversion rate
    CRGPS3 = ((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(80)) / (parseFloat(100))).toFixed(2)) * (parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(IndGMT1).toFixed(2)) / (parseFloat(100))).toFixed(2);
    $txtCRGPS3.val(parseFloat(CRGPS3).toFixed(2));
    $('#_hdCRGPS3').val(parseFloat(CRGPS3).toFixed(2));
    $txtCRGPS3.val(formatter.format(CRGPS3));
}


//Cap Ex Modeling 
function CalculateCapeExModeling() {
    

    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    //Cap Ex Total Cap Calculation
    var CapTotalCapEx = 0;
    var _txtCapChargers = $('#_txtCapChargers').val();
    var _txtCapChargePerChargerPerDay = $('#_txtCapChargePerChargerPerDay').val();
    var _txtCapTotalCapEx = $('#_txtCapTotalCapEx').val();
    var _txtCapitalCostOfWHCharger = $('#Text28').val();

    if (_txtCapChargers == "") _txtCapChargers = 0;
    if (_txtCapChargePerChargerPerDay == "") _txtCapChargePerChargerPerDay = 0;
    if (_txtCapTotalCapEx == "") _txtCapTotalCapEx = 0;
    if (_txtCapitalCostOfWHCharger == "") _txtCapitalCostOfWHCharger = 0;

    CapTotalCapEx = (parseFloat(_txtCapitalCostOfWHCharger).toFixed(2)) * (parseFloat(_txtCapChargers).toFixed(2));

    $('#_hdCapTotalCapEx').val(parseFloat(CapTotalCapEx).toFixed(2));
    $('#_txtCapTotalCapEx').val(formatter.format(CapTotalCapEx));

    //Cap ex Hurdle Calculation
    var CapHurdleReturn = 0;
    var _txtHurdleRateROI = $('#Text29').val();
    if (_txtHurdleRateROI == "") _txtHurdleRateROI = 0;
    CapHurdleReturn = ((parseFloat(_txtHurdleRateROI).toFixed(2)) / (parseFloat(100).toFixed(2))) * (parseFloat(CapTotalCapEx).toFixed(2));

    $('#_hdCapHurdleReturn').val(parseFloat(CapHurdleReturn).toFixed(2));
    $('#_txtCapHurdleReturn').val(formatter.format(CapHurdleReturn));

    //Cap Ex Indirect GP$ per Year
    var CapMonthlyChargesPerMonth = 0;
    var ConversionRate = 0;
    var BasketSize = 0;
    var GrossMargin = 0;
    var CapGrossMargin = 0;
    var IndChargesChargerDay = $('#_txtIndChargesChargerDay').val();
    var BSIS = $('#_txtIndbsis').val();

    if (IndChargesChargerDay == "") IndChargesChargerDay = 0;
    if (BSIS == "") BSIS = 0;

    CapMonthlyChargesPerMonth = (parseFloat(_txtCapChargers).toFixed(2)) * (parseFloat(_txtCapChargePerChargerPerDay).toFixed(2)) * (parseFloat(30).toFixed(2));
    ConversionRate = (parseFloat(CapMonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(20)) / (parseFloat(100))).toFixed(2);
    BasketSize = (parseFloat(ConversionRate).toFixed(2)) * (parseFloat(BSIS).toFixed(2));
    GrossMargin = (parseFloat(BasketSize).toFixed(2)) * ((parseFloat(35)) / (parseFloat(100))).toFixed(2);
    CapGrossMargin = (parseFloat(GrossMargin).toFixed(2)) * (parseFloat(12).toFixed(2));

    $('#_hdCapIndirectGPSPerYear').val(parseFloat(CapGrossMargin).toFixed(2));
    $('#_txtCapIndirectGPSPerYear').val(formatter.format(CapGrossMargin));

    //Cap Ex Required Charging net $* 
    var CapRequiredChargingNetS = 0;
    var _txtCapIndirectGPSPerYear = $('#_hdCapIndirectGPSPerYear').val();
    if (_txtCapIndirectGPSPerYear == "") _txtCapIndirectGPSPerYear = 0;
    CapRequiredChargingNetS = (parseFloat($('#_hdCapHurdleReturn').val()).toFixed(2)) - (parseFloat(_txtCapIndirectGPSPerYear).toFixed(2));

    $('#_hdCapRequiredChargingNetS').val(parseFloat(CapRequiredChargingNetS).toFixed(2));
    $('#_txtCapRequiredChargingNetS').val(formatter.format(CapRequiredChargingNetS));

    //Cap Ex Number of charge per year
    var CapNumberOfChargePerYear = 0;
    CapNumberOfChargePerYear = (parseFloat(_txtCapChargers).toFixed(2)) * (parseFloat(_txtCapChargePerChargerPerDay).toFixed(2)) * (parseFloat(365).toFixed(2));
    $('#_hdCapChargePerYear').val(parseFloat(CapNumberOfChargePerYear).toFixed(0));
    $('#_txtCapChargePerYear').val(parseFloat(CapNumberOfChargePerYear).toFixed(0).replace(/\B(?=(\d{3})+(?!\d))/g, ","));

    //Cap Ex  Required net $ per charge**
    var CapRequiredNet$PerCharge = 0;
    CapRequiredNet$PerCharge = (parseFloat(CapRequiredChargingNetS).toFixed(2)) / (parseFloat(CapNumberOfChargePerYear).toFixed(2));
    $('#_hdCapRequiredNetSPerCharge').val(parseFloat(CapRequiredNet$PerCharge).toFixed(2));
    $('#_txtCapRequiredNetSPerCharge').val(formatter.format(CapRequiredNet$PerCharge));

    //Cap Ex Required Gross $ Per 
    var DemandkW = 0;
    var TotalMonthlykWh = 0;

    var _txtNumberOfCharger = $('#_txtCapChargers').val();
    var _txtNumberChargePerChargerPerDay = $('#_txtCapChargePerChargerPerDay').val();
    var _txtkWhCharge = $('#Text35').val();
    var _txtkWCharger = $('#Text22').val();
    var _txtTotalBaseCStoreUtilityCosts = $('#Text25').val();
    var _txtBaseCStoreEnergyCosts = $('#Text23').val();
    var _txtEnergyCostkwh = $('#Text26').val();
    var _txtChargers = $('#_txtCapChargers').val();
    var _txtChargesChargerDay = $('#_txtCapChargePerChargerPerDay').val();
    var _txtDemandCostkW = $('#Text27').val();
    var _txtDemandBasekW = $('#Text36').val();
    var _txtDaysMonth = $('#Text21').val();

    if (_txtNumberOfCharger == "") _txtNumberOfCharger = 0;
    if (_txtNumberChargePerChargerPerDay == "") _txtNumberChargePerChargerPerDay = 0;
    if (_txtkWhCharge == "") _txtkWhCharge = 0;
    if (_txtBaseCStoreEnergyCosts == "") _txtBaseCStoreEnergyCosts = 0;
    if (_txtEnergyCostkwh == "") _txtEnergyCostkwh = 0;
    if (_txtkWCharger == "") _txtkWCharger = 0;
    if (_txtChargers == "") _txtChargers = 0;
    if (_txtChargesChargerDay == "") _txtChargesChargerDay = 0;
    if (_txtDemandBasekW == "") _txtDemandBasekW = 0;
    if (_txtDemandCostkW == "") _txtDemandCostkW = 0;
    if (_txtDaysMonth == "") _txtDaysMonth = 0;
    if (_txtTotalBaseCStoreUtilityCosts == "") _txtTotalBaseCStoreUtilityCosts = 0;

    //Energy Charges
    var ChargesPerMonth = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));
    var IncrementalMonthlykWh = parseFloat(ChargesPerMonth * ((parseFloat(_txtkWhCharge).toFixed(2))));
    var BaseMonthlykWh = ((parseFloat(_txtBaseCStoreEnergyCosts).toFixed(2)) / (parseFloat(_txtEnergyCostkwh).toFixed(2)));
    var TotalMonthlykWh = parseFloat(BaseMonthlykWh + IncrementalMonthlykWh).toFixed(2);

    //Demand Charges
    var DemandkW = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtkWCharger).toFixed(2));
    var TotalDemandkW = parseFloat(DemandkW + ((parseFloat(_txtDemandBasekW)))).toFixed(2);
    var TotalDemandCharge = parseFloat(TotalDemandkW).toFixed(2) * (parseFloat(_txtDemandCostkW).toFixed(2));

    //Total Energy Charge
    var TotalEnergyCharge = parseFloat(TotalMonthlykWh).toFixed(2) * parseFloat(_txtEnergyCostkwh).toFixed(2);

    //SUM(Energy:Demand)
    var SumEnergyAndDemand = parseFloat(TotalEnergyCharge + ((parseFloat(TotalDemandCharge)))).toFixed(2);

    //Increment Over Total Base
    var IncrementOverTotalBase = parseFloat(SumEnergyAndDemand - ((parseFloat(_txtTotalBaseCStoreUtilityCosts)))).toFixed(2);

    //Cost Per Charge
    var CostPerCharge = (parseFloat(IncrementOverTotalBase).toFixed(2)) / (parseFloat(ChargesPerMonth).toFixed(2));

    //Required Gross $ per Charge
    var RequiredGrossSperCharge = parseFloat(CapRequiredNet$PerCharge + ((parseFloat(CostPerCharge)))).toFixed(2);
    $('#_hdCapRequiredGrossSPerCharge').val(parseFloat(RequiredGrossSperCharge).toFixed(2));
    $('#_txtCapRequiredGrossSPerCharge').val(formatter.format(RequiredGrossSperCharge));
}

//Calculate Basket Sensitivity Analysis
function CalculateConversionAndBasketSensitivityAnalysis() {
    

    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });

    // Cap Ex Total Cap Calculation
    var ConTotalCapEx = 0;
    var _txtConvChargers = $('#_txtConvChargers').val();
    var _txtConvChargePerChargersPerDay = $('#_txtConvChargePerChargersPerDay').val();
    var _txtCapTotalCapEx = $('#_txtCapTotalCapEx').val();
    var _txtCapitalCostOfWHCharger = $('#Text28').val();

    if (_txtConvChargers == "") _txtConvChargers = 0;
    if (_txtConvChargePerChargersPerDay == "") _txtConvChargePerChargersPerDay = 0;
    if (_txtCapTotalCapEx == "") _txtCapTotalCapEx = 0;
    if (_txtCapitalCostOfWHCharger == "") _txtCapitalCostOfWHCharger = 0;

    ConTotalCapEx = (parseFloat(_txtCapitalCostOfWHCharger).toFixed(2)) * (parseFloat(_txtConvChargers).toFixed(2));

    // Cap Ex Hurdle Calculation
    var ConHurdleReturn = 0;
    var _txtHurdleRateROI = $('#Text29').val();
    if (_txtHurdleRateROI == "") _txtHurdleRateROI = 0;
    ConHurdleReturn = ((parseFloat(_txtHurdleRateROI).toFixed(2)) / (parseFloat(100).toFixed(2))) * (parseFloat(ConTotalCapEx).toFixed(2));

    // Cap Ex Number of charge per year
    var ConNumberOfChargePerYear = 0;
    ConNumberOfChargePerYear = (parseFloat(_txtConvChargers).toFixed(2)) * (parseFloat(_txtConvChargePerChargersPerDay).toFixed(2)) * (parseFloat(365).toFixed(2));

    // Cap Ex Indirect GP$ per Year
    var ConversionRate = 0;
    var BasketSize = 0;
    var GrossMargin = 0;
    var IndChargesChargerDay = $('#_txtIndChargesChargerDay').val();
    var BSIS = $('#_txtIndbsis').val();

    if (IndChargesChargerDay == "") IndChargesChargerDay = 0;
    if (BSIS == "") BSIS = 0;

    ConversionRate = (parseFloat(ConNumberOfChargePerYear).toFixed(2)) * ((parseFloat(20)) / (parseFloat(100))).toFixed(2);
    BasketSize = (parseFloat(ConversionRate).toFixed(2)) * (parseFloat(BSIS).toFixed(2));
    GrossMargin = (parseFloat(BasketSize).toFixed(2)) * ((parseFloat(35)) / (parseFloat(100))).toFixed(2);
    CapGrossMargin = (parseFloat(GrossMargin).toFixed(2)) * (parseFloat(12).toFixed(2));

    // Cap Ex Required Gross $ Per
    var DemandkW = 0;
    var TotalMonthlykWh = 0;

    var _txtNumberOfCharger = $('#_txtConvChargers').val();
    var _txtNumberChargePerChargerPerDay = $('#_txtConvChargePerChargersPerDay').val();
    var _txtkWhCharge = $('#Text35').val();
    var _txtkWCharger = $('#Text22').val();
    var _txtTotalBaseCStoreUtilityCosts = $('#Text25').val();
    var _txtBaseCStoreEnergyCosts = $('#Text23').val();
    var _txtEnergyCostkwh = $('#Text26').val();
    var _txtChargers = $('#_txtConvChargers').val();
    var _txtChargesChargerDay = $('#_txtConvChargePerChargersPerDay').val();
    var _txtDemandCostkW = $('#Text27').val();
    var _txtDemandBasekW = $('#Text36').val();
    var _txtDaysMonth = $('#Text21').val();

    if (_txtNumberOfCharger == "") _txtNumberOfCharger = 0;
    if (_txtNumberChargePerChargerPerDay == "") _txtNumberChargePerChargerPerDay = 0;
    if (_txtkWhCharge == "") _txtkWhCharge = 0;
    if (_txtBaseCStoreEnergyCosts == "") _txtBaseCStoreEnergyCosts = 0;
    if (_txtEnergyCostkwh == "") _txtEnergyCostkwh = 0;
    if (_txtkWCharger == "") _txtkWCharger = 0;
    if (_txtChargers == "") _txtChargers = 0;
    if (_txtChargesChargerDay == "") _txtChargesChargerDay = 0;
    if (_txtDemandBasekW == "") _txtDemandBasekW = 0;
    if (_txtDemandCostkW == "") _txtDemandCostkW = 0;
    if (_txtDaysMonth == "") _txtDaysMonth = 0;
    if (_txtTotalBaseCStoreUtilityCosts == "") _txtTotalBaseCStoreUtilityCosts = 0;

    // Energy Charges
    var ChargesPerMonth = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));
    var IncrementalMonthlykWh = parseFloat(ChargesPerMonth * ((parseFloat(_txtkWhCharge).toFixed(2))));
    var BaseMonthlykWh = ((parseFloat(_txtBaseCStoreEnergyCosts).toFixed(2)) / (parseFloat(_txtEnergyCostkwh).toFixed(2)));
    var TotalMonthlykWh = parseFloat(BaseMonthlykWh + IncrementalMonthlykWh).toFixed(2);

    // Demand Charges
    var DemandkW = (parseFloat(_txtChargers).toFixed(2)) * (parseFloat(_txtkWCharger).toFixed(2));
    var TotalDemandkW = parseFloat(DemandkW + ((parseFloat(_txtDemandBasekW)))).toFixed(2);
    var TotalDemandCharge = parseFloat(TotalDemandkW).toFixed(2) * (parseFloat(_txtDemandCostkW).toFixed(2));

    // Total Energy Charge
    var TotalEnergyCharge = parseFloat(TotalMonthlykWh).toFixed(2) * parseFloat(_txtEnergyCostkwh).toFixed(2);

    // SUM(Energy:Demand)
    var SumEnergyAndDemand = parseFloat(TotalEnergyCharge + ((parseFloat(TotalDemandCharge)))).toFixed(2);

    // Increment Over Total Base
    var IncrementOverTotalBase = parseFloat(SumEnergyAndDemand - ((parseFloat(_txtTotalBaseCStoreUtilityCosts)))).toFixed(2);

    // Cost Per Charge
    var CostPerCharge = (parseFloat(IncrementOverTotalBase).toFixed(2)) / (parseFloat(ChargesPerMonth).toFixed(2));

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    var MonthlyChargesPerMonth = 0;
    var ConversionRate = 0;
    var BasketSize = 0;
    var GrossMargin20 = 0;
    var IndBSIS = 0;
    var GrossMargin40 = 0;
    var GrossMargin60 = 0;
    var GrossMargin80 = 0;

    var IndChargers = $('#_txtConvChargers').val();
    var IndChargesChargerDay = $('#_txtConvChargePerChargersPerDay').val();
    var _txtDaysMonth = $('#Text21').val();
    var IndMonthlyChargesPerMonth = $('#_txtIndMonthlyChargesPerMonth').val();
    var IndConvRateStoreTrans = $('#_txtIndConvRateStoreTrans').val();
    var IndBasketSizeInSale = $('#_txtIndBasketSizeInSale').val();
    var IndGrossMarginTotal = $('#_txtIndGrossMarginTotal').val();
    var BSIS = $('#_txtIndbsis').val();
    var _txtConvFourtyRequiredGrossSPerCharge = $('#_txtConvFourtyRequiredGrossSPerCharge').val();
    var _txtConvSixtyRequiredGrossSPerCharge = $('#_txtConvSixtyRequiredGrossSPerCharge').val();
    var _txtConvEightyRequiredGrossSPerCharge = $('#_txtConvEightyRequiredGrossSPerCharge').val();
    var _txtCRGPS1 = $('#_txtCRGPS1').val();
    var _txtCRGPS2 = $('#_txtCRGPS2').val();
    var _txtCRGPS3 = $('#_txtCRGPS3').val();

    if (IndChargers == "") IndChargers = 0;
    if (IndChargesChargerDay == "") IndChargesChargerDay = 0;
    if (_txtDaysMonth == "") _txtDaysMonth = 0;
    if (IndMonthlyChargesPerMonth == "") IndMonthlyChargesPerMonth = 0;
    if (IndConvRateStoreTrans == "") IndConvRateStoreTrans = 0;
    if (IndBasketSizeInSale == "") IndBasketSizeInSale = 0;
    if (IndGrossMarginTotal == "") IndGrossMarginTotal = 0;
    if (BSIS == "") BSIS = 0;
    if (_txtConvFourtyRequiredGrossSPerCharge == "") _txtConvFourtyRequiredGrossSPerCharge = 0;
    if (_txtConvSixtyRequiredGrossSPerCharge == "") _txtConvSixtyRequiredGrossSPerCharge = 0;
    if (_txtConvEightyRequiredGrossSPerCharge == "") _txtConvEightyRequiredGrossSPerCharge = 0;
    if (_txtCRGPS1 == "") _txtCRGPS1 = 0;
    if (_txtCRGPS2 == "") _txtCRGPS2 = 0;
    if (_txtCRGPS3 == "") _txtCRGPS3 = 0;

    // Conversion rate
    MonthlyChargesPerMonth = (parseFloat(IndChargers).toFixed(2)) * (parseFloat(IndChargesChargerDay).toFixed(2)) * (parseFloat(_txtDaysMonth).toFixed(2));

    ConversionRate = (parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(20)) / (parseFloat(100))).toFixed(2);

    BasketSize = (parseFloat(ConversionRate).toFixed(2)) * (parseFloat(BSIS).toFixed(2));

    IndBSIS = (parseFloat(BasketSize).toFixed(2)) / (parseFloat(ConversionRate).toFixed(2));

    GrossMargin20 = (parseFloat(BasketSize).toFixed(2)) * ((parseFloat(35)) / (parseFloat(100))).toFixed(2);
    GrossMargin20.value = parseFloat(GrossMargin20).toFixed(2);

    // 40% Conversion rate
    GrossMargin40 = (((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(40)))) / (parseFloat(100).toFixed(2))) * (((parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(35)))) / (parseFloat(100).toFixed(2)));
    GrossMargin40.value = parseFloat(GrossMargin40).toFixed(2);

    // 60% Conversion rate
    GrossMargin60 = (((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(60)))) / (parseFloat(100).toFixed(2))) * (((parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(35)))) / (parseFloat(100).toFixed(2)));
    GrossMargin60.value = parseFloat(GrossMargin60).toFixed(2);

    // 80% Conversion rate
    GrossMargin80 = (((parseFloat(MonthlyChargesPerMonth).toFixed(2)) * ((parseFloat(80)))) / (parseFloat(100).toFixed(2))) * (((parseFloat(IndBSIS).toFixed(2)) * ((parseFloat(35)))) / (parseFloat(100).toFixed(2)));
    GrossMargin80.value = parseFloat(GrossMargin80).toFixed(2);

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    var RGPC20 = 0;
    var RGPC201 = 0;
    var RGPC40 = 0;
    var RGPC401 = 0;
    var RGPC60 = 0;
    var RGPC601 = 0;
    var RGPC80 = 0;
    var RGPC801 = 0;

    

    RGPC201 = ((parseFloat(ConHurdleReturn).toFixed(2) - ((parseFloat(GrossMargin20).toFixed(2)) * (parseFloat(12).toFixed(2)))) / (parseFloat(ConNumberOfChargePerYear).toFixed(2)));
    RGPC20 = parseFloat(RGPC201 + ((parseFloat(CostPerCharge)))).toFixed(2);
    $('#_hdConvTwentyRequiredGrossSPerCharge').val(parseFloat(RGPC20).toFixed(2));
    $('#_txtConvTwentyRequiredGrossSPerCharge').val(formatter.format(RGPC20));

    RGPC401 = ((parseFloat(ConHurdleReturn).toFixed(2) - ((parseFloat(GrossMargin40).toFixed(2)) * (parseFloat(12).toFixed(2)))) / (parseFloat(ConNumberOfChargePerYear).toFixed(2)));
    RGPC40 = parseFloat(RGPC401 + ((parseFloat(CostPerCharge)))).toFixed(2);
    $('#_hdConvFourtyRequiredGrossSPerCharge').val(parseFloat(RGPC40).toFixed(2));
    $('#_txtConvFourtyRequiredGrossSPerCharge').val(formatter.format(RGPC40));

    RGPC601 = ((parseFloat(ConHurdleReturn).toFixed(2) - ((parseFloat(GrossMargin60).toFixed(2)) * (parseFloat(12).toFixed(2)))) / (parseFloat(ConNumberOfChargePerYear).toFixed(2)));
    RGPC60 = parseFloat(RGPC601 + ((parseFloat(CostPerCharge)))).toFixed(2);
    $('#_hdConvSixtyRequiredGrossSPerCharge').val(parseFloat(RGPC60).toFixed(2));
    $('#_txtConvSixtyRequiredGrossSPerCharge').val(formatter.format(RGPC60));

    RGPC801 = ((parseFloat(ConHurdleReturn).toFixed(2) - ((parseFloat(GrossMargin80).toFixed(2)) * (parseFloat(12).toFixed(2)))) / (parseFloat(ConNumberOfChargePerYear).toFixed(2)));
    RGPC80 = parseFloat(RGPC801 + ((parseFloat(CostPerCharge)))).toFixed(2);
    $('#_hdConvEightyRequiredGrossSPerCharge').val(parseFloat(RGPC80).toFixed(2));
    $('#_txtConvEightyRequiredGrossSPerCharge').val(formatter.format(RGPC80));

}