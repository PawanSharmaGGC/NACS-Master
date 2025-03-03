using CMS.Core;
using CMS.Helpers;
using Convenience.org.Components.Widgets.EVChargingCalculator;
using Convenience.org.Helpers;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading;

[assembly: RegisterWidget(identifier: EVChargingCalculatorWidget.IDENTIFIER,
    name: "EV Charging Calculator",
    viewComponentType: typeof(EVChargingCalculatorWidget),
    propertiesType: null, Description = "EV Charging Calculator",
    IconClass = "icon-pda", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EVChargingCalculator;

public class EVChargingCalculatorWidget : ViewComponent
{
    public const string IDENTIFIER = "EVChargingCalculator";
    private readonly IEventLogService _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EVChargingCalculatorWidget(IEventLogService logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public IViewComponentResult Invoke(int step, EVChargingCalculatorViewModel model)
    {
        model = model ?? new EVChargingCalculatorViewModel();

        if (step == 0)
        {
            model.CurrentStep = 1;
        }
        else
        {
            DataCapture(model);
        }


        if (model.CurrentStep == 2)
        {
            DataCalculations(model);
        }

        if (model.CurrentStep == 3)
        {
            // finish button action
        }

        // Save to session
        HttpContext.Session.SetObject("EVChargingCalculatorData", model);

        //Return the correct view for the current step
        if (step > 0)
            return View($"~/Components/Widgets/EVChargingCalculator/_EVChargingCalculatorStep1.cshtml", model);
        else
        {
            return View("~/Components/Widgets/EVChargingCalculator/EVChargingCalculator.cshtml", model);
        }
    }

    private void DataCapture(EVChargingCalculatorViewModel model)
    {
        //Direct Profit 
        #region Direct Profit
        model._txtNumberOfCharger = ValidationHelper.GetString(model._txtNumberOfCharger.Trim(), ""); //Number Of Charging Station
        model.Text0 = model._txtNumberOfCharger.Trim().ToString();

        model._txtNumberChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charging Station
        model.Text1 = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        model._txtDaysMonth = model._txtDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days
        model.Text2 = model._txtDaysMonth.Trim().ToString();

        model._txtkWCharger = model._txtkWCharger.Trim().ToString(); //kwh Charge Constant 22.83
        model.Text3 = model._txtkWCharger.Trim().ToString();

        model._txtBaseCStoreEnergyCosts = model._txtBaseCStoreEnergyCosts.Trim().ToString();//Base C-Store Energy Costs
        model.Text4 = model._txtBaseCStoreEnergyCosts.Trim().ToString();

        model._txtBaseCStoreDemandCharges = model._txtBaseCStoreDemandCharges.Trim().ToString();//Base C-Store Demand Charges
        model.Text5 = model._txtBaseCStoreDemandCharges.Trim().ToString();

        model._txtTotalBaseCStoreUtilityCosts = model._txtTotalBaseCStoreUtilityCosts.Trim().ToString();//Total Base C-Store Utility Costs
        model.Text6 = model._txtTotalBaseCStoreUtilityCosts.Trim().ToString();

        model._txtEnergyCostkwh = model._txtEnergyCostkwh.Trim().ToString();//Energy Cost kwh
        model.Text7 = model._txtEnergyCostkwh.Trim().ToString();

        model._txtDemandCostkW = model._txtDemandCostkW.Trim().ToString();//Demand Cost kW
        model.Text8 = model._txtDemandCostkW.Trim().ToString();

        model._txtCapitalCostOfWHCharger = model._txtCapitalCostOfWHCharger.Trim().ToString();//Capital Cost Of WHC harger
        model.Text9 = model._txtCapitalCostOfWHCharger.Trim().ToString();

        model._txtHurdleRateROI = model._txtHurdleRateROI.Trim().ToString();//Hurdle Rate ROI
        model.Text10 = model._txtHurdleRateROI.Trim().ToString();

        model._txtNumberOfCharger = model._txtNumberOfCharger.Trim().ToString();//Number Of Charger
        model.Text11 = model._txtNumberOfCharger.Trim().ToString();

        model._txtNumberChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();//Number Charge Per Charger Per Day
        model.Text12 = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        model._txtEnergyCharges = model._txtEnergyCharges.Trim().ToString();
        model.Text13 = model._txtEnergyCharges.Trim().ToString();

        model._txtTotalDemandkW = model._txtTotalDemandkW.Trim().ToString();//Total Demand kW
        model.Text14 = model._txtTotalDemandkW.Trim().ToString();

        model._txtkWhCharge = model._txtkWhCharge.Trim().ToString(); //kwh Charge Constant 22.83
        model.Text15 = model._txtkWhCharge.Trim().ToString();

        model._txtBasekW = model._txtBasekW.Trim().ToString();
        model.Text16 = model._txtBasekW.Trim().ToString();//Number Of kW per Charging Station 

        model._txtCostPercharges = model._txtCostPercharges.Trim().ToString();
        model.Text17 = model._txtCostPercharges.Trim().ToString();//Cost Per kWh 

        #endregion

        //Indirect Profit 
        #region Indirect Profit
        model._txtNumberOfCharger = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station
        model.Text19 = model._txtNumberOfCharger.Trim().ToString();

        model._txtNumberChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charging Station
        model.Text20 = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        model._txtDaysMonth = model._txtDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days
        model.Text21 = model._txtDaysMonth.Trim().ToString();

        model._txtkWCharger = model._txtkWCharger.Trim().ToString(); //kwh Charge Constant 22.83
        model.Text22 = model._txtkWCharger.Trim().ToString();

        model._txtBaseCStoreEnergyCosts = model._txtBaseCStoreEnergyCosts.Trim().ToString();//Base C-Store Energy Costs
        model.Text23 = model._txtBaseCStoreEnergyCosts.Trim().ToString();

        model._txtBaseCStoreDemandCharges = model._txtBaseCStoreDemandCharges.Trim().ToString();//Base C-Store Demand Charges
        model.Text24 = model._txtBaseCStoreDemandCharges.Trim().ToString();

        model._txtTotalBaseCStoreUtilityCosts = model._txtTotalBaseCStoreUtilityCosts.Trim().ToString();//Total Base C-Store Utility Costs
        model.Text25 = model._txtTotalBaseCStoreUtilityCosts.Trim().ToString();

        model._txtEnergyCostkwh = model._txtEnergyCostkwh.Trim().ToString();//Energy Cost kwh
        model.Text26 = model._txtEnergyCostkwh.Trim().ToString();

        model._txtDemandCostkW = model._txtDemandCostkW.Trim().ToString();//Demand Cost kW
        model.Text27 = model._txtDemandCostkW.Trim().ToString();

        model._txtCapitalCostOfWHCharger = model._txtCapitalCostOfWHCharger.Trim().ToString();//Capital Cost Of WHC harger
        model.Text28 = model._txtCapitalCostOfWHCharger.Trim().ToString();

        model._txtHurdleRateROI = model._txtHurdleRateROI.Trim().ToString();//Hurdle Rate ROI
        model.Text29 = model._txtHurdleRateROI.Trim().ToString();

        model._txtNumberOfCharger = model._txtNumberOfCharger.Trim().ToString();//Number Of Charger
        model.Text30 = model._txtNumberOfCharger.Trim().ToString();

        model._txtNumberChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();//Number Charge Per Charger Per Day
        model.Text31 = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        model._txtEnergyCharges = model._txtEnergyCharges.Trim().ToString();
        model.Text32 = model._txtEnergyCharges.Trim().ToString();

        model._txtTotalDemandkW = model._txtTotalDemandkW.Trim().ToString();//Total Demand kW
        model.Text33 = model._txtTotalDemandkW.Trim().ToString();

        model._txtkWhCharge = model._txtkWhCharge.Trim().ToString(); //kwh Charge Constant 22.83
        model.Text35 = model._txtkWhCharge.Trim().ToString();

        model._txtBasekW = model._txtBasekW.Trim().ToString();
        model.Text36 = model._txtBasekW.Trim().ToString();//Number Of kW per Charging Station 

        model._txtCostPercharges = model._txtCostPercharges.Trim().ToString();
        model.Text37 = model._txtCostPercharges.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 

        //_txtCostPercharges.Value.Trim().ToString();
        //Text37.Value = _txtCostPercharges.Value.Trim().ToString();//Cost Per kWh 


        #endregion
    }

    private void DataCalculations(EVChargingCalculatorViewModel model)
    {
        CultureInfo us = new CultureInfo("en-US");

        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        CultureInfo newCulture = new CultureInfo(currentCulture.Name);
        newCulture.NumberFormat.CurrencyNegativePattern = 1;
        Thread.CurrentThread.CurrentCulture = newCulture;

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;


        //Direct Profit Charge per month 
        #region Direct Profit
        model._txtNumberofChargingStations = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station            
        model._txtNumberofChargesperStationsperDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charges per Charger per Station           

        model._txtDaysMonth = model._txtDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days
                                                                     //_hdDaysMonth = _txtDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days

        model._txtkWhCharge = model._txtkWhCharge.Trim().ToString(); //kwh Charge Constant 22.83
                                                                     //_hdkWhCharge = _txtkWhCharge.Trim().ToString(); //kwh Charge Constant 22.83

        model._txtBaseCStoreEnergyCosts = model._txtBaseCStoreEnergyCosts.Trim().ToString();//Base C-Store Energy Costs
                                                                                            //_hdBaseCStoreEnergyCosts = _txtBaseCStoreEnergyCosts.Trim().ToString();//Base C-Store Energy Costs

        model._txtBaseCStoreDemandCharges = model._txtBaseCStoreDemandCharges.Trim().ToString();//Base C-Store Demand Charges
                                                                                                //_hdBaseCStoreDemandCharges = _txtBaseCStoreDemandCharges.Trim().ToString();//Base C-Store Demand Charges

        model._txtTotalBaseCStoreUtilityCosts = model._txtTotalBaseCStoreUtilityCosts.Trim().ToString();//Total Base C-Store Utility Costs
        model._hdTotalBaseCStoreUtilityCosts = model._hdTotalBaseCStoreUtilityCosts.Trim().ToString();//Total Base C-Store Utility Costs

        model._txtEnergyCostkwh = model._txtEnergyCostkwh.Trim().ToString();//Energy Cost kwh
        model._hdEnergyCostkwh = model._hdEnergyCostkwh.Trim().ToString();//Energy Cost kwh

        model._txtDemandCostkW = model._txtDemandCostkW.Trim().ToString();//Demand Cost kW
                                                                          //_hdDemandCostkW = _txtDemandCostkW.Trim().ToString();//Demand Cost kW  

        model._txtCapitalCostOfWHCharger = model._txtCapitalCostOfWHCharger.Trim().ToString();//Capital Cost Of WHC harger
                                                                                              //_hdCapitalCostOfWHCharger = _txtCapitalCostOfWHCharger.Trim().ToString();//Capital Cost Of WHC harger

        model._txtHurdleRateROI = model._txtHurdleRateROI.Trim().ToString();//Hurdle Rate ROI
                                                                            //_hdHurdleRateROI = _txtHurdleRateROI.Trim().ToString();//Hurdle Rate ROI

        model._txtNumberOfCharger = model._txtNumberOfCharger.Trim().ToString();//Number Of Charger
                                                                                //_hdNumberOfCharger = _txtNumberOfCharger.Trim().ToString();//Number Of Charger

        model._txtNumberChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();//Number Charge Per Charger Per Day
                                                                                                          //_hdNumberChargePerChargerPerDay = _txtNumberChargePerChargerPerDay.Trim().ToString();//Number Charge Per Charger Per Day


        //Number of charges per month
        var ChargesPerMonth = (ValidationHelper.GetDouble(model._txtNumberofChargingStations.ToString(), 0)) * (ValidationHelper.GetDouble(model._txtNumberofChargesperStationsperDay.ToString(), 0)) * (ValidationHelper.GetDouble(model._txtDaysMonth.ToString(), 0));
        model._txtChargespermonth = ChargesPerMonth.ToString("0");
        model._hdChargespermonth = ChargesPerMonth.ToString("0");

        //Incremental Monthly kWh
        var IncrementalMonthlykWh = (ValidationHelper.GetDouble(model._hdChargespermonth.ToString(), 0) * ValidationHelper.GetDouble(model._txtkWhCharge.ToString(), 0));
        model._txtIncrementalMonthlykWh = IncrementalMonthlykWh.ToString("c");
        //model._txtIncrementalMonthlykWh = IncrementalMonthlykWh.ToString("#,##0.00");
        model._hdIncrementalMonthlykWh = IncrementalMonthlykWh.ToString("0.00");

        //Base Monthly kWh
        var BaseMonthlykWh = (ValidationHelper.GetDouble(model._txtBaseCStoreEnergyCosts.ToString(), 0) / ValidationHelper.GetDouble(model._txtEnergyCostkwh.ToString(), 0));
        model._txtBaseMonthlykWh = BaseMonthlykWh.ToString("c");
        //_txtBaseMonthlykWh = BaseMonthlykWh.ToString("#,##0.00");
        model._hdBaseMonthlykWh = BaseMonthlykWh.ToString("0.00");

        //Total Monthly kWh
        var TotalMonthlykWhl = (ValidationHelper.GetDouble(model._hdBaseMonthlykWh.ToString(), 0) + ValidationHelper.GetDouble(model._hdIncrementalMonthlykWh.ToString(), 0));
        model._txtTotalMonthlykWh = TotalMonthlykWhl.ToString("c");
        //_txtTotalMonthlykWh = TotalMonthlykWhl.ToString("#,##0.00");
        model._hdTotalMonthlykWh = TotalMonthlykWhl.ToString("0.00");

        //Total Energy Charges kWh
        var TotalEnergyCharges = (ValidationHelper.GetDouble(model._hdTotalMonthlykWh.ToString(), 0) * ValidationHelper.GetDouble(model._txtEnergyCostkwh.ToString(), 0));
        model._txtTotalEnergyCharge = TotalEnergyCharges.ToString("c");
        model._hdTotalEnergyCharge = TotalEnergyCharges.ToString("0.00");

        //DEMAND CHARGES
        model._txtNumberOfChargers = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station
        model._txtDemandkWCharger = model._txtkWCharger.Trim().ToString(); //Number Of Charging Station
        model._hdDemandkWCharger = model._txtkWCharger.Trim().ToString(); //Number Of Charging Station

        //Demand kW
        var DemandkW = (ValidationHelper.GetDouble(model._txtNumberOfChargers.ToString(), 0) * ValidationHelper.GetDouble(model._txtkWCharger.ToString(), 0));
        model._txtDemandkW = DemandkW.ToString("0");
        model._hdDemandkW = DemandkW.ToString("0");

        //Base kW 
        //var DemandBasekW = Convert.ToDouble(model._txtBasekW.ToString());
        model._txtDemandBasekW = model._txtBasekW.ToString(); //DemandBasekW.ToString("0.00"); //Number kW Charging Station
        model._hdDemandBasekW = model._txtBasekW.ToString(); //DemandBasekW.ToString("0.00"); //Number kW Charging Station

        //Total Demand kW           
        var TotalDemandkW = (DemandkW + ValidationHelper.GetDouble(model._txtDemandBasekW.ToString(), 0));
        model._txtTotalDemandkW = TotalDemandkW.ToString("0");
        model._hdTotalDemandkW = TotalDemandkW.ToString("0");

        model._txtTotalDemandkW.Trim().ToString();//Total Demand kW
        model._hdTotalDemandkW.Trim().ToString();//Total Demand kW
        model.Text14 = model._hdTotalDemandkW.Trim().ToString();

        //Total Demand Charges
        var TotalDemandCharges = (ValidationHelper.GetDouble(model._hdTotalDemandkW.ToString(), 0) * ValidationHelper.GetDouble(model._txtDemandCostkW.ToString(), 0));
        model._txtTotalDemandCharge = TotalDemandCharges.ToString("c");
        model._hdTotalDemandCharge = TotalDemandCharges.ToString("c");

        //TOTAL UTILITY CHARGES
        model._txtChargers = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station
        model._hdChargers = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station      

        model._txtChargesChargerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charges per Charger per Station 
        model._hdChargesChargerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charges per Charger per Station 

        var ChargesperDayperMonth = ValidationHelper.GetDouble(model._txtChargers.ToString(), 0) * ValidationHelper.GetDouble(model._txtChargesChargerDay.ToString(), 0) * ValidationHelper.GetDouble(model._txtDaysMonth.ToString(), 0);
        model._txtMonthlyChargesPerMonth = ChargesperDayperMonth.ToString("#,##0"); // Charges per Day per Month
        model._hdMonthlyChargesPerMonth = ChargesperDayperMonth.ToString("0");  // Charges per Day per Month

        //Energy Charge
        var EvergyCharges = (ValidationHelper.GetDouble(model._hdTotalMonthlykWh.ToString(), 0) * ValidationHelper.GetDouble(model._txtEnergyCostkwh.ToString(), 0));
        model._txtEnergyCharges = EvergyCharges.ToString("c");
        model._hdEnergyCharges = EvergyCharges.ToString("0.00");

        //Demand Charge
        var DemandCharges = (ValidationHelper.GetDouble(model._hdTotalDemandkW.ToString(), 0) * ValidationHelper.GetDouble(model._txtDemandCostkW.ToString(), 0));
        model._txtDemandCharges = DemandCharges.ToString("c");
        model._hdDemandCharges = DemandCharges.ToString("0.00");

        //Total Charges
        var TotalCharges = (ValidationHelper.GetDouble(model._hdEnergyCharges.ToString(), 0)) + (ValidationHelper.GetDouble(model._hdDemandCharges.ToString(), 0));
        model._txtTotalCharges = TotalCharges.ToString("c");
        model._hdTotalCharges = TotalCharges.ToString("0.00");

        //Inceremtal Over Total Base
        var InceremtalOverTotal = (ValidationHelper.GetDouble(TotalCharges.ToString(), 0)) - (ValidationHelper.GetDouble(model._hdTotalBaseCStoreUtilityCosts.ToString(), 0));
        model._txtIncrementOverTotalBase = InceremtalOverTotal.ToString("c");
        model._hdIncrementOverTotalBase = InceremtalOverTotal.ToString("0.00");

        //Cost Per Charge
        var CostPerCharge = (ValidationHelper.GetDouble(model._hdIncrementOverTotalBase.ToString(), 0) / ValidationHelper.GetDouble(model._hdMonthlyChargesPerMonth.ToString(), 0));
        model._txtCostPercharges = CostPerCharge.ToString("c");
        model._hdCostPercharges = CostPerCharge.ToString("0.00");

        //Cost Per kWh
        var CostPerkWh = (ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0) / ValidationHelper.GetDouble(model._txtkWhCharge.ToString(), 0));
        model._txtCostPerkWh = CostPerkWh.ToString("c");
        model._hdCostPerkWh = CostPerkWh.ToString("0.00");

        #endregion

        #region Indirect Profit

        //Indirect Direct Profit Charge per month 
        model._txtIndChargers = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station
        model._hdIndChargers = model._txtNumberOfCharger.Trim().ToString(); //Number Of Charging Station  

        model._txtIndChargesChargerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charges per Charger per Station
        model._hdIndChargesChargerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString(); //Number Of Charges per Charger per Station

        model._txtDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days
                                               //_hdDaysMonth.Trim().ToString(); //Number of charges per month standard 30 days

        //_txtIndMonthlyChargesPerMonth.Trim().ToString();//Number Charge Per Charger Per Day
        //_txtkWhCharge.Trim().ToString(); //kwh Charge Constant 22.83
        //_txtBaseCStoreEnergyCosts.Trim().ToString();//Base C-Store Energy Costs
        //_txtBaseCStoreDemandCharges.Trim().ToString();//Base C-Store Demand Charges
        //_txtTotalBaseCStoreUtilityCosts.Trim().ToString();//Total Base C-Store Utility Costs
        //_txtEnergyCostkwh = _txtEnergyCostkwh.Trim().ToString();//Energy Cost kwh
        //_txtDemandCostkW.Trim().ToString();//Demand Cost kW
        //_txtCapitalCostOfWHCharger.Trim().ToString();//Capital Cost Of WHC harger
        //_txtHurdleRateROI.Trim().ToString();//Hurdle Rate ROI
        //_txtNumberOfCharger.Trim().ToString();//Number Of Charger

        #region  Conversion Rate Sensitivity Analysis
        //var ChargesperDayperMonth = Convert.ToDouble(_txtChargers.ToString()) * Convert.ToDouble(_txtChargesChargerDay.ToString()) * Convert.ToDouble(_txtDaysMonth.ToString());
        //_txtMonthlyChargesPerMonth = ChargesperDayperMonth.ToString("0"); // Charges per Day per Month
        //_hdMonthlyChargesPerMonth = ChargesperDayperMonth.ToString("0");  // Charges per Day per Month

        //Number of charges per month
        var IndChargesPerMonth = (ValidationHelper.GetDouble(model._txtIndChargers.ToString(), 0) * ValidationHelper.GetDouble(model._txtIndChargesChargerDay.ToString(), 0) * ValidationHelper.GetDouble(model._txtDaysMonth.ToString(), 0));
        model._txtIndMonthlyChargesPerMonth = IndChargesPerMonth.ToString("#,##0");
        model._hdIndMonthlyChargesPerMonth = IndChargesPerMonth.ToString("0");

        //20% Conversion Rate Store Transactions
        var ConversionRateStoreTransactions = (ValidationHelper.GetDouble(IndChargesPerMonth.ToString(), 0) * (Convert.ToDouble(20) / Convert.ToDouble(100)));
        model._txtIndConvRateStoreTrans = ConversionRateStoreTransactions.ToString("0");
        model._hdIndConvRateStoreTrans = ConversionRateStoreTransactions.ToString("0");

        //7.34 Basket Size Inside Sales
        var BasketSizeInsideSales = (ValidationHelper.GetDouble(model._hdIndConvRateStoreTrans.ToString(), 0) * Convert.ToDouble(7.34));
        model._txtIndBasketSizeInSale = BasketSizeInsideSales.ToString("c");
        model._hdIndBasketSizeInSale = BasketSizeInsideSales.ToString("0.00");

        //35% GMT            
        var IndGMT = ValidationHelper.GetDouble(model._txtIndGMT.Trim().ToString(), 0);
        model._txtIndGMT = IndGMT.ToString("0.00");

        //35% Gross Margin Total 
        var GrossMarginTotal = (ValidationHelper.GetDouble(model._hdIndBasketSizeInSale.ToString(), 0) * (Convert.ToDouble(35) / Convert.ToDouble(100)));
        model._txtIndGrossMarginTotal = GrossMarginTotal.ToString("c");
        model._hdIndGrossMarginTotal = GrossMarginTotal.ToString("0.00");

        //40%  Conversion rate Store Transactions
        var CRGPS1 = (ValidationHelper.GetDouble(IndChargesPerMonth.ToString(), 0) * (Convert.ToDouble(40) / Convert.ToDouble(100)) * (Convert.ToDouble(7.34)) * (Convert.ToDouble(35) / Convert.ToDouble(100)));
        model._txtCRGPS1 = CRGPS1.ToString("c");
        model._hdCRGPS1 = CRGPS1.ToString("0.00");

        //60%  Conversion rate Store Transactions
        var CRGPS2 = (ValidationHelper.GetDouble(IndChargesPerMonth.ToString(), 0) * (Convert.ToDouble(60) / Convert.ToDouble(100)) * (Convert.ToDouble(7.34)) * (Convert.ToDouble(35) / Convert.ToDouble(100)));
        model._txtCRGPS2 = CRGPS2.ToString("c");
        model._hdCRGPS2 = CRGPS2.ToString("0.00");

        //80%  Conversion rate Store Transactions
        var CRGPS3 = (ValidationHelper.GetDouble(IndChargesPerMonth.ToString(), 0) * (Convert.ToDouble(80) / Convert.ToDouble(100)) * (Convert.ToDouble(7.34)) * (Convert.ToDouble(35) / Convert.ToDouble(100)));
        model._txtCRGPS3 = CRGPS3.ToString("c");
        model._hdCRGPS3 = CRGPS3.ToString("0.00");
        #endregion

        #region CAP EX MODELING
        //Number of Charger
        model._txtCapChargers = model._txtNumberOfCharger.Trim().ToString();
        model._hdCapChargers = model._txtNumberOfCharger.Trim().ToString();

        model._txtCapChargers = model._txtNumberOfCharger.Trim().ToString();
        model._hdCapChargers = model._txtNumberOfCharger.Trim().ToString();

        //Charges Per Charger Per Day
        model._txtCapChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();
        model._hdCapChargePerChargerPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        //Total Cap-Ex 
        var TotalCapEx = (ValidationHelper.GetDouble(model._txtNumberOfCharger.ToString(), 0) * ValidationHelper.GetDouble(model._txtCapitalCostOfWHCharger.ToString(), 0));
        model._txtCapTotalCapEx = TotalCapEx.ToString("c");
        model._hdCapTotalCapEx = TotalCapEx.ToString("0.00");

        //Hurdle Return
        var HurdleReturn = (ValidationHelper.GetDouble(TotalCapEx.ToString(), 0) * ((ValidationHelper.GetDouble(model._txtHurdleRateROI.ToString(), 0)) / Convert.ToDouble(100)));
        model._txtCapHurdleReturn = HurdleReturn.ToString("c");
        model._hdCapHurdleReturn = HurdleReturn.ToString("0.00");

        //Indirect GP$ Per Year Return
        var IndirectGPSPerYearReturn = (ValidationHelper.GetDouble(GrossMarginTotal.ToString(), 0) * Convert.ToDouble(12));
        model._txtCapIndirectGPSPerYear = IndirectGPSPerYearReturn.ToString("c");
        model._hdCapIndirectGPSPerYear = IndirectGPSPerYearReturn.ToString("0.00");

        //Cap Charge Per Year
        var CapChargePerYear = (ValidationHelper.GetDouble(model._txtNumberOfCharger.ToString(), 0) * ValidationHelper.GetDouble(model._txtNumberChargePerChargerPerDay.ToString(), 0) * Convert.ToDouble(365));
        model._txtCapChargePerYear = CapChargePerYear.ToString("#,##0");
        model._hdCapChargePerYear = CapChargePerYear.ToString("0");

        //Required Charging net $
        var CapRequiredChargingNetS = (ValidationHelper.GetDouble(HurdleReturn.ToString(), 0) - ValidationHelper.GetDouble(IndirectGPSPerYearReturn.ToString(), 0));
        model._txtCapRequiredChargingNetS = CapRequiredChargingNetS.ToString("c");
        model._hdCapRequiredChargingNetS = CapRequiredChargingNetS.ToString("0.00");

        //Required Net $ Per Charge
        var CapRequiredNetSPerCharge = (ValidationHelper.GetDouble(CapRequiredChargingNetS.ToString(), 0)) / (ValidationHelper.GetDouble(CapChargePerYear, 0));
        model._txtCapRequiredNetSPerCharge = CapRequiredNetSPerCharge.ToString("c");
        model._hdCapRequiredNetSPerCharge = CapRequiredNetSPerCharge.ToString("0.00");

        //Required Gross $ Per Charge
        var CapRequiredGrossSPerCharge = (ValidationHelper.GetDouble(CapRequiredNetSPerCharge.ToString(), 0) + ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0));
        model._txtCapRequiredGrossSPerCharge = CapRequiredGrossSPerCharge.ToString("c");
        model._hdCapRequiredGrossSPerCharge = CapRequiredGrossSPerCharge.ToString("0.00");

        //Cost to Consumer Per kWh
        var CostConsumerPerkWh = (ValidationHelper.GetDouble(CapRequiredGrossSPerCharge.ToString(), 0)) / (ValidationHelper.GetDouble(model._txtkWhCharge.ToString(), 0));
        model._txtCostConsumerPerkWh = CostConsumerPerkWh.ToString("c");
        model._hdCostConsumerPerkWh = CostConsumerPerkWh.ToString("0.00");


        ////Required Gross Charge w/o Incremental Demand Charges
        //var CapRequiredGrossChargeWithoutIncrementalDemandCharges = (Convert.ToDouble(CapRequiredNetSPerCharge.ToString()) + ((Convert.ToDouble(model._txtkWhCharge.ToString())) * (Convert.ToDouble(_txtEnergyCostkwh.ToString()))));
        //_txtCapRequiredGrossChargeWithoutIncrementalDemandCharges = CapRequiredGrossChargeWithoutIncrementalDemandCharges.ToString("0.00");

        ////Gross Price Per kWh
        //var CapGrossPricePerkWh = (Convert.ToDouble(CapRequiredGrossChargeWithoutIncrementalDemandCharges.ToString())) / (Convert.ToDouble(_txtkWhCharge.ToString()));
        //_txtCapGrossPricePerkWh = CapGrossPricePerkWh.ToString("0.00");


        #endregion

        #region CONVERSION & BASKET SENSITIVITY ANALYSIS
        //Number of Charger
        model._txtConvChargers = model._txtNumberOfCharger.Trim().ToString();
        model._hdConvChargers = model._txtNumberOfCharger.Trim().ToString();

        //Charges Per Charger Per Day
        model._txtConvChargePerChargersPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();
        model._hdConvChargePerChargersPerDay = model._txtNumberChargePerChargerPerDay.Trim().ToString();

        //20% Required Gross Per Charge 
        var RequiredGrossPerCharge20 = (((ValidationHelper.GetDouble(HurdleReturn.ToString(), 0)) - ((ValidationHelper.GetDouble(GrossMarginTotal.ToString(), 0)) * (Convert.ToDouble(12)))) / (ValidationHelper.GetDouble(CapChargePerYear.ToString(), 0))) + (ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0));
        model._txtConvTwentyRequiredGrossSPerCharge = RequiredGrossPerCharge20.ToString("c");
        model._hdConvTwentyRequiredGrossSPerCharge = RequiredGrossPerCharge20.ToString("0.00");

        //40% Required Gross Per Charge 
        var RequiredGrossPerCharge40 = (((ValidationHelper.GetDouble(HurdleReturn.ToString(), 0)) - ((ValidationHelper.GetDouble(CRGPS1.ToString(), 0)) * (Convert.ToDouble(12)))) / (ValidationHelper.GetDouble(CapChargePerYear.ToString(), 0))) + (ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0));
        model._txtConvFourtyRequiredGrossSPerCharge = RequiredGrossPerCharge40.ToString("c");
        model._hdConvFourtyRequiredGrossSPerCharge = RequiredGrossPerCharge40.ToString("0.00");

        //60% Required Gross Per Charge 
        var RequiredGrossPerCharge60 = (((ValidationHelper.GetDouble(HurdleReturn.ToString(), 0)) - ((ValidationHelper.GetDouble(CRGPS2.ToString(), 0)) * (Convert.ToDouble(12)))) / (ValidationHelper.GetDouble(CapChargePerYear.ToString(), 0))) + (ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0));
        model._txtConvSixtyRequiredGrossSPerCharge = RequiredGrossPerCharge60.ToString("c");
        model._hdConvSixtyRequiredGrossSPerCharge = RequiredGrossPerCharge60.ToString("0.00");

        //80% Required Gross Per Charge 
        var RequiredGrossPerCharge80 = (((ValidationHelper.GetDouble(HurdleReturn.ToString(), 0)) - ((ValidationHelper.GetDouble(CRGPS3.ToString(), 0)) * (Convert.ToDouble(12)))) / (ValidationHelper.GetDouble(CapChargePerYear.ToString(), 0))) + (ValidationHelper.GetDouble(model._hdCostPercharges.ToString(), 0));
        model._txtConvEightyRequiredGrossSPerCharge = RequiredGrossPerCharge80.ToString("c");
        model._hdConvEightyRequiredGrossSPerCharge = RequiredGrossPerCharge80.ToString("0.00");

        ////Basket Required Gross Per Charge 
        //var SOSBasketRequiredGrossPerCharge = (((Convert.ToDouble(HurdleReturn.ToString())) - ((Convert.ToDouble(CRGPS3.ToString())) * (Convert.ToDouble(12)) * (Convert.ToDouble(2)))) / (Convert.ToDouble(365))) + (Convert.ToDouble(_txtCostPercharges.ToString()));
        //_txtSOSBasketRequiredGrossPerCharge = SOSBasketRequiredGrossPerCharge.ToString("0.00");

        ////Gross Price Per kWh With Demand Charge 
        //var SOSGrossPricePerkWhWithDemandCharge = (Convert.ToDouble(SOSBasketRequiredGrossPerCharge.ToString())) / (Convert.ToDouble(_txtkWhCharge.ToString()));
        //_txtSOSGrossPricePerkWhWithDemandCharge = SOSGrossPricePerkWhWithDemandCharge.ToString("0.00");

        ////Gross Price Per kWh W/O Demand Charge 
        //var SOSGrossPricePerkWhWithOutDemandCharge = ((((Convert.ToDouble(HurdleReturn.ToString())) - ((Convert.ToDouble(CRGPS3.ToString())) * (Convert.ToDouble(12)) * (Convert.ToDouble(2)))) / (Convert.ToDouble(365))) + ((Convert.ToDouble(_txtkWhCharge.ToString())) * (Convert.ToDouble(_txtEnergyCostkwh.ToString())))) / (Convert.ToDouble(_txtkWhCharge.ToString()));
        //_txtSOSGrossPricePerkWhWithOutDemandCharge = SOSGrossPricePerkWhWithOutDemandCharge.ToString("0.00");

        #endregion

        #endregion

    }

}
