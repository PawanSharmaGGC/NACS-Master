using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using CMS.Core;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.GoodJobCalculator;
using Convenience.org.Helpers;
using CMS.Helpers;
using Convenience.org.Controllers;
using Microsoft.Extensions.Logging;
using Convenience.org.Components.Widgets.Tier2Hero;
using CMS.MediaLibrary;
using System.Linq;
using NACS.Portal.Core.Services;

[assembly: RegisterWidget(identifier: GoodJobCalculatorWidget.IDENTIFIER, name: "Good Job Calculator",
    viewComponentType: typeof(GoodJobCalculatorWidget),
    propertiesType: typeof(GoodJobCalculatorProperties), Description = "Good Job Calculator",
    IconClass = "icon-pda", AllowCache = true)]

namespace Convenience.org.Components.Widgets.GoodJobCalculator;

public class GoodJobCalculatorWidget : ViewComponent
{
    public const string IDENTIFIER = "GoodJobCalculator";
    private readonly IEventLogService _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssetItemService _itemService;

    public GoodJobCalculatorWidget(IEventLogService logger, IHttpContextAccessor httpContextAccessor, IAssetItemService itemService)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _itemService = itemService;
    }

    public IViewComponentResult Invoke(int step, GoodJobCalculatorViewModel model, GoodJobCalculatorProperties properties)
    {
        try
        {
            if (step == 0)
            {
                model = LoadSessionData();

                var logoImage = _itemService.RetrieveMediaFileImage(properties?.Image?.FirstOrDefault()).Result;
                model.LogoImageUrl = logoImage == null ? "" : logoImage?.URLData?.RelativePath;

                model.CurrentStep = 1;
            }

            model = model ?? new GoodJobCalculatorViewModel();

            if (model.CurrentStep == 3)
            {
                DataCalculations(model);
            }

            // Save to session
            HttpContext.Session.SetObject("GoodJobCalculatorData", model);

            // Return the correct view for the current step
            if (step > 0)
                return View($"~/Components/Widgets/GoodJobCalculator/_GoodJobCalculatorStep{model.CurrentStep}.cshtml", model);
            else
            {
                return View("~/Components/Widgets/GoodJobCalculator/GoodJobCalculator.cshtml", model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(nameof(GoodJobCalculatorWidget), nameof(Invoke), ex);
        }

        return View("~/Components/Widgets/GoodJobCalculator/GoodJobCalculator.cshtml", new GoodJobCalculatorViewModel());
    }


    private void DataCalculations(GoodJobCalculatorViewModel model)
    {
        try
        {
            //Revenue Uplift 
            model.Text0 = model.Field0R.Trim().ToString(); //Number of stores (at the beginning of your most recent fiscal year) //Ticket size (inside sales / inside
            model.Text1 = model.Field1R.Trim().ToString(); //Total annual inside sales
                                                           //Text2.Value = Field2R.Value.Trim().ToString(); //Average inside square footage per store 
            model.Text3 = model.Field3R.Trim().ToString(); //Total number of inside transactions per store per day
            model.Text4 = model.Field4R.Trim().ToString(); //Inside net margin (Formula: GP Total Inside/Inside Sales)
            model.Text5 = model.Field5R.Trim().ToString(); //Average number of gallons sold per fuel transaction
            model.Text6 = model.Field6R.Trim().ToString(); //Average cent per gallon fuel gross margin (e.g., $0.38 per gallon)

            //Cost Mitigation
            model.Text7 = model.Field0C.Trim().ToString(); //Annual cost of goods sold for inside (merchandise and foodservice) per store            
            model.Text8 = model.Field2C.Trim().ToString(); //Number Full-time employees
            model.Text9 = model.Field3C.Trim().ToString(); //Number Part-time employees
            model.Text10 = model.Field4C.Trim().ToString(); //Cost Full-time employee 
            model.Text11 = model.Field5C.Trim().ToString(); //Cost Part-time employee
            model.Text12 = model.Field6C.Trim().ToString(); //Annual hourly employee turnover rate        
            model.Text13 = model.Field7C.Trim().ToString(); //Annual merchandise shrink, as a % of inside sales
            model.Text14 = model.Field8C.Trim().ToString(); //Average hourly store employee overtime wage
            model.Text18 = model.Field11C.Trim().ToString(); //Average hourly store employee wage
            model.Text15 = model.Field9C.Trim().ToString(); //Full-time employee (hours per employee/per store)
            model.Text16 = model.Field10C.Trim().ToString(); //Part-time employee (hours per employee/per store)        

            var TicketSize = (Convert.ToDouble(model.Field1R) / Convert.ToDouble(model.Field3R) / Convert.ToDouble(model.Field0R) / 365);
            model.CaField1 = TicketSize.ToString("N2"); //, CultureInfo.CurrentCulture);
            model.CaField111 = TicketSize.ToString("0.00"); //, CultureInfo.CurrentCulture);
                                                            //var CAField33 = Field3R.Value.Trim().ToString();
                                                            //CaField1.Value = Field0R.Value.Trim().ToString(); //Number of stores (at the beginning of your most recent fiscal year) //Ticket size (inside sales / inside transactions)
            var CaField333 = Convert.ToDouble(model.Field3R);
            model.CaField3 = CaField333.ToString("N0"); // Field3R.Value.Trim().ToString("0");  //Total annual inside sales
            model.CaField33 = CaField333.ToString("0");  //Total annual inside sales

            var CaField777 = Convert.ToDouble(model.Field6C);
            model.CaField7 = CaField777.ToString("0"); //Annual hourly employee turnover rate        

            //Calculating Future Performance boxes
            var FP = Convert.ToDouble(TicketSize) + (Convert.ToDouble(TicketSize) * 0.049);
            //(Convert.ToDouble(Field0R.Value) + ((Convert.ToDouble(Field0R.Value) * 0.049)));
            model.CaField2 = FP.ToString("0.00");

            var FP1 = (Convert.ToDouble(model.Field3R) + (Convert.ToDouble(model.Field3R) * 0.017));
            model.CaField4 = FP1.ToString("0");

            var FP2 = Convert.ToDouble(model.Field6C) + (Convert.ToDouble(model.Field6C) * (-0.318));
            model.CaField8 = FP2.ToString("0");

            var Shrink1 = (Convert.ToDouble(model.Field1R) * (Convert.ToDouble(model.Field7C) / 100));
            var FP3 = (Shrink1 * (-0.286)) + Shrink1;//(Convert.ToDouble(Shrink1) * (-0.286)) + Convert.ToDouble(Shrink1);
            model.CaField10 = FP3.ToString("0");

            var FP4 = ((((Convert.ToDouble(model.Field2C) * Convert.ToDouble(model.Field9C)) +
                         (Convert.ToDouble(model.Field3C) * Convert.ToDouble(model.Field10C))) * 52)
                    + ((((Convert.ToDouble(model.Field2C) * Convert.ToDouble(model.Field9C)) +
                         (Convert.ToDouble(model.Field3C) * Convert.ToDouble(model.Field10C))) * 52) * (-0.137)));
            model.CaField12 = FP4.ToString("0");

            ///
            //Calculate Annual merchandise shrink, as % of sales, per store(excludes foodservice)
            var Shrink = (Convert.ToDouble(model.Field1R) * (Convert.ToDouble(model.Field7C) / 100));
            model.CaField9 = Shrink.ToString("N0");
            model.CaField999 = ValidationHelper.GetDouble(Shrink, 0);

            //Calculate Overtime and Unplanned Labor
            var OUTotal = ((Convert.ToDouble(model.Field2C) * Convert.ToDouble(model.Field9C)) +
                           (Convert.ToDouble(model.Field3C) * Convert.ToDouble(model.Field10C))) * 52;
            model.CaField11 = OUTotal.ToString("N0");
            model.CaField1111 = ValidationHelper.GetDouble(OUTotal, 0);

            //Calculate Total number of Full Time/Part Time employees included in hourly wage increase (Does not include outside contractors)
            var TotalEmployee = (Convert.ToDouble(model.Field2C) + Convert.ToDouble(model.Field3C));
            model.CaField16 = ValidationHelper.GetDouble(TotalEmployee, 0);
            model.CaField17 = ValidationHelper.GetDouble(TotalEmployee, 0);

            //Total Estimated Revenue uplift
            #region Revenu Uplift
            var TotalERU = (((Convert.ToDouble(TicketSize) * 0.049) * Convert.ToDouble(model.Field3R))
                           * Convert.ToDouble(model.Field0R) * 365);
            //(((Convert.ToDouble(Field0R.Value) * 0.049) * Convert.ToDouble(Field3R.Value))
            //               * Convert.ToDouble(Field0R.Value) * 365);
            var TotalERU1 = (((Convert.ToDouble(model.Field3R) * 0.017) * Convert.ToDouble(FP))
                             * Convert.ToDouble(model.Field0R) * 365);
            //(((Convert.ToDouble(Field3R.Value) * 0.017) * Convert.ToDouble(FP))
            //           * Convert.ToDouble(Field0R.Value) * 365);
            var Total = (Convert.ToDouble(TotalERU) + Convert.ToDouble(TotalERU1));
            model.ReField0 = Total.ToString("N0");
            model.ReField00 = ValidationHelper.GetDouble(Total, 0);
            #endregion

            //Gross profit uplift from new GJS inside revenue uplift
            #region Gross profit uplift from new GJS inside revenue uplift
            var OtherInformationutputs = (Convert.ToDouble(model.Field1R) - (Convert.ToDouble(model.Field0C) * Convert.ToDouble(model.Field0R)))
                                            / Convert.ToDouble(model.Field1R);
            var Grossprofituplift = Convert.ToDouble(Total) * Convert.ToDouble(OtherInformationutputs);
            model.ReField1 = Grossprofituplift.ToString("N0");
            model.ReField11 = ValidationHelper.GetDouble(Grossprofituplift, 0);
            #endregion

            //Total Estimated Cost Reduction
            #region Total estimated cost reduction
            var turnover = ((Convert.ToDouble(model.Field6C) / 100) * 0.318) * Convert.ToDouble(TotalEmployee)
                              * ((Convert.ToDouble(model.Field4C) + Convert.ToDouble(model.Field5C)) / 2);
            var reducedshrink = (Convert.ToDouble(Shrink) * 0.286);

            var OTHrs = ((Convert.ToDouble(OUTotal) * 0.137) * (Convert.ToDouble(model.Field8C) - Convert.ToDouble(model.Field11C))); //*52

            var TotalCost = turnover + reducedshrink + OTHrs; //Convert.ToDouble(turnover) + Convert.ToDouble(reducedshrink) + Convert.ToDouble(OTHrs); 
            model.ReField2 = TotalCost.ToString("N0");
            model.ReField22 = ValidationHelper.GetDouble(TotalCost, 0);
            #endregion

            //Estimated increase in fuel profitability
            #region Estimated increase in fuel profitability
            //if ((CaField6.Value == null) || (CaField6.Value == ""))
            //{
            var FS = (Convert.ToDouble(model.Field5R) * Convert.ToDouble(model.Field6R)) * (Convert.ToDouble(model.Field3R) * 0.017) * 0.1;
            var TotalFR = (Convert.ToDouble(FS) * (Convert.ToDouble(model.Field0R) * 365));
            model.ReField3 = TotalFR.ToString("N0");
            model.ReField33 = ValidationHelper.GetDouble(TotalFR, 0);

            //}
            #endregion

            //Total P&L impact (A+B+C)
            #region Total P&L impact (A+B+C)
            var FS1 = (Convert.ToDouble(model.Field5R) * Convert.ToDouble(model.Field6R)) * (Convert.ToDouble(model.Field3R) * 0.017) * 0.1;
            var TotalFR1 = (Convert.ToDouble(FS1) * (Convert.ToDouble(model.Field0R) * 365));
            var TotalImpact = Convert.ToDouble(Grossprofituplift) + Convert.ToDouble(TotalCost) + Convert.ToDouble(TotalFR1);
            model.ReField4 = TotalImpact.ToString("N0");
            model.ReField44 = ValidationHelper.GetDouble(TotalImpact, 0);
            #endregion

            //P&L Wages
            #region P&L Wages        
            var Wages = (Convert.ToDouble(TotalImpact) * 0.75)
                      / (30 * 52 * Convert.ToDouble(TotalEmployee));

            model.ReField5 = Wages.ToString("N2");
            model.ReField55 = ValidationHelper.GetDouble(Wages, 0.00);
            #endregion

            //P&L impact as % of current inside sales net income
            #region sales net income 
            var NetIncome = (Convert.ToDouble(TotalImpact) * 100)
                          / (Convert.ToDouble(model.Field1R) * (Convert.ToDouble(model.Field4R) / 100));
            model.ReField6 = NetIncome.ToString("N0");
            model.ReField66 = ValidationHelper.GetDouble(NetIncome, 0);
            #endregion
        }
        catch (Exception ex)
        {
            _logger.LogException(nameof(GoodJobCalculatorWidget), nameof(DataCalculations), ex);
        }
    }

    private GoodJobCalculatorViewModel LoadSessionData()
    {
        try
        {
            var session = _httpContextAccessor.HttpContext.Session;

            // Check if the cookie exists
            var cookie = HttpContext.Request.Cookies["CMSSubmittedWebForm_ContactInfo"];

            // If the cookie is found and not empty, split the values and set session data
            if (!string.IsNullOrEmpty(cookie))
            {
                var cookieArray = cookie.Split('|');

                // Store values in the session
                session.SetString("Email", cookieArray[0]);
                session.SetString("FirstName", cookieArray[1]);
                session.SetString("LastName", cookieArray[2]);

                // Optionally, you can create a model to pass to the view
                return new GoodJobCalculatorViewModel
                {
                    Email = session.GetString("Email"),
                    FirstName = session.GetString("FirstName"),
                    LastName = session.GetString("LastName"),
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(nameof(GoodJobCalculatorWidget), nameof(LoadSessionData), ex);
        }
        return new GoodJobCalculatorViewModel();
    }

    public void SendConfirmationEmail(GoodJobCalculatorViewModel model)
    {
        try
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                //var subject = "Good Job Calculator Results";
                //var body = $"Hello {model.FirstName} {model.LastName},\n\nThank you for using the Good Job Calculator.\nYour results are ready.";

                //var msg = new MailMessage("no-reply@goodjobs.com", model.Email, subject, body)
                //{
                //    IsBodyHtml = false
                //};

                //using var smtpClient = new SmtpClient("smtp.goodjobs.com")
                //{
                //    Credentials = new NetworkCredential("username", "password"),
                //    EnableSsl = true
                //};

                //smtpClient.Send(msg);
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(nameof(GoodJobCalculatorWidget), nameof(SendConfirmationEmail), ex, "Error sending confirmation email");
        }
    }
}
