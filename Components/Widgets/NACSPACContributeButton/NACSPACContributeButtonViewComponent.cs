using CMS.Membership;
using Convenience.org.Components.Widgets.NACSPACContributeButton;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using CMS.Websites;
using System.Threading.Tasks;
using NACS.Helper.CustomerService;
using System;

[assembly: RegisterWidget(identifier: NACSPACContributeButtonViewComponent.IDENTIFIER, name: "NACSPACContributeButton",
    viewComponentType: typeof(NACSPACContributeButtonViewComponent), Description = "NACSPACContributeButton",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.NACSPACContributeButton
{
    public class NACSPACContributeButtonViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "NACSPACContributeButton";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        protected string NACSAPIKey = ConfigurationManager.AppSettings["NACSAPIKey"];
        public string CustomerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

        public NACSPACContributeButtonViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebPageUrlRetriever? _webPageUrlRetriever)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            webPageUrlRetriever = _webPageUrlRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel componentViewModel)
        {
            var currentUrlPath = await webPageUrlRetriever!.Retrieve(componentViewModel.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            var vm = new NACSPACContributeButtonViewModel();

            string CstRecno = GetCustID();
            if (CstRecno != "0")
            {
                if (CheckAuthorization(CstRecno) == true)
                {
                    vm.ShowNotAuthorizedPanel = false;
                    vm.ShowAuthorizedPanel = true;

                    string mxsite = "";

                    if (currentURL.ToLower().Contains("staging") || currentURL.ToLower().Contains("kentico") || currentURL.ToLower().Contains("dev"))
                    {
                        vm.NavigateUrl = "https://nacspac-nacsstagednn1.pcbscloud.com";
                    }
                    else
                    {
                        vm.NavigateUrl = "https://nacspac.convenience.org";
                    }
                }
            }

            return View("~/Components/Widgets/NACSPACContributeButton/_NACSPACContributeButton.cshtml", vm);
        }

        //Gets the current SP user login name
        private string GetCustID()
        {
            return this.CustomerID;
        }

        //Get currently logged in user details from NF database
        internal bool CheckAuthorization(string cid)
        {
            bool status = false;
            bool CompanyAuthFlag = false;
            bool FONFlag = false;

            NACSAPICustomerSoapClient service = new NACSAPICustomerSoapClient();

            NACSIndividual ind = service.Individual_GetById(cid, "", this.NACSAPIKey);

            if (ind != null)
            {
                CompanyAuthFlag = ind.PACOrgAuthorized;

                if (ind.FONExpireDate > DateTime.Now)
                {
                    FONFlag = true;
                }
            }

            #region Pre-Protech Code
            /* Pre-Protech code with NF API

            NACSPACContributorDetail contributor = service.NACSPAC_GetContributor(cid, "", this.NACSAPIKey);

            if (contributor != null)
            {
                CompanyAuthFlag = contributor.AuthorizedByCompany;
                FONFlag = contributor.FriendOfNACS;
                //PreviousContribution = contributor.TotalPACContributions;
            }

            if (CompanyAuthFlag == "Yes" || FONFlag == "Yes") { status = true; }
            */
            #endregion

            if (CompanyAuthFlag == true || FONFlag == true) { status = true; }

            return status;
        }
    }
}
