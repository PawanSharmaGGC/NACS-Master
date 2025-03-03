using System.Collections.Generic;

namespace Convenience.org.Models;

public class EVChargingCalculatorViewModel
{
    public int CurrentStep { get; set; } = 1;
    public List<string> Steps { get; set; } = new List<string> { "Data", "Direct Profit", "Indirect Profit" };

    public bool Step1Completed { get; set; }
    public bool Step2Completed { get; set; }
    public bool Step3Completed { get; set; }


    // Direct Profit Properties
    public string _txtNumberOfCharger { get; set; } = "1";// Number of Charging Stations
    public string Text0 { get; set; } = "";

    public string _txtNumberChargePerChargerPerDay { get; set; } = "1"; // Number of Charges per Day per Charger
    public string Text1 { get; set; } = "";

    public string _txtDaysMonth { get; set; } = "30"; // Days in Month (usually 30)
    public string Text2 { get; set; } = "";

    public string _txtkWCharger { get; set; } = "150"; // kW per Charger
    public string Text3 { get; set; } = "";

    public string _txtBaseCStoreEnergyCosts { get; set; } = "3000"; // Base C-Store Energy Costs
    public string Text4 { get; set; } = "";

    public string _txtBaseCStoreDemandCharges { get; set; } = "927.20"; // Base C-Store Demand Charges
    public string Text5 { get; set; } = "";

    public string _txtTotalBaseCStoreUtilityCosts { get; set; } = "3927.20";// Total Base C-Store Utility Costs
    public string Text6 { get; set; } = "";

    public string _txtEnergyCostkwh { get; set; } = "0.06";// Energy Cost per kWh
    public string Text7 { get; set; } = "";

    public string _txtDemandCostkW { get; set; } = "11.59"; // Demand Cost per kW
    public string Text8 { get; set; } = "";

    public string _txtCapitalCostOfWHCharger { get; set; } = "150000"; // Capital Cost of WH Charger
    public string Text9 { get; set; } = "";

    public string _txtHurdleRateROI { get; set; } = "15";// Hurdle Rate ROI
    public string Text10 { get; set; } = "";

    public string Text11 { get; set; } = "";
    public string Text12 { get; set; } = "";

    public string _txtEnergyCharges { get; set; } = ""; // Energy Charges
    public string Text13 { get; set; } = "";

    public string _txtTotalDemandkW { get; set; } = ""; // Total Demand kW
    public string Text14 { get; set; } = "";

    public string _txtkWhCharge { get; set; } = "22.83"; // kWh Charge Constant
    public string Text15 { get; set; } = "";

    public string _txtBasekW { get; set; } = "80"; // Number of kW per Charging Station
    public string Text16 { get; set; } = "";

    public string _txtCostPercharges { get; set; } = ""; // Cost per Charge
    public string Text17 { get; set; } = "";
    public string Text18 { get; set; } = "";

    // Indirect Profit Properties
    public string Text19 { get; set; } = "";
    public string Text20 { get; set; } = "";
    public string Text21 { get; set; } = "";
    public string Text22 { get; set; } = "";
    public string Text23 { get; set; } = "";
    public string Text24 { get; set; } = "";
    public string Text25 { get; set; } = "";
    public string Text26 { get; set; } = "";
    public string Text27 { get; set; } = "";
    public string Text28 { get; set; } = "";
    public string Text29 { get; set; } = "";
    public string Text30 { get; set; } = "";
    public string Text31 { get; set; } = "";
    public string Text32 { get; set; } = "";
    public string Text33 { get; set; } = "";
    public string Text34 { get; set; } = "";
    public string Text35 { get; set; } = "";
    public string Text36 { get; set; } = "";
    public string Text37 { get; set; } = "";
    public string Text38 { get; set; } = "";
    public string Text39 { get; set; } = "";
    public string Text40 { get; set; } = "";
    public string Text41 { get; set; } = "";
    public string Text42 { get; set; } = "";
    public string Text43 { get; set; } = "";
    public string Text44 { get; set; } = "";

    //-------------

    public string _txtNumberofChargingStations { get; set; } = "";
    public string _txtNumberofChargesperStationsperDay { get; set; } = "";

    public string _hdTotalBaseCStoreUtilityCosts { get; set; } = "3927.20";
    public string _hdEnergyCostkwh { get; set; } = "";
    public string _txtNumberOfChargers { get; set; } = "";
    public string _txtDemandkWCharger { get; set; } = "";
    public string _hdDemandkWCharger { get; set; } = "";
    public string _txtChargespermonth { get; set; } = "";
    public string _hdChargespermonth { get; set; } = "";

    public string _txtIncrementalMonthlykWh { get; set; } = "";
    public string _hdIncrementalMonthlykWh { get; set; } = "";

    public string _txtBaseMonthlykWh { get; set; } = "";
    public string _hdBaseMonthlykWh { get; set; } = "";

    public string _txtTotalMonthlykWh { get; set; } = "";
    public string _hdTotalMonthlykWh { get; set; } = "";

    public string _txtTotalEnergyCharge { get; set; } = "";
    public string _hdTotalEnergyCharge { get; set; } = "";

    public string _txtDemandkW { get; set; } = "";
    public string _hdDemandkW { get; set; } = "";

    public string _hdTotalDemandkW { get; set; } = "";

    public string _txtTotalDemandCharge { get; set; } = "";
    public string _hdTotalDemandCharge { get; set; } = "";

    public string _txtMonthlyChargesPerMonth { get; set; } = "";
    public string _hdMonthlyChargesPerMonth { get; set; } = "";

    public string _hdEnergyCharges { get; set; } = "";

    public string _txtDemandCharges { get; set; } = "";
    public string _hdDemandCharges { get; set; } = "";

    public string _txtTotalCharges { get; set; } = "";
    public string _hdTotalCharges { get; set; } = "";

    public string _txtIncrementOverTotalBase { get; set; } = "";
    public string _hdIncrementOverTotalBase { get; set; } = "";

    public string _hdCostPercharges { get; set; } = "";

    public string _txtCostPerkWh { get; set; } = "";
    public string _hdCostPerkWh { get; set; } = "";

    public string _txtIndChargers { get; set; } = "";
    public string _hdIndChargers { get; set; } = "";

    public string _txtIndChargesChargerDay { get; set; } = "";
    public string _hdIndChargesChargerDay { get; set; } = "";

    public string _txtIndMonthlyChargesPerMonth { get; set; } = "";
    public string _hdIndMonthlyChargesPerMonth { get; set; } = "";

    public string _txtIndConvRateStoreTrans { get; set; } = "";
    public string _hdIndConvRateStoreTrans { get; set; } = "";

    public string _txtIndBasketSizeInSale { get; set; } = "";
    public string _hdIndBasketSizeInSale { get; set; } = "";

    public string _txtIndGMT { get; set; } = "35.00";
    public string _txtIndGrossMarginTotal { get; set; } = "";
    public string _hdIndGrossMarginTotal { get; set; } = "";

    public string _txtCRGPS1 { get; set; } = "";
    public string _hdCRGPS1 { get; set; } = "";

    public string _txtCRGPS2 { get; set; } = "";
    public string _hdCRGPS2 { get; set; } = "";

    public string _txtCRGPS3 { get; set; } = "";
    public string _hdCRGPS3 { get; set; } = "";

    public string _txtCapChargers { get; set; } = "";
    public string _hdCapChargers { get; set; } = "";

    public string _txtCapChargePerChargerPerDay { get; set; } = "";
    public string _hdCapChargePerChargerPerDay { get; set; } = "";

    public string _txtCapTotalCapEx { get; set; } = "";
    public string _hdCapTotalCapEx { get; set; } = "";

    public string _txtCapHurdleReturn { get; set; } = "";
    public string _hdCapHurdleReturn { get; set; } = "";

    public string _txtCapIndirectGPSPerYear { get; set; } = "";
    public string _hdCapIndirectGPSPerYear { get; set; } = "";

    public string _txtCapChargePerYear { get; set; } = "";
    public string _hdCapChargePerYear { get; set; } = "";

    public string _txtCapRequiredChargingNetS { get; set; } = "";
    public string _hdCapRequiredChargingNetS { get; set; } = "";

    public string _txtCapRequiredNetSPerCharge { get; set; } = "";
    public string _hdCapRequiredNetSPerCharge { get; set; } = "";

    public string _txtCapRequiredGrossSPerCharge { get; set; } = "";
    public string _hdCapRequiredGrossSPerCharge { get; set; } = "";

    public string _txtCostConsumerPerkWh { get; set; } = "";
    public string _hdCostConsumerPerkWh { get; set; } = "";

    public string _txtConvChargers { get; set; } = "";
    public string _hdConvChargers { get; set; } = "";

    public string _txtConvChargePerChargersPerDay { get; set; } = "";
    public string _hdConvChargePerChargersPerDay { get; set; } = "";

    public string _txtConvTwentyRequiredGrossSPerCharge { get; set; } = "0";//
    public string _hdConvTwentyRequiredGrossSPerCharge { get; set; } = "";

    public string _txtConvFourtyRequiredGrossSPerCharge { get; set; } = "0";//
    public string _hdConvFourtyRequiredGrossSPerCharge { get; set; } = "";

    public string _txtConvSixtyRequiredGrossSPerCharge { get; set; } = "0";//
    public string _hdConvSixtyRequiredGrossSPerCharge { get; set; } = "";

    public string _txtConvEightyRequiredGrossSPerCharge { get; set; } = "0";//
    public string _hdConvEightyRequiredGrossSPerCharge { get; set; } = "";


    public string _txtDemandBasekW { get; set; } = "";
    public string _hdDemandBasekW { get; set; } = "";
    public string _txtChargers { get; set; } = "";
    public string _hdChargers { get; set; } = "";
    public string _txtChargesChargerDay { get; set; } = "";
    public string _hdChargesChargerDay { get; set; } = "";
    //public string _txtConvFourtyRequiredGrossSPerCharge { get; set; }
    //public string _hdConvFourtyRequiredGrossSPerCharge { get; set; }

    public string _hdNumberofChargingStations { get; set; } = "";
    public string _hdNumberofChargesperStationsperDay { get; set; } = "";
    public string _hdNumberOfChargers { get; set; } = "";
    public string _txtIndbsis { get; set; } = "7.34";

}
