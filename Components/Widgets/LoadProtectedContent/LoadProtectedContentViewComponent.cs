using CMS.Membership;
using Convenience.org.Components.Widgets.LoadProtectedContent;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using CMS.Websites;
using System.Threading.Tasks;
using System;
using NACS.Protech.Entities;
using NACS.Protech.Framework;
using System.Linq;
using System.Text;
using BigMarkerApi;
using BigMarkerApi.Registration;

[assembly: RegisterWidget(identifier: LoadProtectedContentViewComponent.IDENTIFIER, name: "LoadProtectedContent",
    propertiesType: typeof(LoadProtectedContentProperties), viewComponentType: typeof(LoadProtectedContentViewComponent), Description = "LoadProtectedContent",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.LoadProtectedContent
{
    public class LoadProtectedContentViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "LoadProtectedContent";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        public UserInfo user = MembershipContext.AuthenticatedUser;
        public ContactRepository conRepo = new ContactRepository();
        public string CustomerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

        public LoadProtectedContentViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebPageUrlRetriever? _webPageUrlRetriever)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            webPageUrlRetriever = _webPageUrlRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<LoadProtectedContentProperties> widgetProperties)
        {

            var currentUrlPath = await webPageUrlRetriever!.Retrieve(widgetProperties.Page.WebPageItemID, "en");
            var currentURL = currentUrlPath.RelativePath;
            LoadProtectedContentViewModel vm = new LoadProtectedContentViewModel();
            string cid = GetCustID();
            if (cid == "0" || cid == "")
            {
                vm.RedirectURL = string.Format("/Convenience.org/ApplicationPages/Login.aspx?Source={0}", currentURL);
                return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
            }

            string pid = NACS_Classes.NACSUtilities.GetQueryStringValue("pid");
            try
            {
                ProtechId PID = new ProtechId(pid);
                if (PID == null || PID == new ProtechId()) { throw new Exception("Invalid ProtectedContent Id."); }
                #region Check Contact record

                var contact = conRepo.GetByNumber(cid);
                vm.ContactEmail = widgetProperties.Properties.ContactEmail;
                if (contact == null || contact.Id == null)
                {
                    vm.ErrorMessage = "Invalid user record.";
                    return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                }

                #endregion

                #region Confirm Contact has Permission to view this Protected Content

                var genRepo = new GeneralRepository();

                NACSContentPermission cp = null;
                try
                {
                    cp = contact.ContentPermissions.Where(
                        x => x.ProtectedContentId == pid).First();
                }
                catch
                {
                    try
                    {
                        cp = contact.EnterprisePermissions.Where(
                            x => x.ProtectedContentId == pid).First();
                    }

                    catch { cp = null; }
                }

                if (cp == null)
                {
                    vm.ErrorMessage = "User does not have permission to view this content.";
                    return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                }

                #endregion

                #region Check ContentStartDate and OverrideStartDate Flag

                else if (cp.ContentStartDate.HasValue && cp.ContentStartDate.Value > DateTime.Now
                    && !cp.OverrideStartDate)
                {
                    if (!string.IsNullOrWhiteSpace(cp.PreLaunchMessage))
                    {
                        vm.ErrorMessage = cp.PreLaunchMessage;
                    }
                    else
                    {
                        vm.ErrorMessage = "This content is not yet available for viewing."
                            + " Be sure to check back on " + cp.ContentStartDate.Value.ToString("MMMM d") + "!";
                    }

                    vm.ShowContactEmailPanel = false;

                    return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                }

                #endregion

                #region Get Content Type

                string contentType = "NACS";
                if (!string.IsNullOrEmpty(cp.BigMarkerConferenceId)) { contentType = "BigMarker"; }
                else if (cp.AccessUrl.Contains("view.protectedpdf.com")
                    || cp.AccessUrl.Contains("viewer.convenience.org")) { contentType = "Vitrium"; }

                #endregion

                #region If Vitrium: Create WebViewer Token and redirect to Protected Content

                if (contentType == "Vitrium")
                {
                    string vid = contact.ContactNumber;
                    string t = DateTime.UtcNow.AddSeconds(60).ToString("yyyyMMddHHmmssZ");
                    string token = vid + "|" + t;

                    byte[] b = Encoding.UTF8.GetBytes(token);
                    token = Convert.ToBase64String(b);

                    var url = cp.AccessUrl + "?token=" + token;
                    vm.RedirectURL=url;
                    return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                }

                #endregion

                #region Else if BigMarker: Register with BigMarker and redirect to Protected Content

                else if (contentType == "BigMarker")
                {
                    var token = APIToken.Authenticate();
                    if (token == null || string.IsNullOrEmpty(token.api_token))
                    { throw new Exception("Could not authenticate api."); }

                    var regRepo = new RegistrantRepository(token);

                    var BMRegForm = new RegistrationForm
                    {
                        id = cp.BigMarkerConferenceId,
                        email = contact.EmailAddress1,
                        first_name = contact.FirstName,
                        last_name = contact.LastName,
                        custom_user_id = contact.ContactNumber,
                        temporary_password = contact.Id.ToString(),
                        utm_bmcr_source = null
                    };
                    var regResponse = regRepo.Register(BMRegForm);

                    if (!string.IsNullOrEmpty(regResponse.conference_url))
                    {
                        vm.RedirectURL = regResponse.conference_url;
                        return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                    }
                    else { throw new Exception("Error registering user with BigMarker."); }
                }

                #endregion

                #region Else if NACS: Check for WebRoles, then Redirect to Protected Content

                else if (contentType == "NACS")
                {
                    #region Check For Required WebRole

                    if (cp.NACSWebRoleId != null)
                    {
                        if (user == null)
                        {
                            vm.RedirectURL = "~/Convenience.org/ApplicationPages/Login.aspx?Source=" + currentURL;
                            return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                        }

                        //TBD need to check role stuff 
                        //check roles and if role does not exist, add user to role
                        //if (!user.Is(cp.NACSWebRoleName, "", true, true))
                        //{
                        //    UserInfoProvider.AddUserToRole(user.UserName, cp.NACSWebRoleName, "");
                        //}
                    }

                    #endregion
                    vm.RedirectURL = cp.AccessUrl;
                    return View("~/Components/Widgets/LoadProtectedContent/_LoadProtectedContent.cshtml", vm);
                }

                #endregion
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            return View("~/Components/Widgets/NACSPACContributeButton/_NACSPACContributeButton.cshtml", vm);
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
