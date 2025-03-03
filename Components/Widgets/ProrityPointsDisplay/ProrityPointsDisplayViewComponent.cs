using CMS.Core;
using CMS.DataEngine;
using CMS.OnlineForms;

using Convenience.org.Components.Widgets;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using NACS.Helper.AuthService;
using NACS.Protech.Entities;

using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Convenience.org.Components
{
    public class ProrityPointsDisplayViewComponent : ViewComponent
    {
        private readonly IEventLogService _eventLogService;
        private readonly IInfoProvider<BizFormInfo> _bizFormInfoProvider;
        private readonly IConfiguration _configuration;
        public string mxtoken = "";

        private ProrityPointsDisplayViewModel viewModel = new ProrityPointsDisplayViewModel();

        public ProrityPointsDisplayViewComponent(IEventLogService eventLogService, IInfoProvider<BizFormInfo> bizFormInfoProvider, IConfiguration configuration)
        {
            _eventLogService = eventLogService;
            _bizFormInfoProvider = bizFormInfoProvider;
            _configuration = configuration;
        }
        public IViewComponentResult Invoke()
        {
            var currentUser = CMS.Membership.MembershipContext.AuthenticatedUser;
            //C-00146035 is Mike Rahel, Kentico developer. Feel free to change this if I am no longer around :'(
            mxtoken = GetProtechMXTokenFromAPI(currentUser.UserName == "administrator" ? "C-00146035" : currentUser.UserName);
            //viewModel.MembershipRenewUrl = "https://mynacs.convenience.org/My-Account/Company-Membership?token=" + mxtoken;
            viewModel = new ProrityPointsDisplayViewModel
            {
                MembershipRenewUrl = "https://mynacs.convenience.org/My-Account/Company-Membership?token=" + mxtoken
            };
            CompanyPriorityPoints();
            return View("~/Components/Widgets/ProrityPointsDisplay/ProrityPointsDisplay.cshtml", viewModel);
        }
        protected string CustomerID
        {
            get
            {
                if (CMS.Membership.MembershipContext.AuthenticatedUser != null && CMS.Membership.MembershipContext.AuthenticatedUser.UserName != "public")
                    return CMS.Membership.MembershipContext.AuthenticatedUser.UserName;
                return "";
            }
        }
        private void CompanyPriorityPoints()
        {
            try
            {
                var contact = Contact.GetByAnyId(CustomerID);
                if (contact == null)
                {
                    viewModel.Description = "Contact not found.";
                    return;
                }

                var account = contact?.ParentAccount;
                if (account == null)
                {
                    viewModel.Description = "Account not found.";
                    return;
                }

                viewModel.CompanyName = account.Name;
                viewModel.PriorityPoints = account.PriorityPoints > 0
                    ? $"{account.PriorityPoints} Priority Points"
                    : "0 Priority Point";

                if (account.MembershipExpiration.HasValue)
                {
                    DateTime todayDate = DateTime.Today;
                    DateTime membershipExpirationDate = account.MembershipExpiration.Value.Date;
                    viewModel.MembershipExpired = membershipExpirationDate < todayDate;
                }
                else
                {
                    viewModel.MembershipExpired = false;
                }

                viewModel.IsMember = !viewModel.MembershipExpired;
                viewModel.MembershipStatus = viewModel.IsMember ? "NACS Member" : "Membership Expired";

                if (account.NACSPrimaryMembership != null)
                    viewModel.MembershipType = account.NACSPrimaryMembership.MemberTypeName;

                if (account.NACSSecondaryMembership != null)
                    viewModel.SecondaryMembershipType = account.NACSSecondaryMembership.MemberTypeName;

                if (viewModel.IsMember)
                {
                    DateTime currentDate = DateTime.Now.AddMonths(2);
                    if (!(currentDate > new DateTime(currentDate.Year, 6, 15) && currentDate < new DateTime(currentDate.Year, 12, 1)))
                    {
                        if (account.MemberBenefitName == "NACS Hunter Club")
                        {
                            viewModel.Description = "Your exhibit space selection dates are: " + account.BracketSelection;
                        }
                        else
                        {
                            string bracketType = account.ExhibitorRecords.Count == 0 ? "new" : "";
                            int priorityPoints = account.PriorityPoints ?? 0;
                            DateTime bracketSelectionDate = GetBracket(priorityPoints, bracketType);
                            viewModel.BracketSelectionDate = bracketSelectionDate.ToShortDateString();

                            viewModel.Description = $"Based on your company’s current priority points, you will be able to select a booth for the 2024 NACS Show starting: {viewModel.BracketSelectionDate}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.Description = "Error: " + ex.Message;
            }
        }
        public DateTime GetBracket(int points, string type)
        {
            DateTime bracket = new DateTime();
            var formObject = _bizFormInfoProvider.Get("ExhibitorPriorityPointBrackets");
            if (formObject != null)
            {
                string bracket_type = (type.ToLower() == "new") ? "New Exhibitor" : "Regular";
                DataClassInfo formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
                string formClassName = formClass.ClassName;

                ObjectQuery<BizFormItem> data = BizFormItemProvider.GetItems(formClassName)
                    .WhereEquals("BracketType", bracket_type)
                    .WhereGreaterOrEquals("PointRange_Highest", points)
                    .WhereLessOrEquals("PointRange_Lowest", points);

                if (data.Count > 0)
                {
                    BizFormItem item = data.FirstOrDefault();
                    if (item != null)
                    {
                        bracket = item.GetDateTimeValue("OpenDate", new DateTime(2024, 06, 01, 9, 0, 0));
                    }
                    else
                    {
                        bracket = new DateTime(2000, 12, 31, 9, 0, 0);
                    }
                }
                else
                {
                    bracket = new DateTime(2000, 12, 31, 9, 0, 0);
                }
            }
            return bracket;
        }
        //private string GetProtechMXToken(string ProtechNumber)
        //{
        //    var task = Task.Run(() =>
        //    {
        //        return GetProtechMXTokenFromAPI(ProtechNumber);
        //    });

        //    bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(5000));

        //    if (isCompletedSuccessfully)
        //    {
        //        return task.Result;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        private string GetProtechMXTokenFromAPI(string ProtechNumber)
        {
            // Ensure the correct binding and endpoint address are used for the SOAP client
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            EndpointAddress endpoint = new EndpointAddress("https://api-test.nacsonline.com/nacssoap/AuthService.asmx");

            NACSAPIAuthenticationSoapClient authService = new(binding, endpoint);
            string mxtoken = "";

            //BEGIN PROTECH API CALL (hide if API is having issues)-----------------------
            try
            {
                var savedtoken = HttpContext.Session.GetString("NACSMXToken") as string;
                //var savedtoken = SessionHelper.GetValue("NACSMXToken") as string;

                if (savedtoken != null) 
                {
                    mxtoken = savedtoken;
                }
                else
                {
                    //Get NACSAPIKEY value
                    var nacsApiKey = _configuration["NACSAPIKey"];
                    //Get new token from API
                    NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, nacsApiKey);
                    mxtoken = serviceUser.Token.ToString();

                    //SessionHelper.SetValue("NACSMXToken", mxtoken);
                    HttpContext.Session.SetString("NACSMXToken", mxtoken);

                }
            }
            catch (Exception ex)
            {
                //EventLogProvider.LogException("Welcome Control MX Auth Token", "ERROR GETTING TOKEN", ex);
                _eventLogService.LogException("Welcome Control MX Auth Token", "ERROR GETTING TOKEN", ex);
            }
            //END PROTECH API CALL ----------------------------------------------

            return mxtoken;
        }
    }
}
