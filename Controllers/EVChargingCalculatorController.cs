using Convenience.org.Helpers;
using Convenience.org.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.Linq;
using iText.Html2pdf;
using System.Threading;
using CMS.Core;

namespace Convenience.org.Controllers;

public class EVChargingCalculatorController : Controller
{
	private readonly IEventLogService logger;

	public EVChargingCalculatorController(IEventLogService logger)
	{
		this.logger = logger;
	}
	private EVChargingCalculatorViewModel GetEVChargingCalculatorDetail()
	{
		// Retrieve the model from session or create a new one if it doesn't exist
		var model = HttpContext.Session.GetObject<EVChargingCalculatorViewModel>("EVChargingCalculatorData");
		if (model == null)
		{
			model = new EVChargingCalculatorViewModel();  // Create a new instance if no session data exists
		}
		return model;
	}

	private void SaveEVChargingCalculatorDetail(EVChargingCalculatorViewModel model)
	{
		// Save the current evchargingcalculator state into session
		HttpContext.Session.SetObject("EVChargingCalculatorData", model);
	}

	public IActionResult Index()
	{
		var model = GetEVChargingCalculatorDetail();
		return View();
	}

	[HttpPost]
	public IActionResult NextStep(int step, EVChargingCalculatorViewModel model)
	{
		if (step <= 3)
		{
			SetStepsClasses(model, step);
			return ViewComponent("EVChargingCalculatorWidget", new { step, model });
		}
		return View(model);
	}

	[HttpPost]
	public IActionResult PreviousStep(int step, EVChargingCalculatorViewModel model)
	{
		if (step > 0)
		{
			SetStepsClasses(model, step);
			return ViewComponent("EVChargingCalculatorWidget", new { step, model });
		}
		return View(model);
	}

	public void ReplaceNullPropertiesWithEmptyString(EVChargingCalculatorViewModel model)
	{
		if (model == null) return;

		model.GetType()
			 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
			 .Where(p => p.PropertyType == typeof(string) && p.GetValue(model) == null) // Filter string properties with null values
			 .ToList() // Materialize the IEnumerable to operate on it
			 .ForEach(p => p.SetValue(model, "")); // Set null properties to empty string
	}

	private EVChargingCalculatorViewModel SetStepsClasses(EVChargingCalculatorViewModel model, int step)
	{
		ReplaceNullPropertiesWithEmptyString(model);

		model.CurrentStep = step;


		model.Step1Completed = model.CurrentStep >= 1 ? true : false;
		model.Step2Completed = model.CurrentStep >= 2 ? true : false; ;
		model.Step3Completed = model.CurrentStep >= 3 ? true : false; ;

		// Save the current step data to session
		SaveEVChargingCalculatorDetail(model);

		return model;
	}

	public IActionResult UpdateModelData(EVChargingCalculatorViewModel model)
	{
		try
		{
			SaveEVChargingCalculatorDetail(model);
			return Ok();
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
		
	}

	public IActionResult GenerateInvoicePDF(object sender, EventArgs e)
	{
		try
		{
			var model = HttpContext.Session.GetObject<EVChargingCalculatorViewModel>("EVChargingCalculatorData");

			if (model != null)
			{

				CultureInfo us = new CultureInfo("en-US");
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				CultureInfo newCulture = new CultureInfo(currentCulture.Name);
				newCulture.NumberFormat.CurrencyNegativePattern = 1;
				Thread.CurrentThread.CurrentCulture = newCulture;
				NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

				var txtBaseCStoreEnergyCosts = Convert.ToDouble(model._txtBaseCStoreEnergyCosts.ToString());
				string BaseCStoreEnergyCosts = txtBaseCStoreEnergyCosts.ToString("c");

				var txtBaseCStoreDemandCharges = Convert.ToDouble(model._txtBaseCStoreDemandCharges.ToString());
				string BaseCStoreDemandCharges = txtBaseCStoreDemandCharges.ToString("c");

				var txtTotalBaseCStoreUtilityCosts = Convert.ToDouble(model._txtTotalBaseCStoreUtilityCosts.ToString());
				string TotalBaseCStoreUtilityCosts = txtTotalBaseCStoreUtilityCosts.ToString("c");

				var txtDemandCostkW = Convert.ToDouble(model._txtDemandCostkW.ToString());
				string DemandCostkW = txtDemandCostkW.ToString("c");

				var txtCapitalCostOfWHCharger = Convert.ToDouble(model._txtCapitalCostOfWHCharger.ToString());
				string CapitalCostOfWHCharger = txtCapitalCostOfWHCharger.ToString("c");

				var txtEnergyCostkwh = Convert.ToDouble(model._txtEnergyCostkwh.ToString());
				string EnergyCostkwh = txtEnergyCostkwh.ToString("c");

				string url = "https://www.convenience.org/PublishingImages/NACS-Logo-Blue.png";

				// Build HTML string for the PDF
				StringBuilder sb = new StringBuilder();

				//PDF Header & Logo
				#region Header & Logo
				sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
				sb.Append("<tr><td>");
				sb.Append("<td></td><td></td>" + "<img style='float: right;' src='" + url + "' />");
				sb.Append("</td></tr>");
				sb.Append("</table>");

				sb.Append("<span style='color: #727f8a; font-size:8px;'>" + DateTime.Now + "</span>");
				sb.Append("<br />");
				sb.Append("<br />");

				sb.Append("<h1 style='color:#0d2c6c; font-weight:700;'>EV Charging Business Modeling</h1>");

				sb.Append("<br />");
				sb.Append("<br />");
				#endregion

				//PDF Body
				#region Body

				//Provided Data
				#region Provided Data
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width: 100%'> ");

				sb.Append("<tr>");
				sb.Append("<td colspan='2' bgcolor='#005f9e' style='color:#fff; font-size:15px; font-weight:700; border: 1px solid #000; width:100%'>");
				sb.Append("Provided Data".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Base C-Store Energy Costs: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + BaseCStoreEnergyCosts.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtNumberOfCharger.ToString() + "</span>");
				sb.Append("</td>");


				sb.Append("</tr><tr>");


				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Base C-Store Demand Charges:</span>" +
					"&nbsp; " +
					"<span style='color: #727f8a; font-size:12px;'>" + BaseCStoreDemandCharges.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span>" +
					"&nbsp; &nbsp;" +
				  " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtNumberChargePerChargerPerDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Base C-Store Utility Costs:</span>" +
					"&nbsp; " +
					"<span style='color: #727f8a; font-size:12px;'>" + TotalBaseCStoreUtilityCosts.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Hurdle Rate ROI: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtHurdleRateROI.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Base kW:  </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtBasekW.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>kW Output/Charger: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtkWCharger.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Demand Cost/kW: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + DemandCostkW.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Capital Cost of Each Charger: </span>" +
					"&nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + CapitalCostOfWHCharger.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Energy Cost/kWh:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + EnergyCostkwh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>kWh/Charge:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtkWhCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr>");
				sb.Append("</table>");
				#endregion

				//Direct Profit Modeling
				#region Direct Profit Modeling
				sb.Append("<br />");
				sb.Append("<br />");
				sb.Append("<h2 style='color:#0d2c6c; font-weight:700;'>Direct Profit Modeling</h2>");
				sb.Append("<br />");

				//Kilowatt and Demand Charge
				#region Kilowatt and Demand Charge
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width:100%'>");

				sb.Append("<tr>");
				sb.Append("<td bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("Kilowatt Hour Charges".ToString());
				sb.Append("</td>");
				sb.Append("<td  bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("Demand Charge".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtNumberofChargingStations.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtNumberOfChargers.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span>" +
					"&nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtNumberofChargesperStationsperDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>kW/Charger:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				  " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtDemandkWCharger.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges/Month:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtChargespermonth.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Demand kW for Chargers: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtDemandkW.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Incremental Monthly kWh Cost: </span>" +
					"&nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtIncrementalMonthlykWh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Base Demand kW Before Chargers: </span>" +
					" " +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtDemandBasekW.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Base Monthly kWh Cost: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtBaseMonthlykWh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Demand kW: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtTotalDemandkW.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Monthly kWh Cost: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtTotalMonthlykWh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Demand Charge:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtTotalDemandCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Kilowatt per Hour Cost:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtTotalEnergyCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td border-bottom='none'>");
				//sb.Append("<span style='font-size:12px;'>Kilowatt per Hour Cost:</span>" +
				//    "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				//    "<span style='color: #727f8a; font-size:12px;'>" + _txtkWhCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr>");
				sb.Append("</table>");
				#endregion

				sb.Append("<br />");

				//Total Utility Cost
				#region Total Utility Cost
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width:100%'>");

				sb.Append("<tr>");
				sb.Append("<td colspan='2' bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000;  font-weight:700;'>");
				sb.Append("Total Utility Cost".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtChargers.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Cost of Kilowatt Hours Delivered: </span>" +
					"&nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtEnergyCharges.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtChargesChargerDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Demand Charge Costs:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp;" +
				  " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtDemandCharges.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Charges Per Month <span style='font-size:8px;'>(30 day month):</span></span>" +
					"&nbsp; " +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtMonthlyChargesPerMonth.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Costs: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				  " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtTotalCharges.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Cost per Charge: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCostPercharges.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Increment Over Total Base: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtIncrementOverTotalBase.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Cost per kWh:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCostPerkWh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("</td>");

				sb.Append("</tr>");
				sb.Append("</table>");
				#endregion
				#endregion

				//Indirect and Total Profit Modeling
				#region Indirect and Total Profit Modeling
				sb.Append("<br />");
				sb.Append("<br />");
				sb.Append("<br />");
				sb.Append("<h2 style='color:#0d2c6c; font-weight:700;'>Indirect and Total Profit Modeling</h2>");
				sb.Append("<br />");
				sb.Append("<h3 style='color:#60bc50; font-weight:700;'>In-Store Profitability</h3>");
				sb.Append("<br />");

				//Conversion Rate – Monthly Data
				#region Conversion Rate – Monthly Data
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width:100%'>");

				sb.Append("<tr>");
				sb.Append("<td colspan='3' bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("Conversion Rate – Monthly Data".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");
				sb.Append("<td>");
				sb.Append("</td>");

				sb.Append("<td  bgcolor='#60bc50' align='center' style='color:#fff; font-size:12px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("<span style='font-size:12px;'>20% Conversion Rate</span>");
				sb.Append("</td>");

				sb.Append("<td  bgcolor='#60bc50' align='center' style='color:#fff; font-size:12px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("<span style='font-size:12px;'>Conversion Rate Sensitivity Analysis</span>");
				sb.Append("</td>");

				sb.Append("</tr>");


				sb.Append("<tr>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers: </span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtIndChargers.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>Store Transactions: </span> <br />" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtIndConvRateStoreTrans.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>40% Conversion Rate Total GP$:</span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtCRGPS1.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span> <br />" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtIndChargesChargerDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='color: #727f8a; font-size:12px;'>" + model._txtIndbsis.ToString() + "</span> of <br />" +
					"<span style='font-size:12px;'>Basket Size Inside Sales :</span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtIndBasketSizeInSale.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>60% Conversion Rate Total GP$: </span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtCRGPS2.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>Total Charges Per Month <br /> <span style='font-size:8px;'>(30 day month):</span></span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtIndMonthlyChargesPerMonth.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='color: #727f8a; font-size:12px;'>" + model._txtIndGMT.ToString() + " %" + "</span> of <br />" +
					"<span style='font-size:12px;'>In-Store Gross Margin: </span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtIndGrossMarginTotal.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>80% Conversion Rate Total GP$: </span> <br />" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtCRGPS3.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr>");

				sb.Append("</table>");
				#endregion

				sb.Append("<br />");

				//CAP EX Modeling
				#region CAP EX Modeling
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width:100%'>");

				sb.Append("<tr>");
				sb.Append("<td colspan='2' bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("CAP EX Modeling".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapChargers.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Total Cap-Ex: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapTotalCapEx.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				  " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapChargePerChargerPerDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Hurdle Return $: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtCapHurdleReturn.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Year: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapChargePerYear.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Indirect GP$ per Year: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapIndirectGPSPerYear.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Required Return Net of Indirect GP$:</span>" +
					" " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapRequiredChargingNetS.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Required Net Return per Charge:</span>" +
					"&nbsp; " +
					"<span style='color: #727f8a; font-size:12px;'>" + model._txtCapRequiredNetSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Required Charge to Consumer:</span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCapRequiredGrossSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Cost to Consumer per kWh: </span>" +
					"&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtCostConsumerPerkWh.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr>");
				sb.Append("</table>");
				#endregion

				sb.Append("<br />");
				sb.Append("<br />");
				sb.Append("<h3 style='color:#60bc50; font-weight:700;'>Total Profitability / Req'd Gross $</h3>");
				sb.Append("<br />");

				//Conversion & Basket Sensitivity Analysis
				#region Conversion & Basket Sensitivity Analysis
				sb.Append("<table border='1' style='border-collapse: collapse; border-spacing: 0; width:100%'>");

				sb.Append("<tr>");
				sb.Append("<td colspan='2' bgcolor='#005f9e' style='color:#fff; font-size:15px; border: 1px solid #000; font-weight:700;'>");
				sb.Append("Conversion & Basket Sensitivity Analysis".ToString());
				sb.Append("</td>");
				sb.Append("</tr>");

				sb.Append("<tr>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Number of Chargers:</span>" +
				   "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvChargers.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td>");
				sb.Append("<span style='font-size:12px;'>Charges per Charger per Day:</span>" +
				   "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;" +
				   " " + "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvChargePerChargersPerDay.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>20% Conversion: Req’d Consumer $/Charge: </span>" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvTwentyRequiredGrossSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>40% Conversion: Req’d Consumer $/Charge: </span>" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvFourtyRequiredGrossSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr><tr>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>60% Conversion: Req’d Consumer $/Charge: </span>" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvSixtyRequiredGrossSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("<td align='center'>");
				sb.Append("<span style='font-size:12px;'>80% Conversion: Req’d Consumer $/Charge: </span>" +
				   "<span style='color: #727f8a; font-size:12px;'>" + model._txtConvEightyRequiredGrossSPerCharge.ToString() + "</span>");
				sb.Append("</td>");

				sb.Append("</tr>");
				sb.Append("</table>");
				#endregion
				#endregion

				#endregion

				byte[] pdfBytes;

				using (MemoryStream ms = new MemoryStream())
				{
					// Convert HTML content to PDF directly using the HtmlConverter
					HtmlConverter.ConvertToPdf(sb.ToString(), ms);
					pdfBytes = ms.ToArray();
				}

				return File(pdfBytes, "application/pdf", "EV_Charging_Business_Modeling_Report.pdf");

			}
		}
		catch (Exception ex)
		{
			logger.LogException(nameof(EVChargingCalculatorController), nameof(GenerateInvoicePDF), ex);
		}
		return Ok();
	}
}
