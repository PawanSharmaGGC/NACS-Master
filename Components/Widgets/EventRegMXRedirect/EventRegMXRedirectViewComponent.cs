using CMS.ContentEngine;
using CMS.Membership;
using CMS.Websites.Routing;
using Convenience.org.Components.Widgets.EventRegMXRedirect;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using NACS.Helper.AuthService;
using CMS.Websites;
using Microsoft.AspNetCore.Http;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using NACS_Classes;

[assembly: RegisterWidget(identifier: EventRegMXRedirectViewComponent.IDENTIFIER, name: "EventRegMXRedirect",
    viewComponentType: typeof(EventRegMXRedirectViewComponent),
    propertiesType: typeof(EventRegMXRedirectProperties), Description = "EventRegMXRedirect",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventRegMXRedirect
{
    public class EventRegMXRedirectViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "EventRegMXRedirect";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IContentQueryExecutor contentQueryExecutor;
        public string EventKey = "30b442e5-7032-ec11-b6e5-000d3a9d00cd";
        protected string NACSAPIKey = ConfigurationManager.AppSettings["NACSAPIKey"];

        public EventRegMXRedirectViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebsiteChannelContext channelContext, IContentQueryExecutor contentQueryExecutor, IWebPageUrlRetriever? _webPageUrlRetriever, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.channelContext = channelContext;
            this.contentQueryExecutor = contentQueryExecutor;
            webPageUrlRetriever = _webPageUrlRetriever;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<EventRegMXRedirectProperties> properties)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            var currentUrlPath = await webPageUrlRetriever!.Retrieve(properties.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            var user = httpContextAccessor.HttpContext.User.Identity;
            var vm = new EventRegMXRedirectViewModel();
            string autoredir = NACSUtilities.GetQueryStringValue("autoredir");
            string mid = NACSUtilities.GetQueryStringValue("mid");
            string pnum = NACSUtilities.GetQueryStringValue("pnum");
            string pkey = NACSUtilities.GetQueryStringValue("pkey");
            string site = NACSUtilities.GetQueryStringValue("site");
            string src = NACSUtilities.GetQueryStringValue("utm_medium");
            var customerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

            vm.RegistrationSiteURL = properties.Properties.RegistrationSiteURL;
            vm.RegistrationSite_Production = properties.Properties.RegistrationSite_Production;
            vm.RegistrationSite_Staging = properties.Properties.RegistrationSite_Staging;
            //--BSM URL fix - bad URL from Mktg sendig to MX instead of NACS Show - 5/8/2024
            //--forward user into nacsshow.com
            if (mid == "c5dd9f9e-6b8d-ee11-8179-00224827b364") //NACS Show 2024
            {
                pkey = (!string.IsNullOrEmpty(pnum)) ? GetPersonKey(pnum) : "";
                vm.RedirectURL = "https://www.nacsshow.com/register/start?nacskey=" + pkey + "&srccode=" + src;
                return View("~/Components/Widgets/EventRegMXRedirect/_EventRegMXRedirect.cshtml", vm);
            }

            if (site == "fi")
            {
                vm.RegistrationSite_Staging = "http://fuelsinstitute-nacsstagednn1.pcbscloud.com";
                vm.RegistrationSite_Production = "https://myfi.fuelsinstitute.org";
            }
            else if (site == "tei")
            {
                vm.RegistrationSite_Staging = "http://fuelsinstitute-nacsstagednn1.pcbscloud.com";
                vm.RegistrationSite_Production = "https://myfi.fuelsinstitute.org";
            }
            else if (site == "cx")
            {
                vm.RegistrationSite_Staging = "http://conexxus-nacsstagednn1.pcbscloud.com";
                vm.RegistrationSite_Production = "https://conexxus.convenience.org";
            }


            string mxsite = "";

            if (currentURL != null)
            {
                if (currentURL.ToString().ToLower().Contains("staging"))
                {
                    mxsite = properties.Properties.RegistrationSite_Staging + "/Events/Calendar/Registration-Start";
                }
                else if (currentURL.ToLower().Contains("kentico") || currentURL.ToLower().Contains("dev"))
                {
                    mxsite = vm.RegistrationSite_Staging + "/Events/Calendar/Registration-Start";
                }
                else
                {
                    mxsite = vm.RegistrationSite_Production + "/Events/Calendar/Registration-Start";
                }
            }

            if (string.IsNullOrEmpty(mid))
                mid = EventKey;

            if (user != null && user.IsAuthenticated && !string.IsNullOrEmpty(pnum))
            {
                string cid = (!string.IsNullOrEmpty(pnum)) ? pnum : customerID;
                string mxtoken = GetProtechMXToken(cid);
                vm.ShowAnonymousPanel = false;
                vm.ShowAuthenticatedPanel = true;
                vm.AuthenticatedNavigateUrl = string.Format("{0}?MeetingId={1}&token={2}", mxsite, mid, mxtoken);

                if (!channelContext.IsPreview)
                {
                    vm.RedirectURL = string.Format("{0}?MeetingId={1}&token={2}", mxsite, mid, mxtoken);
                    return View("~/Components/Widgets/EventRegMXRedirect/_EventRegMXRedirect.cshtml", vm);
                }
            }
            else
            {
                vm.ShowAnonymousPanel = true;
                vm.ShowAuthenticatedPanel = false;
                vm.AnonymousNavigateUrl = string.Format("/Convenience.org/ApplicationPages/Login.aspx?autoredir=1&Source={0}", currentURL);

                if (!channelContext.IsPreview)
                {
                    vm.RedirectURL = string.Format("/Convenience.org/ApplicationPages/Login.aspx?autoredir=1&Source={0}", currentURL);
                    return View("~/Components/Widgets/EventRegMXRedirect/_EventRegMXRedirect.cshtml", vm);
                }
            }

            return View("~/Components/Widgets/EventRegMXRedirect/_EventRegMXRedirect.cshtml", vm);
        }

        private string GetProtechMXToken(string ProtechNumber)
        {
            NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();

            NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, ConfigurationManager.AppSettings["NACSAPIKey"]);

            return serviceUser.Token.ToString();

        }

        private string GetPersonKey(string ID)
        {
            string nacskey = "";

            try
            {
                //if logged in, use that key first
                UserInfo currentUser = MembershipContext.AuthenticatedUser;
                if (currentUser != null)
                {
                    nacskey = MembershipContext.AuthenticatedUser.GetStringValue("ProtechId", "");
                }
                else
                {
                    NACS.Helper.CustomerService.NACSAPICustomerSoapClient service = new NACS.Helper.CustomerService.NACSAPICustomerSoapClient();

                    NACS.Helper.CustomerService.NACSIndividual dt = service.Individual_GetById(ID, "", NACSAPIKey);

                    nacskey = dt.ProtechId.ToString();
                }
            }
            catch
            {
                nacskey = "";
            }

            return nacskey;
        }
    }
}