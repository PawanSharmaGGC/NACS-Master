using CMS.ContentEngine;
using CMS.Membership;
using CMS.Websites.Routing;
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
using Convenience.org.Components.Widgets.EventRegMXRedirectSessionSignup;
using NACS.Protech.Framework;
using NACS.Protech.Entities;
using System.Linq;
using System.ServiceModel;
using NACSUser = NACS.Helper.AuthService.NACSUser;


[assembly: RegisterWidget(identifier: EventRegMXRedirectSessionSignupViewComponent.IDENTIFIER, name: "EventRegMXRedirectSessionSignup",
    viewComponentType: typeof(EventRegMXRedirectSessionSignupViewComponent),
    propertiesType: typeof(EventRegMXRedirectSessionSignupProperties), Description = "EventRegMXRedirectSessionSignup",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventRegMXRedirectSessionSignup
{
    public class EventRegMXRedirectSessionSignupViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "EventRegMXRedirect";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IContentQueryExecutor contentQueryExecutor;
        protected string NACSAPIKey = ConfigurationManager.AppSettings["NACSAPIKey"];

        public EventRegMXRedirectSessionSignupViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebsiteChannelContext channelContext, IContentQueryExecutor contentQueryExecutor, IWebPageUrlRetriever? _webPageUrlRetriever, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.channelContext = channelContext;
            this.contentQueryExecutor = contentQueryExecutor;
            webPageUrlRetriever = _webPageUrlRetriever;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<EventRegMXRedirectSessionSignupProperties> properties)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            var currentUrlPath = await webPageUrlRetriever!.Retrieve(properties.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            var user = httpContextAccessor.HttpContext.User.Identity;
            var vm = new EventRegMXRedirectSessionSignupViewModel();
            string autoredir = NACSUtilities.GetQueryStringValue("autoredir");
            string mcode = NACSUtilities.GetQueryStringValue("mcode");
            string pnum = NACSUtilities.GetQueryStringValue("pnum");
            string pkey = NACSUtilities.GetQueryStringValue("pkey");
            var customerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

            vm.RegistrationSiteURL = properties.Properties.RegistrationSiteURL;
            vm.ReturnURL = properties.Properties.ReturnURL;
            vm.EventCode = properties.Properties.EventCode;
            vm.ShowRegisterButton = properties.Properties.ShowRegisterButton;

            string mxsite = "";

            if (currentURL != null)
            {
                if (currentURL.ToString().ToLower().Contains("staging"))
                {
                    mxsite = "https://nacsstagednn1.pcbscloud.com/Events/Edit";
                }
                else if (currentURL.ToLower().Contains("kentico") || currentURL.ToLower().Contains("dev"))
                {
                    mxsite = "https://nacsstagednn1.pcbscloud.com/Events/Edit";
                }
                else
                {
                    mxsite = "https://mynacs.convenience.org/Events/Edit";
                }
            }

            if (string.IsNullOrEmpty(mcode))
                mcode = vm.EventCode;


            if (user != null && user.IsAuthenticated && !string.IsNullOrEmpty(pkey))
            {
                vm.ShowAnonymousPanel = false;
                vm.ShowAuthenticatedPanel = true;

                try
                {
                    GeneralRepository genRepo = new GeneralRepository();

                    var evt = Event.GetByCode(mcode);
                    var evtsessions = evt.Sessions;
                    var registrants = Event.GetRegistrantsByEvent(evt.Id); //Get EventRegistrants By EventId static method

                    var registrant = registrants.Where(r => r.ContactId == pkey).First();

                    string cid = (!string.IsNullOrEmpty(pnum)) ? pnum : customerID;
                    string mxtoken = GetProtechMXToken(cid);
                    string utms = GetUTMs();

                    vm.AuthenticatedNavigateUrl = string.Format("{0}?RegId={1}&token={2}{3}", mxsite, registrant.Id, mxtoken, utms);

                    if (!channelContext.IsPreview)
                    {
                        vm.RedirectURL = string.Format("{0}?RegId={1}&token={2}{3}", mxsite, registrant.Id, mxtoken, utms);
                        return View("~/Components/Widgets/EventRegMXRedirectSessionSignup/_EventRegMXRedirectSessionSignup.cshtml", vm);
                    }
                }
                catch
                {
                    vm.ShowAnonymousPanel = false;
                    vm.ShowAuthenticatedPanel = true;
                    vm.InformationMessage = "<p>Problem finding registration. Please contact NACS.</p>";
                }
            }
            else
            {
                vm.ShowAnonymousPanel = true;
                vm.ShowAuthenticatedPanel = false;
                vm.AnonymousNavigateUrl = "/Convenience.org/ApplicationPages/Login.aspx?autoredir=1&Source=" + currentURL;

                if (!channelContext.IsPreview)
                {
                    vm.RedirectURL = string.Format("/Convenience.org/ApplicationPages/Login.aspx?autoredir=1&Source={0}", currentURL);
                    return View("~/Components/Widgets/EventRegMXRedirectSessionSignup/_EventRegMXRedirectSessionSignup.cshtml", vm);
                }
            }
            return View("~/Components/Widgets/EventRegMXRedirectSessionSignup/_EventRegMXRedirectSessionSignup.cshtml", vm);
        }

        private string GetProtechMXToken(string ProtechNumber)
        {
            // Ensure the correct binding and endpoint address are used for the SOAP client
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress("https://api-test.nacsonline.com/nacssoap/AuthService.asmx");

            NACSAPIAuthenticationSoapClient authService = new(binding.ToString(), endpoint.ToString());

            NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, ConfigurationManager.AppSettings["NACSAPIKey"]);

            return serviceUser.Token.ToString();
        }

        private string GetUTMs()
        {
            string utms = "";

            utms += (NACSUtilities.GetQueryStringValue("utm_source") != null) ? "&utm_source=" + NACSUtilities.GetQueryStringValue("utm_source") : "";
            utms += (NACSUtilities.GetQueryStringValue("utm_campaign") != null) ? "&utm_campaign=" + NACSUtilities.GetQueryStringValue("utm_campaign") : "";
            utms += (NACSUtilities.GetQueryStringValue("utm_medium") != null) ? "&utm_medium=" + NACSUtilities.GetQueryStringValue("utm_medium") : "";
            utms += (NACSUtilities.GetQueryStringValue("utm_content") != null) ? "&utm_content=" + NACSUtilities.GetQueryStringValue("utm_content") : "";
            utms += (NACSUtilities.GetQueryStringValue("utm_term") != null) ? "&utm_term=" + NACSUtilities.GetQueryStringValue("utm_term") : "";

            return utms;
        }
    }
}