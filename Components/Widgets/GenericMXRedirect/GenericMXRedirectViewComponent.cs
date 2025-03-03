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
using Convenience.org.Components.Widgets.GenericMXRedirect;

[assembly: RegisterWidget(identifier: GenericMXRedirectViewComponent.IDENTIFIER, name: "GenericMXRedirect",
    viewComponentType: typeof(GenericMXRedirectViewComponent), Description = "GenericMXRedirect",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.GenericMXRedirect
{
    public class GenericMXRedirectViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "GenericMXRedirect";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebsiteChannelContext channelContext;

        public GenericMXRedirectViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebsiteChannelContext channelContext, IWebPageUrlRetriever? _webPageUrlRetriever, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.channelContext = channelContext;
            webPageUrlRetriever = _webPageUrlRetriever;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel componentViewModel)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            var currentUrlPath = await webPageUrlRetriever!.Retrieve(componentViewModel.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            var user = httpContextAccessor.HttpContext.User.Identity;
            var vm = new GenericMXRedirectViewModel();
            string autoredir = NACSUtilities.GetQueryStringValue("autoredir");
            string mxsite = NACSUtilities.GetQueryStringValue("mxurl");
            string pnum = NACSUtilities.GetQueryStringValue("pnum");
            string pid = NACSUtilities.GetQueryStringValue("pid");
            var customerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

            if (user != null && user.IsAuthenticated || !string.IsNullOrEmpty(pnum) || !string.IsNullOrEmpty(pid))
            {
                string cid = (!string.IsNullOrEmpty(pnum)) ? pnum : customerID;
                string ckey = (!string.IsNullOrEmpty(pid)) ? pid : "";// this is the NF key: this.CustomerKey;

                string mxtoken = GetProtechMXToken(cid, ckey);
                vm.ShowAnonymousPanel = false;
                vm.ShowAuthenticatedPanel = true;
                vm.AuthenticatedNavigateUrl = string.Format("{0}?token={1}", mxsite, mxtoken);

                if (!channelContext.IsPreview)
                {
                    vm.RedirectURL = string.Format("{0}?token={1}", mxsite, mxtoken);
                    return View("~/Components/Widgets/GenericMXRedirect/_GenericMXRedirect.cshtml", vm);
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
                    return View("~/Components/Widgets/GenericMXRedirect/_GenericMXRedirect.cshtml", vm);
                }
            }
            return View("~/Components/Widgets/GenericMXRedirect/_GenericMXRedirect.cshtml", vm);
        }

        private string GetProtechMXToken(string ProtechNumber, string ProtechId)
        {
            NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();

            string id = (ProtechId != "") ? ProtechId : ProtechNumber;
            NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(id, ConfigurationManager.AppSettings["NACSAPIKey"]);

            return serviceUser.Token.ToString();
        }
    }
}