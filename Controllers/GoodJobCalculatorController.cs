using Convenience.org.Helpers;
using Convenience.org.Models;
using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using CMS.Core;
using Convenience.org.Components.Widgets.GoodJobCalculator;

namespace Convenience.org.Controllers;

public class GoodJobCalculatorController : Controller
{
    private readonly IConfiguration configuration;
    private readonly IEventLogService logger;

    public GoodJobCalculatorController(IConfiguration configuration, IEventLogService logger)
    {
        this.configuration = configuration;
        this.logger = logger;
    }

    private GoodJobCalculatorViewModel GetGoodJobCalculatorDetail()
    {
        // Retrieve the model from session or create a new one if it doesn't exist
        var model = HttpContext.Session.GetObject<GoodJobCalculatorViewModel>("GoodJobCalculatorData");
        if (model == null)
        {
            model = new GoodJobCalculatorViewModel();
        }
        return model;
    }

    private void SaveGoodJobCalculatorDetail(GoodJobCalculatorViewModel model)
    {
        // Save the current goodjobcalculator state into session
        HttpContext.Session.SetObject("GoodJobCalculatorData", model);
    }

    public IActionResult Index()
    {
        var model = GetGoodJobCalculatorDetail();
        return View();
    }

    [HttpPost]
    public IActionResult NextStep(int step, GoodJobCalculatorViewModel model)
    {
        try
        {
            var widgetProperties = new GoodJobCalculatorProperties();
            if (step <= 4)
            {
                SetStepsClasses(model, step);
                return ViewComponent("GoodJobCalculatorWidget", new { step, model, widgetProperties });
            }
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(NextStep), ex);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult PreviousStep(int step, GoodJobCalculatorViewModel model)
    {
        try
        {
            var widgetProperties = new GoodJobCalculatorProperties();
            if (step > 0)
            {
                SetStepsClasses(model, step);
                return ViewComponent("GoodJobCalculatorWidget", new { step, model, widgetProperties });
            }
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(PreviousStep), ex);
        }

        return View(model);
    }

    private GoodJobCalculatorViewModel SetStepsClasses(GoodJobCalculatorViewModel model, int step)
    {
        model.CurrentStep = step;


        model.Step1Completed = model.CurrentStep >= 1 ? true : false;
        model.Step2Completed = model.CurrentStep >= 2 ? true : false; ;
        model.Step3Completed = model.CurrentStep >= 3 ? true : false; ;
        model.Step4Completed = model.CurrentStep >= 4 ? true : false; ;

        // Save the current step data to session
        SaveGoodJobCalculatorDetail(model);

        return model;
    }

    #region "Export to excel"

    public IActionResult ExportCalculatedData()
    {
        try
        {
            var model = HttpContext.Session.GetObject<GoodJobCalculatorViewModel>("GoodJobCalculatorData");

            if (model != null)
            {
                DataTable dt = CreateDataTable(model);

                // Convert GUID columns to string if needed
                DataTable dtnew = ConvertGuidColumnsToString(dt);

                // Create the Excel file
                var workbook = GenerateExcelFile(dtnew);

                // Generate file name with the current date
                string fileName = "TheGoodJobsCalculatorData_" + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx";


                // Save the Excel file to a memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.Save(stream, SaveOptions.XlsxDefault);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(ExportCalculatedData), ex);
        }
        return Ok();
    }

    // Method to create DataTable (from your original code)
    private DataTable CreateDataTable(GoodJobCalculatorViewModel model)
    {
        DataTable dt = new DataTable();

        try
        {

            #region Data
            //Revenue
            dt.Columns.Add("Number Of Stores", typeof(string));
            dt.Columns["Number Of Stores"].Caption = "Number Of Stores";

            dt.Columns.Add("Total annual inside sales", typeof(string));
            dt.Columns["Total annual inside sales"].Caption = "Total annual inside sales";

            dt.Columns.Add("Total number of inside transactions per store per day", typeof(string));
            dt.Columns["Total number of inside transactions per store per day"].Caption = "Total number of inside transactions per store per day";

            dt.Columns.Add("Inside net margin", typeof(string));
            dt.Columns["Inside net margin"].Caption = "Number Of Stores";

            dt.Columns.Add("Average number of gallons sold per fuel transaction", typeof(string));
            dt.Columns["Average number of gallons sold per fuel transaction"].Caption = "Average number of gallons sold per fuel transaction";

            dt.Columns.Add("Average cent per gallon fuel gross margin (e.g., $0.38 per gallon)", typeof(string));
            dt.Columns["Average cent per gallon fuel gross margin (e.g., $0.38 per gallon)"].Caption = "Average cent per gallon fuel gross margin (e.g., $0.38 per gallon)";

            //Cost
            dt.Columns.Add("Annual cost of goods sold for inside (merchandise and foodservice) per store", typeof(string));
            dt.Columns["Annual cost of goods sold for inside (merchandise and foodservice) per store"].Caption = "Annual cost of goods sold for inside (merchandise and foodservice) per store";

            dt.Columns.Add("Number of employees at the store-level Full-time employees", typeof(string));
            dt.Columns["Number of employees at the store-level Full-time employees"].Caption = "Number of employees at the store-level Full-time employees";

            dt.Columns.Add("Number of employees at the store-level Part-time employees", typeof(string));
            dt.Columns["Number of employees at the store-level Part-time employees"].Caption = "Number of employees at the store-level Part-time employees";

            dt.Columns.Add("Cost of turnover for one store-level employee Full-time employee", typeof(string));
            dt.Columns["Cost of turnover for one store-level employee Full-time employee"].Caption = "Cost of turnover for one store-level employee Full-time employee";

            dt.Columns.Add("Cost of turnover for one store-level employee Part-time employee", typeof(string));
            dt.Columns["Cost of turnover for one store-level employee Part-time employee"].Caption = "Cost of turnover for one store-level employee Part-time employee";

            dt.Columns.Add("Annual hourly employee turnover rate", typeof(string));
            dt.Columns["Annual hourly employee turnover rate"].Caption = "Annual hourly employee turnover rate";

            dt.Columns.Add("Annual merchandise shrink, as a % of inside sales", typeof(string));
            dt.Columns["Annual merchandise shrink, as a % of inside sales"].Caption = "Annual merchandise shrink, as a % of inside sales";

            dt.Columns.Add("Average hourly store employee wage", typeof(string));
            dt.Columns["Average hourly store employee wage"].Caption = "Average hourly store employee wage";

            dt.Columns.Add("Average hourly store employee overtime wage", typeof(string));
            dt.Columns["Average hourly store employee overtime wage"].Caption = "Average hourly store employee overtime wage";

            dt.Columns.Add("Average number of unplanned overtime hours Full-time employee", typeof(string));
            dt.Columns["Average number of unplanned overtime hours Full-time employee"].Caption = "Average number of unplanned overtime hours Full-time employee";

            dt.Columns.Add("Average number of unplanned overtime hours Part-time employee", typeof(string));
            dt.Columns["Average number of unplanned overtime hours Part-time employee"].Caption = "Average number of unplanned overtime hours Part-time employee";

            //Levers & Results
            //Inside sales lift
            dt.Columns.Add("Inside sales lift - Ticket size", typeof(string));
            dt.Columns["Inside sales lift - Ticket size"].Caption = "Inside sales lift - Ticket size";

            dt.Columns.Add("Inside sales lift - Number of transactions per store per day", typeof(string));
            dt.Columns["Inside sales lift - Number of transactions per store per day"].Caption = "Inside sales lift - Number of transactions per store per day";

            //COST MITIGATION
            dt.Columns.Add("Cost Mittigation - Turnover Rate", typeof(string));
            dt.Columns["Cost Mittigation - Turnover Rate"].Caption = "Cost Mittigation - Turnover Rate";

            dt.Columns.Add("Cost Mittigation - Shrink", typeof(string));
            dt.Columns["Cost Mittigation - Shrink"].Caption = "Cost Mittigation - Shrink";

            dt.Columns.Add("Cost Mittigation - Overtime", typeof(string));
            dt.Columns["Cost Mittigation - Overtime"].Caption = "Cost Mittigation - Overtime";

            dt.Columns.Add("How much of the gross profit uplift would you like to put toward wages", typeof(string));
            dt.Columns["How much of the gross profit uplift would you like to put toward wages"].Caption = "How much of the gross profit uplift would you like to put toward wages";

            dt.Columns.Add("Cost Mittigation - Total number of employees included in hourly wage increase", typeof(string));
            dt.Columns["Cost Mittigation - Total number of employees included in hourly wage increase"].Caption = "Cost Mittigation - Total number of employees included in hourly wage increase";

            //Impact?
            dt.Columns.Add("Total estimated revenue uplift", typeof(string));
            dt.Columns["Total estimated revenue uplift"].Caption = "Total estimated revenue uplift";

            dt.Columns.Add("A - Gross profit uplift", typeof(string));
            dt.Columns["A - Gross profit uplift"].Caption = "A - Gross profit uplift";

            dt.Columns.Add("B - Total estimated cost reduction", typeof(string));
            dt.Columns["B - Total estimated cost reduction"].Caption = "B - Total estimated cost reduction";

            dt.Columns.Add("C - Estimated increase in fuel profitability", typeof(string));
            dt.Columns["C - Estimated increase in fuel profitability"].Caption = "C - Estimated increase in fuel profitability";

            dt.Columns.Add("Total P&L impact (A+B+C)", typeof(string));
            dt.Columns["Total P&L impact (A+B+C)"].Caption = "Total P&L impact (A+B+C)";

            dt.Columns.Add("If P&L impact were translated directly to wages", typeof(string));
            dt.Columns["If P&L impact were translated directly to wages"].Caption = "If P&L impact were translated directly to wages";

            dt.Columns.Add("P&L impact as % of current inside sales net income", typeof(string));
            dt.Columns["P&L impact as % of current inside sales net income"].Caption = "P&L impact as % of current inside sales net income";

            dt.Rows.Add(model.Field0R.ToString(), model.Field1R.ToString(), model.Field3R.ToString(), model.Field4R.ToString(), model.Field5R.ToString(), model.Field6R.ToString(),
            model.Field0C.ToString(), model.Field2C.ToString(), model.Field3C.ToString(), model.Field4C.ToString(), model.Field5C.ToString(), model.Field6C.ToString(), model.Field7C.ToString(), model.Field11C.ToString(), model.Field8C.ToString(), model.Field9C.ToString(), model.Field10C.ToString(),
            model.CaField1.ToString(), model.CaField3.ToString(), model.CaField7.ToString().ToString(), model.CaField9.ToString().ToString(), model.CaField11.ToString().ToString(), model.CaField14.ToString(), model.CaField16.ToString(),
                model.ReField0.ToString(), model.ReField1.ToString(), model.ReField2.ToString(), model.ReField3.ToString(), model.ReField4.ToString(), model.ReField5.ToString(), model.ReField6.ToString());
            #endregion
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(CreateDataTable), ex);
        }
        return dt;
    }

    // Method to convert GUID columns to string if needed
    private DataTable ConvertGuidColumnsToString(DataTable dt)
    {
        try
        {
            DataTable dtnew = dt.Clone();

            foreach (DataColumn c in dtnew.Columns)
            {
                if (c.DataType == typeof(Guid))
                {
                    c.DataType = typeof(string);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                dtnew.ImportRow(row);
            }

            return dtnew;
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(ConvertGuidColumnsToString), ex);
        }
        return dt;
    }

    // Method to generate Excel file from DataTable
    private ExcelFile GenerateExcelFile(DataTable dt)
    {
        try
        {
            var gemboxLicense = configuration["GemboxLicense"] ?? string.Empty;
            SpreadsheetInfo.SetLicense(gemboxLicense);

            // Create a new ExcelFile
            var workbook = new ExcelFile();

            // Add a worksheet
            var worksheet = workbook.Worksheets.Add("Export");

            // Insert the DataTable into the worksheet
            worksheet.InsertDataTable(dt, new InsertDataTableOptions("A1") { ColumnHeaders = true });
            return workbook;
        }
        catch (Exception ex)
        {
            logger.LogException(nameof(GoodJobCalculatorController), nameof(GenerateExcelFile), ex);
        }
        return new ExcelFile();
    }

    #endregion
}
