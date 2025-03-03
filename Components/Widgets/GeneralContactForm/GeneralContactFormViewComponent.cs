using CMS.Membership;
using Convenience.org.Components.Widgets.GeneralContactForm;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.Websites;
using System.Threading.Tasks;
using System;
using NACS.Protech.Framework;
using System.Collections.Generic;
using CMS.Helpers;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

[assembly: RegisterWidget(identifier: GeneralContactFormViewComponent.IDENTIFIER, name: "GeneralContactForm",
    propertiesType: typeof(GeneralContactFormProperties), viewComponentType: typeof(GeneralContactFormViewComponent), Description = "GeneralContactForm",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.GeneralContactForm
{
    public class GeneralContactFormViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "GeneralContactForm";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        public UserInfo user = MembershipContext.AuthenticatedUser;
        public ContactRepository conRepo = new ContactRepository();
        public string CustomerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;
        public string clientID = string.Empty;
        public string CustID = string.Empty;
        public string requestTypes = string.Empty;
        protected string NACSAPIKey = ConfigurationManager.AppSettings["NACSAPIKey"];
        public Dictionary<string, string> emails
        {
            get
            {
                var ret = TempData["emails"] as Dictionary<string, string>;
                if (ret == null)
                    ret = new Dictionary<string, string>();
                return ret;
            }
            set
            {
                TempData["emails"] = value;
            }
        }

        public GeneralContactFormViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebPageUrlRetriever? _webPageUrlRetriever)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            webPageUrlRetriever = _webPageUrlRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<GeneralContactFormProperties> widgetProperties)
        {
            var currentUrlPath = await webPageUrlRetriever!.Retrieve(widgetProperties.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            GeneralContactFormViewModel vm = new GeneralContactFormViewModel();

            try
            {
                CustID = GetCustID();
                if (CustID == "")
                {
                    vm.ManageProfileText = "<a href='/AccountAdmin/ManageProfile'>Manage Profile</a>"
                        + "<br /><i>(you will be required to sign in first)</i>";
                }
                else
                {
                    if (!string.IsNullOrEmpty(CustID))
                    {
                        var svc = new NACS.Helper.AuthService.NACSAPIAuthenticationSoapClient();
                        var user = svc.Individual_GetById(CustID, "", NACSAPIKey);
                        if (user != null)
                        {
                            vm.FirstName = user.FirstName;
                            vm.LastName = user.LastName;
                            vm.Email = user.Email;
                            vm.CompanyName = user.CompanyName;
                            vm.Phone = user.Phone;
                            vm.PhoneExtension = user.PhoneExtension;
                        }
                    }
                    vm._hdnCiD = CustID;
                    vm.ManageProfileText = "<a href='/AccountAdmin/ManageProfile'>Manage Profile</a>";
                }

                vm.ShowMainPanel = true;
                vm.ShowFinishPanel = false;

                requestTypes = DataHelper.GetNotEmpty(widgetProperties.Properties.RequestType, string.Empty);
                var requestTypeArray = requestTypes.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var emls = new Dictionary<string, string>();
                List<SelectListItem> lstRequestType  = new List<SelectListItem>();
                for (var i = 0; i < requestTypeArray.Length; i++)
                {
                    var rtaParts = requestTypeArray[i].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var li = new SelectListItem { Value = rtaParts[0].Trim(), Text = rtaParts[1].Trim() };
                    lstRequestType.Add(li);
                    emls.Add(rtaParts[0].Trim(), rtaParts[2].Trim());
                }
                emails = emls;
                vm.ListEmails = emails;
                vm.ListRequestType = lstRequestType;
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }
            return View("~/Components/Widgets/GeneralContactForm/_GeneralContactForm.cshtml", vm);
        }


        #region Internal Methods

        internal string GetCustID()
        {
            string custid = "";

            if (user != null)
            {
                custid = CustomerID;
            }
            else
            {
                custid = NACS_Classes.NACSUtilities.GetQueryStringValue("cid");
            }

            return custid;
        }

        #endregion Internal Methods
    }
}
