using CMS.DataEngine;
using CMS.Membership;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NACS.Protech.Entities;
using NACS.Protech.Framework;
using NACS.Helper.CustomerService;
using NACS.Helper.AuthService;
using NACS.Protech.WebServices;
using NACS.Utilities;
using NACSShow.Components.Widgets.DailyNewsListing;
using Microsoft.AspNetCore.Http;
using CMS.ContentEngine;
using CMS.Websites.Routing;
using CMS.Websites;
using Kentico.Content.Web.Mvc.Routing;
using NACSShow.Repositories.Pages.Interfaces;
using CMS.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using ipstackapi;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Encodings.Web;
using Ignition.Framework.Entities;
using Microsoft.Extensions.Configuration;

namespace NACSShow.Components.Widgets.ExhibitorApplication
{
    public class CurrentLoggedInUser
    {
        public string NACSID { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
    public class Eligibility
    {
        public bool Allowed { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Reason_Header { get; set; } = string.Empty;
        public string Reason_Subheader { get; set; } = string.Empty;
    }

    public class ExhibitorApplication
    {
        public bool _pnlShowSelectExhibitor { get; set; }
        public bool _pnlShowEligibilityMessage { get; set; }
        public string _lblStatus { get; set; } = string.Empty;
    }
    public class ExhibitorApplicationViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserInfoProvider userInfoProvider;
        private IWebsiteChannelContext websiteChannelContext;
        private IConfiguration configuration;
        public string PersonID = "";
        public string MYSID = "";
        public string clientID = "";
        public CultureInfo en_US = CultureInfo.CreateSpecificCulture("en-US");

        private static string Environment = "PROD";
        string urlMYS = (Environment == "PROD") ? "http://nacs24.exh.mapyourshow.com/7_0/boothsales/index.cfm?eid=" : "http://nacs24.exh.mysstaging.com/7_0/boothsales/index.cfm?eid=";
        string urlDues = (Environment == "PROD") ? "http://www.convenience.org/Membership/Renew/RenewMembership.aspx" : "http://staging.convenience.org/Membership/Renew/RenewMembership.aspx";
        string subdomain = (Environment == "PROD") ? "https://www" : "http://staging";
        public string mxsite = (Environment == "PROD") ? "https://mynacs.convenience.org" : "https://nacsstagednn1.pcbscloud.com";
        public string renewalsURL = "";

        string returnURL = "";
        string urlLogin = "";
        string urlIsAuthenticated = "";
        private bool UserIsEditor = false;
        private bool UserIsTeamMember = false;

        //ENCRYPTION KEYS
        private static readonly string encryptionKeyExhibitApps = "NACS16EXHIBITORAPPS4MYS!"; //needs to be 16 or 24 characters long
        private static readonly string encryptionKeyLogin = "NACS16LOGINOVERRIDE4STS!"; //needs to be 16 or 24 characters long
        ExhibitorApplication exhibitorApplication = new ExhibitorApplication();

        public ExhibitorApplicationViewComponent(IHttpContextAccessor HttpContextAccessor, IUserInfoProvider userInfoProvider, IWebsiteChannelContext websiteChannelContext, IConfiguration configuration)
        {
            // Initializes instances of required services using dependency injection
            this.httpContextAccessor = HttpContextAccessor;
            this.userInfoProvider = userInfoProvider;
            this.websiteChannelContext = websiteChannelContext;
            this.configuration = configuration;
        }
        public IViewComponentResult Invoke()
        {
            if (User?.Identity?.IsAuthenticated == true || !string.IsNullOrEmpty(GetQueryStringValue("cnum")))
            {
                CurrentLoggedInUser user;

                //see if QS contains an ID
                if (!string.IsNullOrEmpty(GetQueryStringValue("cnum")))
                {
                    PersonID = GetQueryStringValue("cnum");

                    if (!string.IsNullOrEmpty(PersonID))
                    {
                        if (httpContextAccessor?.HttpContext?.Session.GetString("pid") != PersonID)
                        {
                            httpContextAccessor?.HttpContext?.Session.Clear();
                        }
                        httpContextAccessor?.HttpContext?.Session.SetString("pid", PersonID);
                    }

                    CurrentLoggedInUser newUser = new CurrentLoggedInUser();
                    newUser.NACSID = PersonID;

                    user = newUser;

                }
                else
                {
                    user = SetUser();
                }

                //Show override panel only to editors
                var userObj = userInfoProvider.Get(user.NACSID);
                UserIsTeamMember = userObj.IsInRole("NACSExhibitsTeam");
                UserIsEditor = userObj.HasAdministrationAccess();

                //_pnlTestOverrides.Visible = (UserIsTeamMember || UserIsEditor) ? true : false; //user must be assigned to site as well
                exhibitorApplication._pnlShowSelectExhibitor = false;
                exhibitorApplication._pnlShowEligibilityMessage = false;
                //_pnlClosedMessage.Visible = false;

                try
                {
                    PerformLogin();
                }
                catch (Exception ex)
                {
                    exhibitorApplication._pnlShowSelectExhibitor = false;
                    exhibitorApplication._pnlShowEligibilityMessage = false;
                    //_pnlClosedMessage.Visible = false;
                }
            }
            else
            {
                exhibitorApplication._lblStatus = "You must be logged in to view this page.";
            }

            return View();
        }

        public void PerformLogin()
        {
            string NACSID = PersonID;
            StringBuilder _lblMsg = new StringBuilder();
            //TODO: the text values below need to get fixed - _txtTestDate, _txtExpDate, _ddlTestTime
            //DateTime overridedate = (_txtTestDate.Text != "") ? Convert.ToDateTime(_txtTestDate.Text.Trim() + " " + _ddlTestTime.SelectedValue) : DateTime.Now;
            //DateTime overrideexpdate = (_txtExpDate.Text != "") ? Convert.ToDateTime(_txtExpDate.Text.Trim() + " " + DateTime.Now.ToShortTimeString()) : new DateTime(1900, 1, 1);

            try
            {
                List<NACSExhibitorApplicationUser> users = new List<NACSExhibitorApplicationUser>();

                if (NACSID != "")
                {
                    //TODO: Commented out due to above values needing fixed
                    //users = GetExhibitorApplicationUsers(NACSID, overridedate, overrideexpdate);
                }


                if (users.Count > 0)
                {
                    //TODO: commented out due to below value needing to be fixed
                    if (true)//_chkShowLoginData.Checked)
                    {
                        exhibitorApplication._pnlShowSelectExhibitor = false;
                        exhibitorApplication._pnlShowEligibilityMessage = true;
                        StringBuilder sb = new StringBuilder();

                        foreach (var user in users)
                        {

                            sb.Append("<h2>Login Data:</h2>");
                            sb.Append("<span style='font-size:0.8em'>");
                            Type type = typeof(NACSExhibitorApplicationUser);
                            PropertyInfo[] properties = type.GetProperties();
                            foreach (PropertyInfo property in properties)
                            {
                                sb.Append(property.Name + ":" + property.GetValue(user, null) + " | ");
                            }
                            sb.Append("</span>");
                            sb.Append("<br/><br/>");
                        }

                        _lblMsg.Append(sb);
                    }
                    else
                    {
                        //live mode - go ahead with process
                        if (users[0].StatusMessage == "Invalid")
                        {
                            exhibitorApplication._pnlShowSelectExhibitor = false;
                            exhibitorApplication._pnlShowEligibilityMessage = false;
                            exhibitorApplication._lblStatus = "<br/>Error: Invalid";
                        }
                        else
                        {
                            //TODO: This code is not able to be hit because of hardcoded true value above, but once that is fixed we need this working too and there are values that need to be fixed
                            //I commented out lines 215, 221-222, and 229-230 for now
                            if (users.Count > 1)
                            {
                                #region Handle Multiple
                                //if more than one result, user must choose company to use before continuing
                                exhibitorApplication._pnlShowSelectExhibitor = true;
                                exhibitorApplication._pnlShowEligibilityMessage = false;

                                foreach (var user in users)
                                {
                                    SelectListItem li = new SelectListItem();
                                    //li.Attributes.Add("onclick", "enableMoveNext('" + clientID + "__btnFinish','" + clientID.Replace("_", "$") + "$_btnFinish','" + clientID.Replace("_", "$") + "$_rblCompanies')");

                                    if (user.StatusMessage == "Eligible") //only allow eligible companies to be listed
                                    {
                                        li.Text = "<div style='margin-bottom:10px;display:inline'><strong>" + user.ExhibitorName + "</strong></div>";
                                        li.Value = urlMYS + UrlEncoder.Default.Encode(Encrypt(user.OrganizationId, encryptionKeyExhibitApps, false));
                                        //li.Enabled = true;
                                        //_rblCompanies.Items.Add(li);

                                    }
                                    else if (user.StatusMessage == "Ineligible")
                                    {
                                        li.Text = "<span style='color:#777'><strong>" + user.ExhibitorName + "</strong> (ineligible at this time)</span>";
                                        li.Value = user.OrganizationId;
                                        //li.Enabled = false;
                                        //_rblCompanies.Items.Add(li);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region Handle Single

                                DateTime expDate = (users[0].MemberExpireDate != null && users[0].MemberExpireDate.ToString() != "") ? Convert.ToDateTime(users[0].MemberExpireDate) : new DateTime(1900, 1, 1);
                                string pei = (users[0].PEIMemberType != null) ? users[0].PEIMemberType : "";
                                string mt = (users[0].MemberType != null) ? users[0].MemberType : "";

                                //returns "true" if membership is set to expire before the Show, or if it is already expired
                                bool expires = MembershipExpires(expDate, pei, mt);

                                //renewal urls
                                string subdomain = httpContextAccessor.HttpContext.Request.GetDisplayUrl().Contains("staging") ? "staging" : "www";
                                //get MX token and set Renewals URL
                                string mxtoken = GetProtechMXToken(NACSID);
                                string renewalurl = mxsite + "/My-Account/Company-Membership?token=" + mxtoken;

                                if (users[0].StatusMessage == "Eligible")
                                {
                                    //if only one result, send directly to MYS
                                    if (expires == true)
                                    {
                                        string option_url = renewalurl;
                                        string option_title = "Renew Membership";
                                        string option_description = "Renew your membership first, then move on to the application, to receive the member rate of $38.00 per square foot.";
                                        string option_button_text = "Renew Now";

                                        string option2_title = "Skip Renewal";
                                        string option2_description = "Skip the renewal process and reserve a booth at the non-member rate of $52.00 per square foot.";

                                        //never existed - new/joins
                                        if (expDate < new DateTime(1900, 1, 2))
                                        {
                                            if (DateTime.Today < new DateTime(2024, 05, 01))
                                            {
                                                _lblMsg.Append("<h2><i class='fas fa-info-circle fa-lg'></i>&nbsp;Before Selecting your booth...</h2>");
                                                _lblMsg.Append("<p>Your company is currently not a member of NACS. Non-member booth rates will apply.</p>");
                                                _lblMsg.Append("<p style='margin-left: 30px;'>");
                                                _lblMsg.Append("<strong>Company:</strong> <span style='color:#004c97;font-weight:600;'>" + users[0].ExhibitorName + "</span><br/>");
                                                _lblMsg.Append("</p><br/>");

                                                option_url = "https://" + subdomain + ".convenience.org/Membership/Supplier";
                                                option_title = "Join NACS";
                                                option_description = "Become a member of NACS to take advantage of member pricing.";
                                                option_button_text = "Join NACS";

                                                option2_title = "Skip Membership";
                                                option2_description = "Skip the membership application process and reserve a booth at the non-member rate of $51.00 per square foot.";
                                            }
                                            else
                                            {
                                                _lblMsg.Append("<h2><i class='fas fa-door-open'></i>&nbsp;" + users[0].StatusMessageHeader + "</h2>");
                                                _lblMsg.Append("<p>" + users[0].StatusMessageSubHeader + "</p>");
                                                _lblMsg.Append("<a class='button' style='width:auto;' href='" + urlMYS + UrlEncoder.Default.Encode(Encrypt(users[0].OrganizationId, encryptionKeyExhibitApps, false)) + "' target='_blank'>Launch Application <span class='fa fa-external-link-square'></a>");

                                                exhibitorApplication._pnlShowEligibilityMessage = true;

                                                //automatically redirect to MYS if not an editor or team member
                                                if (!UserIsTeamMember && !UserIsEditor)
                                                    SendToMYS(users[0].OrganizationId);
                                            }
                                        }
                                        //will be expiring soon, should renew
                                        else if (expDate > DateTime.Today)
                                        {
                                            _lblMsg.Append("<h2><i class='fas fa-info-circle fa-lg'></i>&nbsp;Membership Will Expire Soon</h2>");
                                            _lblMsg.Append("<p>Your membership is not current through the NACS Show. Non-member booth rates will apply.</p>");
                                            _lblMsg.Append("<p style='margin-left: 30px;'>");
                                            _lblMsg.Append("<strong>Company:</strong> <span style='color:#004c97;font-weight:600;'>" + users[0].ExhibitorName + "</span><br/>");
                                            _lblMsg.Append("<strong>Membership expires on:</strong> <span style='color:#004c97;font-weight:600;'>" + expDate.ToShortDateString() + "</span><br/>");
                                            _lblMsg.Append("</p><br/>");
                                        }
                                        //already expired - re-join
                                        else
                                        {
                                            _lblMsg.Append("<h2><i class='fas fa-info-circle fa-lg'></i>&nbsp;Membership Expired</h2>");
                                            _lblMsg.Append("<p>Your membership is not current through the NACS Show. Non-member booth rates will apply.</p>");
                                            _lblMsg.Append("<p style='margin-left: 30px;'>");
                                            _lblMsg.Append("<strong>Company:</strong> <span style='color:#004c97;font-weight:600;'>" + users[0].ExhibitorName + "</span><br/>");
                                            _lblMsg.Append("<strong>Membership expired on:</strong> <span style='color:#004c97;font-weight:600;'>" + expDate.ToShortDateString() + "</span> <br/>");
                                            _lblMsg.Append("</p><br/>");
                                        }

                                        _lblMsg.Append("<p><strong style='color:#004c97'>Your options to move forward:</strong></p>");
                                        _lblMsg.Append("<div class='row'>");
                                        _lblMsg.Append("<div class='col-12 col-sm-6'>");
                                        _lblMsg.Append("<span class='num'>A</span><strong style='color:#00aecc;font-size:1.4em'>" + option_title + "</strong>");
                                        _lblMsg.Append("<div style='margin-left:20px;'>");
                                        _lblMsg.Append(option_description);
                                        _lblMsg.Append("<br/><a class='button' href='" + option_url + "' target='_blank'>" + option_button_text + " <span class='fa fa-external-link-square'></a>");
                                        _lblMsg.Append("</div>");
                                        _lblMsg.Append("</div>");
                                        _lblMsg.Append("<div class='col-12 col-sm-6'>");
                                        _lblMsg.Append("<span class='num'>B</span><strong style='color:#00aecc;font-size:1.4em'>" + option2_title + "</strong>");
                                        _lblMsg.Append("<div style='margin-left:20px;'>");
                                        _lblMsg.Append(option2_description);
                                        _lblMsg.Append("<br/><a class='button' style='width:auto;' href='" + urlMYS + UrlEncoder.Default.Encode(Encrypt(users[0].OrganizationId, encryptionKeyExhibitApps, false)) + "' target='_blank'>Launch Application <span class='fa fa-external-link-square'></a>");
                                        _lblMsg.Append("</div>");
                                        _lblMsg.Append("</div>");
                                        _lblMsg.Append("</div>");

                                        exhibitorApplication._pnlShowSelectExhibitor = false;
                                        exhibitorApplication._pnlShowEligibilityMessage = true;
                                    }
                                    else
                                    {
                                        _lblMsg.Append("<h2><i class='fas fa-door-open'></i>&nbsp;" + users[0].StatusMessageHeader + "</h2>");
                                        _lblMsg.Append("<p>" + users[0].StatusMessageSubHeader + "</p>");
                                        _lblMsg.Append("<a class='button' style='width:auto;' href='" + urlMYS + UrlEncoder.Default.Encode(Encrypt(users[0].OrganizationId, encryptionKeyExhibitApps, false)) + "' target='_blank'>Launch Application <span class='fa fa-external-link-square'></a>");

                                        exhibitorApplication._pnlShowEligibilityMessage = true;

                                        //automatically redirect to MYS if not an editor or team member
                                        if (!UserIsTeamMember && !UserIsEditor)
                                        {
                                            SendToMYS(users[0].OrganizationId);
                                        }
                                    }
                                }
                                else if (users[0].StatusMessage == "Ineligible")
                                {
                                    //exhibitorApplication._pnlShowSelectExhibitor.Visible = false;
                                    //_pnlEligibilityMessage.Visible = true;

                                    //TODO: Commented out following line because overridedate needs to be fixed above (see line 152)
                                    //TimeSpan timespan = Convert.ToDateTime(users[0].BracketOpenDate.ToString()) - Convert.ToDateTime(overridedate); //make EST
                                    string expdate = (users[0].MemberExpireDate != null && (users[0].MemberExpireDate.ToString() != "" && Convert.ToDateTime(users[0].MemberExpireDate.ToString()).ToShortDateString() != "1/1/1900")) ? Convert.ToDateTime(users[0].MemberExpireDate.ToString()).ToShortDateString() : "n/a";

                                    _lblMsg.Append("<h2><i class='fas fa-do-not-enter fa-lg'></i>&nbsp;" + users[0].StatusMessageHeader + "</h2>");
                                    _lblMsg.Append("<p>" + users[0].StatusMessageSubHeader + "</p>");
                                    _lblMsg.Append("<p><br/><strong>Company Name:</strong> <span style='color:#004c97;font-weight:600;'>" + users[0].ExhibitorName + "</span></p>");
                                    _lblMsg.Append("<p><strong>Membership Expiration Date:</strong> <span style='color:#004c97;font-weight:600;'>" + expdate + "</span>");


                                    int ppts = (users[0].PriorityPoints != "") ? Convert.ToInt32(users[0].PriorityPoints) : 0;

                                    //if it was an invalid company type
                                    if (users[0].StatusMessageDetails != "Invalid member type")
                                    {
                                        //if they have expired, and are not members, do not show priority points
                                        if (expires == true && expDate <= DateTime.Today)
                                        {
                                            _lblMsg.Append("<p><strong>Priority Points:</strong> <span style='color:#004c97;font-weight:600;'>" + ppts.ToString() + "</span></p>");

                                            if (ppts > 0)
                                            {
                                                string bracket = GetBracketOLD(ppts, "").ToShortDateString();
                                                string newbracket = "";

                                                if (expdate != "n/a")
                                                {
                                                    //find bracket date for new exhibitors
                                                    if (users[0].FirstTimeExhibitor)
                                                    {
                                                        if (ppts >= 11)
                                                        {
                                                            newbracket = new DateTime(2024, 01, 29, 9, 0, 0).ToShortDateString();
                                                        }
                                                        else if (ppts == 10)
                                                        {
                                                            newbracket = new DateTime(2024, 02, 26, 9, 0, 0).ToShortDateString();
                                                        }
                                                        else if (ppts <= 9)
                                                        {
                                                            newbracket = new DateTime(2024, 03, 04, 9, 0, 0).ToShortDateString();
                                                        }

                                                        _lblMsg.Append("<p style='/*margin-left:40px;*/color:#e0007a'><span class='fa fa-asterisk'></span>&nbsp;<strong>Note: </strong>If you renew your NACS membership, you can submit your application starting on " + newbracket + " for the New Exhibitor Area, or on " + bracket + " for the Main Area, instead of 5/6/2024.");
                                                    }
                                                    else
                                                    {
                                                        _lblMsg.Append("<p style='/*margin-left:40px;*/color:#e0007a'><span class='fa fa-asterisk'></span>&nbsp;<strong>Note: </strong>If you renew your NACS membership, you can submit your application starting on " + bracket + ", instead of 5/6/2024.");
                                                    }

                                                    _lblMsg.Append("<br/><a class='button' href='" + renewalurl + "' target='_blank'>Renew Now <span class='fa fa-external-link-square'></a></p>");
                                                }
                                            }
                                            else
                                            {
                                                _lblMsg.Append("<p style='/*margin-left:40px;*/color:#e0007a'><span class='fa fa-asterisk'></span>&nbsp;<strong>Note: </strong>Save time and money by renewing your NACS membership today, in advance of your bracket open date.");
                                                _lblMsg.Append("<br/><a class='button' href='" + renewalurl + "' target='_blank'>Renew Now <span class='fa fa-external-link-square'></a></p>");
                                            }

                                        }
                                        else
                                        {
                                            _lblMsg.Append("<p><strong>Priority Points:</strong> <span style='color:#004c97;font-weight:600;'>" + ppts.ToString() + "</span></p>");
                                            _lblMsg.Append("<p><strong>Priority Group Open Date:</strong> <span style='color:#004c97;font-weight:600;'>" + Convert.ToDateTime(users[0].BracketOpenDate.ToString()).ToShortDateString());
                                            _lblMsg.Append(" at " + Convert.ToDateTime(users[0].BracketOpenDate.ToString()).AddHours(1).ToShortTimeString() + " EST");
                                            _lblMsg.Append("</span> (");

                                            //TODO: Commented out the following lines because timespan value in line 361 above is commented until stuff above it is fixed. These need uncommented once the value is fixed
                                            //if (timespan.Days > 0)
                                            //    _lblMsg.Append(timespan.Days.ToString() + " day");
                                            //if (timespan.Days > 1)
                                            //    _lblMsg.Append("s");
                                            //if (timespan.Days > 0 && timespan.Hours > 0)
                                            //    _lblMsg.Append(", ");

                                            //if (timespan.Hours > 0)
                                            //    _lblMsg.Append(timespan.Hours.ToString() + " hr");
                                            //if (timespan.Hours > 1)
                                            //    _lblMsg.Append("s");
                                            //if (timespan.Hours > 0 && timespan.Minutes > 0)
                                            //    _lblMsg.Append(", ");

                                            //if (timespan.Hours == 0 && timespan.Days > 0 && timespan.Minutes > 0)
                                            //    _lblMsg.Append(", ");

                                            //if (timespan.Minutes > 0)
                                            //    _lblMsg.Append(timespan.Minutes.ToString() + " min");
                                            //if (timespan.Minutes > 1)
                                            //    _lblMsg.Append("s");

                                            _lblMsg.Append(" away)</p>");
                                        }

                                        _lblMsg.Append("</p><br/><br/>");
                                        _lblMsg.Append("<p><span class='fa fa-question-circle'></span>&nbsp;<strong>Questions: </strong>Please <a href='/Exhibit/ExhibitorSupportTeam' traget='_blank'>contact your NACS representative</a> for questions about joining NACS or PEI, or come back when your specified Priority Group opens.</p>");
                                    }

                                }
                                else if (users[0].StatusMessage == "Access Denied")
                                {
                                    exhibitorApplication._pnlShowSelectExhibitor = false;
                                    exhibitorApplication._pnlShowEligibilityMessage = true;

                                    _lblMsg.Append("<h2><i class='fas fa-lock-alt fa-lg'></i>&nbsp;" + users[0].StatusMessage + "</h2>" + users[0].StatusMessageDetails);
                                }
                                else if (users[0].StatusMessage == "Invalid")
                                {
                                    exhibitorApplication._pnlShowSelectExhibitor = false;
                                    exhibitorApplication._pnlShowEligibilityMessage = false;
                                }
                                else
                                {
                                    if (users[0].StatusMessage.StartsWith("ERROR"))
                                    {
                                        exhibitorApplication._pnlShowSelectExhibitor = false;
                                        exhibitorApplication._pnlShowEligibilityMessage = true;

                                        _lblMsg.Append("<h2><i class='fas fa-exclamation-circle'></i>&nbsp;" + users[0].StatusMessageHeader + "</h2>" + users[0].StatusMessageDetails);
                                    }
                                }

                                #endregion
                            }

                            exhibitorApplication._lblStatus = "";
                        }
                    }

                }
                else
                {
                    exhibitorApplication._pnlShowSelectExhibitor = false;
                    exhibitorApplication._pnlShowEligibilityMessage = false;
                    exhibitorApplication._lblStatus = "<br/>Error: No Companies Found";
                }
            }
            catch (Exception ex)
            {
                exhibitorApplication._pnlShowSelectExhibitor = false;
                exhibitorApplication._pnlShowEligibilityMessage = false;
                exhibitorApplication._lblStatus += "<br/>Error at PerformLogin(): " + ex.Message.ToString() + "<br/>Details: " + ex.StackTrace.ToString();
            }
        }

        private bool MembershipExpires(DateTime ExpireDate, string PEI, string MemberType)
        {
            bool expires = false;

            if (PEI != "" || MemberType.Contains("Hunter"))
            {
                expires = false; //let 'em through
            }
            else
            {
                if (ExpireDate < new DateTime(2024, 10, 31))
                {
                    expires = true;
                }
            }

            return expires;
        }

        //protected void _btnFinish_Click(object sender, EventArgs e)
        //{
        //    SendToMYS(_rblCompanies.SelectedValue);
        //}

        protected string SendToMYS(string OrganizationId)
        {
            string url = urlMYS + UrlEncoder.Default.Encode(Encrypt(OrganizationId, encryptionKeyExhibitApps, false));
            return url;
        }

        //private void _btnRenewNow_Click(object sender, EventArgs e)
        //{

        //    Response.Redirect(urlLogin, false);
        //}

        // BSM 11/2/2019
        // Added a local version of the API method so we can impersonate.
        // Copied the IsAllowedIn and GetBracket as well, from the API. Now it only calls here.
        // Also added iginition to use the SPS service
        private List<NACSExhibitorApplicationUser> GetExhibitorApplicationUsers(string IndividualId, DateTime TestDateOverride, DateTime ExpireDateOverride)
        {
            List<NACSExhibitorApplicationUser> exhibitors = new List<NACSExhibitorApplicationUser>();

            //var sps = new StoredProcedureService();
            //var results = sps.ExecuteStoredProcedure<NACSExhibitorApplicationUser>("client_nacs_api_exhibitorapplication_getallowedcompanies_2020", new { cst_recno = IndividualId, cst_key = "" }).ToList();

            ContactRepository conRepo = new ContactRepository();
            AccountRepository acctRepo = new AccountRepository();
            GeneralRepository genRepo = new GeneralRepository();


            var contact = conRepo.GetByNumber(IndividualId);


            if (contact != null)
            {
                List<string> accountids = new List<string>();

                //1. Get associated exhibitor records from previous year
                try
                {
                    var filters = new FetchFilter
                    {
                        FilterType = FilterTypeOperators.Or,
                        Conditions = new List<FilterCondition>
                    {
                        new FilterCondition("nacs_boothcontactprimary", contact.Id.ToString()),
                        new FilterCondition("nacs_boothcontactsecondary", contact.Id.ToString())
                    }
                    };

                    var exhibs = genRepo.GetAll<NACSExhibitor>(filters).Where(e => e.ExhibitName == "NACS Show 2023").ToList();

                    if (exhibs != null && exhibs.Count() > 0)
                    {
                        foreach (var e in exhibs)
                        {
                            accountids.Add(e.ExhibitingAccountId.ToString());

                            #region handle secondaries if needed
                            //var secondaries = e.SharedBooths;
                            //if (secondaries.Count > 0)
                            //{
                            //    foreach (var s in secondaries)
                            //    {
                            //        _lblStatus.Text += "<br/>&nbsp;&nbsp;&nbsp;Secondary found: " + s.ExhibitingAccountName.ToString() + " (" + s.ExhibitingAccountId.ToString() + ")";
                            //    }
                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        //_lblStatus.Text += "<br/>No Exhibitor found";
                    }
                }
                catch (Exception ex)
                {
                    //_lblStatus.Text += "<br/>Error fetching exhibitor records: " + ex.Message.ToString();
                }


                //2. Get company that this user is linked to
                accountids.Add(contact.ParentAccountId.ToString());
                var accounts = acctRepo.GetFromList<Account>(accountids);
                //var account = acctRepo.GetById(contact.ParentAccountId);

                if (accounts.Count() > 0)
                {
                    foreach (var account in accounts)
                    {
                        if (account != null)
                        {
                            //DEBUG
                            //_lblStatus.Text += "<br/>Account found: " + account.Id.ToString();

                            try
                            {

                                DateTime start = new DateTime(2024, 10, 31); //end of Show month
                                                                             //DateTime expire = (account.NACSPrimaryMembership.ExpirationDate != null) ? new DateTime(1900, 1, 1) : account.NACSPrimaryMembership.ExpirationDate;
                                DateTime expire = (account.NACSPrimaryMembership != null) ? account.NACSPrimaryMembership.ExpirationDate : new DateTime(1900, 1, 1);
                                //int points = (account.PriorityPoints != null && account.PriorityPoints.ToString() != "") ? Convert.ToInt32(account.PriorityPoints) : 0;
                                int points = 0;
                                if (account.PriorityPoints != null && account.PriorityPoints.ToString() != "")
                                { points = Convert.ToInt32(account.PriorityPoints); }


                                string MemberType = (account.NACSPrimaryMembership != null) ? account.NACSPrimaryMembership.MemberTypeName : "";

                                //if recently paid as Hunter Club, invoice may still output Supplier - Standars, so take secondary instead
                                if (account.NACSSecondaryMembership != null)
                                {
                                    if (MemberType.Contains("Supplier - Standard") && account.NACSSecondaryMembership.MemberTypeName.Contains("Hunter Club"))
                                    {
                                        MemberType = account.NACSSecondaryMembership.MemberTypeName;
                                    }
                                }


                                string PEIMemberType = (account.PEIMemberType != null) ? account.PEIMemberType : "";

                                bool FirstTimeExhibitor = false;

                                int recentExhs = 0;

                                if (account.ExhibitorRecords.Count > 0)
                                {
                                    foreach (var e in account.ExhibitorRecords)
                                    {
                                        int year = Convert.ToInt32(e.ExhibitName.Substring(e.ExhibitName.Length - 4));

                                        if (year >= (DateTime.Now.Year - 7))
                                        {
                                            recentExhs++;
                                        }
                                    }

                                    if (recentExhs > 0)
                                    { FirstTimeExhibitor = false; }
                                    else
                                    { FirstTimeExhibitor = true; }

                                }
                                else
                                {
                                    FirstTimeExhibitor = true;
                                }



                                var eligibility = IsAllowedIn(MemberType, PEIMemberType, points, start, expire, FirstTimeExhibitor, "N", TestDateOverride, ExpireDateOverride);

                                string bracket_type = "";

                                if (FirstTimeExhibitor)
                                { bracket_type = "new"; }
                                //else if (exhibitor.PastCBDExhibitor.ToString() == "Y")
                                //{ bracket_type = "cbd"; }

                                if (eligibility == null)
                                {
                                    exhibitors.Add(new NACSExhibitorApplicationUser()
                                    {
                                        StatusMessage = "Access Denied",
                                        StatusMessageDetails = "You do not have access to apply for exhibit space for this company."
                                    });
                                }
                                else
                                {
                                    if (eligibility.Allowed)
                                    {
                                        exhibitors.Add(new NACSExhibitorApplicationUser()
                                        {

                                            ExhibitorName = account.Name,
                                            IndividualId = IndividualId,
                                            IndividualKey = "",
                                            OrganizationId = account.AccountNumber.ToString(),
                                            OrganizationKey = account.Id.ToString(),
                                            PriorityPoints = account.PriorityPoints?.ToString() ?? string.Empty,
                                            PEIMemberType = account.PEIMemberType ?? string.Empty,
                                            MemberType = MemberType,
                                            MemberExpireDate = (ExpireDateOverride > new DateTime(1900, 1, 1)) ? ExpireDateOverride.ToString() : expire.ToString(),
                                            FirstTimeExhibitor = FirstTimeExhibitor,
                                            PastCBDExhibitor = false, //not used anymore
                                            BracketOpenDate = GetBracketOLD(points, bracket_type).ToString(),
                                            StatusMessage = "Eligible",
                                            StatusMessageDetails = eligibility.Reason,
                                            StatusMessageHeader = eligibility.Reason_Header,
                                            StatusMessageSubHeader = eligibility.Reason_Subheader

                                        });
                                    }
                                    else
                                    {
                                        exhibitors.Add(new NACSExhibitorApplicationUser()
                                        {
                                            ExhibitorName = account.Name,
                                            IndividualId = IndividualId,
                                            IndividualKey = "",
                                            OrganizationId = account.AccountNumber.ToString() ?? string.Empty,
                                            OrganizationKey = account.Id.ToString(),
                                            PEIMemberType = account.PEIMemberType ?? string.Empty,
                                            PriorityPoints = account.PriorityPoints.ToString() ?? string.Empty,
                                            MemberType = MemberType,
                                            MemberExpireDate = (ExpireDateOverride > new DateTime(1900, 1, 1)) ? ExpireDateOverride.ToString() : expire.ToString(),
                                            FirstTimeExhibitor = FirstTimeExhibitor,
                                            PastCBDExhibitor = false,//not used anymore
                                            BracketOpenDate = GetBracketOLD(points, bracket_type).ToString(),
                                            StatusMessage = "Ineligible",
                                            StatusMessageDetails = eligibility.Reason,
                                            StatusMessageHeader = eligibility.Reason_Header,
                                            StatusMessageSubHeader = eligibility.Reason_Subheader
                                        });
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                exhibitors.Add(new NACSExhibitorApplicationUser()
                                {
                                    StatusMessage = "Access Denied",
                                    StatusMessageDetails = "Error getting company details: " + ex.Message.ToString()
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                exhibitors.Add(new NACSExhibitorApplicationUser()
                {
                    StatusMessage = "Access Denied",
                    StatusMessageDetails = "You do not have access to apply for exhibit space for this company."
                });
            }

            return exhibitors;

        }

        //Hardcoded dates for 2022
        public Eligibility IsAllowedIn(string MemberType, string PEIMemberType, int PriorityPoints, DateTime StartDate, DateTime NACSMemberExpireDate, bool FirstTimeExhibitor, string PastCBDExhibitor, DateTime TestDateOverride, DateTime ExpireDateOverride)
        {
            Eligibility eligibility = new Eligibility();

            //DEBUG
            //_lblStatus.Text += "<br/>IsAllowedIn() function called";

            bool allowed = false;
            string reason = "";
            string header = "";
            string subheader = "";

            bool newexhibitor = FirstTimeExhibitor;// (FirstTimeExhibitor == "Y") ? true : false;
            bool pastcbdexhibitor = (PastCBDExhibitor == "Y") ? true : false;

            //LIVE DATE vs DEBUG DATE
            //DEBUG: 
            //_pnlTestOverrides.Controls.AddAt(0, new LiteralControl(TestDateOverride.ToShortDateString() + " " + TestDateOverride.ToShortTimeString()));
            DateTime today = (TestDateOverride > new DateTime(1900, 1, 1)) ? Convert.ToDateTime(TestDateOverride) : DateTime.Now; //if value of test override is set, use that. Otherwise use now.
            DateTime expiredate = (ExpireDateOverride > new DateTime(1900, 1, 1)) ? Convert.ToDateTime(ExpireDateOverride) : NACSMemberExpireDate;

            //Always allow Hunter and PEI-Premium
            if (MemberType.Contains("Hunter") || MemberType.Contains("Press - Premier") || PEIMemberType.Contains("Priority"))
            {
                allowed = true;
                reason = "Priority group";
                header = "Select Your Booth";
                subheader = "Priority Booth Selection is Open";
            }
            //Always block access by specific Member Types
            else if ((MemberType.Contains("Retail") && PriorityPoints < 1) || (MemberType.Contains("Press") && PriorityPoints < 1)
                || (MemberType.Contains("Association") && PriorityPoints < 1) || (MemberType.Contains("Advert") && PriorityPoints < 1) || (MemberType == "" && PriorityPoints < 1))
            {
                //non-NACS PEI members should be able to get in for New Exhibitor
                if (PEIMemberType.Contains("Standard") && newexhibitor == true)
                {
                    if (((PriorityPoints >= 11) && today >= new DateTime(2024, 01, 30, 9, 0, 0)) //Convert.ToDateTime("03/08/2022"))
                    || ((PriorityPoints == 10) && today >= new DateTime(2024, 02, 26, 9, 0, 0))
                    || ((PriorityPoints <= 9) && today >= new DateTime(2024, 03, 03, 9, 0, 0)))
                    {
                        allowed = true;
                        reason = "New exhibitor bracket open";
                        header = "Select Your Booth";
                        subheader = "New Exhibitor bracket is open!";
                    }
                    else if (PriorityPoints == 9999)
                    {
                        allowed = true;
                        reason = "Points override (9999)";
                        header = "Select Your Booth";
                        subheader = "";
                    }
                    else
                    {
                        allowed = false;
                        reason = "New exhibitor bracket not open";
                        header = "Bracket Not Open Yet";
                        subheader = "New Exhibitor bracket is not open yet.";
                    }
                }
                else
                {
                    if (today < Convert.ToDateTime("06/03/" + StartDate.Year.ToString())) //Non-member bracket date
                    {
                        allowed = false;
                        reason = "Non-member before 6/3";
                        header = "Bracket Not Open Yet";
                        subheader = "Non-Member entry is not open until June 5.";
                    }
                    else
                    {
                        allowed = true;
                        reason = "Bracket is open";
                        header = "Select Your Booth";
                        subheader = "";
                    }

                }
            }
            //If it is before June 5, do not let non-members in
            else if (today < Convert.ToDateTime("06/03/" + StartDate.Year.ToString())
                && (expiredate < Convert.ToDateTime("12/31/" + (StartDate.Year - 1).ToString()))
                && (PEIMemberType == "")
                )
            {
                if (PriorityPoints == 9999)
                {
                    allowed = true;
                    reason = "Points override (9999)";
                    header = "Select Your Booth";
                    subheader = "";
                }
                else
                {
                    allowed = false;
                    reason = "Non-member before 6/3";
                    header = "Bracket Not Open Yet";
                    subheader = "Non-Member entry is not open until June 3.";
                }
            }
            //new for new exhibitors starting in 2019
            else if (newexhibitor == true)
            {

                if (((PriorityPoints >= 11) && today >= new DateTime(2024, 01, 29, 9, 0, 0)) //Convert.ToDateTime("03/08/2022"))
                || ((PriorityPoints == 10) && today >= new DateTime(2024, 02, 26, 9, 0, 0))
                || ((PriorityPoints <= 9) && today >= new DateTime(2024, 03, 04, 9, 0, 0)))
                {
                    allowed = true;
                    reason = "New exhibitor bracket open";
                    header = "Select Your Booth";
                    subheader = "New Exhibitor bracket is open!";
                }
                else if (PriorityPoints == 9999)
                {
                    allowed = true;
                    reason = "Points override (9999)";
                    header = "Select Your Booth";
                    subheader = "";
                }
                else
                {
                    allowed = false;
                    reason = "New exhibitor bracket not open";
                    header = "Bracket Not Open Yet";
                    subheader = "New Exhibitor bracket is not open yet.";
                }
            }

            #region CBD specs - not valid for 2021 - commented out
            ////new for exhibitors 2019
            //else if (pastcbdexhibitor == true)
            //{
            //    if (((PriorityPoints >= 16) && today >= new DateTime(2021, 02, 24, 9, 0, 0))
            //    || ((PriorityPoints <= 15) && today >= new DateTime(2021, 03, 09, 9, 0, 0)))
            //    {
            //        allowed = true;
            //        reason = "CBD exhibitor bracket open";
            //        header = "Select Your Booth";
            //        subheader = "Past CBD Exhibitor bracket is open!";
            //    }
            //    else if (PriorityPoints == 9999)
            //    {
            //        allowed = true;
            //        reason = "Points override (9999)";
            //        header = "Select Your Booth";
            //        subheader = "";
            //    }
            //    else
            //    {
            //        allowed = false;
            //        reason = "Past CBD exhibitor bracket not open";
            //        header = "Bracket Not Open Yet";
            //        subheader = "Past CBD Exhibitor bracket is not open yet.";
            //    }
            //}
            #endregion
            //PP Override
            else if (PriorityPoints == 9999)
            {
                allowed = true;
                reason = "Points override (9999).";
                header = "Select Your Booth";
                subheader = "";
            }
            //Block access by Priority Point date brackets
            else
            {
                if (GetBracketOLD(PriorityPoints, "") > today)
                {
                    allowed = false;
                    reason = "Bracket is not open";
                    header = "Bracket Not Open Yet";
                    subheader = "Priority points bracket is not open yet.";
                }
                else
                {
                    allowed = true;
                    reason = "Bracket is open";
                    header = "Select Your Booth";
                    subheader = "Priority points bracket is open!";
                }
            }

            eligibility.Allowed = allowed;
            eligibility.Reason = reason;
            eligibility.Reason_Header = header;
            eligibility.Reason_Subheader = subheader;

            return eligibility;
        }

        //Hardcoded dates for 2023
        public DateTime GetBracketOLD(int points, string type)
        {
            DateTime bracket = new DateTime();
            int p = points;

            //new exhibitor brackets
            if (type == "new")
            {
                if (p >= 18)
                {
                    bracket = new DateTime(2023, 01, 30, 9, 0, 0);
                }
                else if (p <= 17 && p >= 10)
                {
                    bracket = new DateTime(2023, 03, 06, 9, 0, 0);
                }
                else if (p <= 9)
                {
                    bracket = new DateTime(2023, 04, 03, 9, 0, 0);
                }
            }
            //OLD CBD brackets from 2019
            ////cbd exhibitor brackets
            //else if (type == "cbd")
            //{
            //    if (p >= 16)
            //    {
            //        bracket = new DateTime(2020, 02, 24, 9, 0, 0); 
            //    }
            //    else if (p <= 15)
            //    {
            //        bracket = new DateTime(2020, 03, 09, 9, 0, 0); 
            //    }
            //}
            //everyone else
            else
            {
                if (p >= 325)
                {
                    bracket = new DateTime(2023, 01, 30, 9, 0, 0); // 325+      Jan 30
                }
                else if (p <= 324 && p >= 311)
                {
                    bracket = new DateTime(2023, 02, 13, 9, 0, 0); // 311-324   Feb 13 
                }
                else if (p <= 310 && p >= 296)
                {
                    bracket = new DateTime(2023, 02, 20, 9, 0, 0); // 296-310   Feb 20
                }
                else if (p <= 295 && p >= 254)
                {
                    bracket = new DateTime(2023, 03, 06, 9, 0, 0); // 254-295   Mar 6
                }
                else if (p <= 253 && p >= 220)
                {
                    bracket = new DateTime(2023, 03, 13, 9, 0, 0); // 220-253   Mar 13
                }
                else if (p <= 219 && p >= 190)
                {
                    bracket = new DateTime(2023, 03, 20, 9, 0, 0); // 190-219    Mar 20
                }
                else if (p <= 189 && p >= 150)
                {
                    bracket = new DateTime(2023, 03, 27, 9, 0, 0); // 150-189     Mar 27
                }
                else if (p <= 149 && p >= 122)
                {
                    bracket = new DateTime(2023, 04, 03, 9, 0, 0); // 122-149     Apr 3
                }
                else if (p <= 121 && p >= 96)
                {
                    bracket = new DateTime(2023, 04, 10, 9, 0, 0); // 96-121     Apr 10
                }
                else if (p <= 95 && p >= 80)
                {
                    bracket = new DateTime(2023, 04, 17, 9, 0, 0); // 80-95     Apr 17
                }
                else if (p <= 79 && p >= 56)
                {
                    bracket = new DateTime(2023, 04, 24, 9, 0, 0); // 56-79     Apr 24
                }
                else if (p <= 55 && p >= 31)
                {
                    bracket = new DateTime(2023, 05, 01, 9, 0, 0); // 31-55     May 1
                }
                else if (p <= 30 && p >= 19)
                {
                    bracket = new DateTime(2023, 05, 08, 9, 0, 0); // 19-30     May 8
                }
                else if (p <= 18)
                {
                    bracket = new DateTime(2023, 05, 15, 9, 0, 0); // 0-18      May 15 
                }
            }


            return bracket;
        }

        //New dates for 2024 - custom table obsolated from XBK
        //public DateTime GetBracket(int points, string type)
        //{
        //    DateTime bracket = new DateTime();
        //    int p = points;
        //    string bracket_type = (type.ToLower() == "new") ? "New Exhibitor" : "Regular";
        //    CustomTableItem item = null;

        //    DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo("customtable.ExhibitorPriorityPointBrackets");

        //    if (customTable != null)
        //    {
        //        item = CustomTableItemProvider.GetItems("customtable.ExhibitorPriorityPointBrackets")
        //                                                .WhereEquals("BracketType", bracket_type)
        //                                                .And()
        //                                                .WhereGreaterOrEquals("PointRange_Highest", points)
        //                                                .And()
        //                                                .WhereLessOrEquals("PointRange_Lowest", points)
        //                                                .FirstOrDefault();

        //        if (item != null)
        //        {
        //            bracket = item.GetDateTimeValue("OpenDate", new DateTime(2024, 06, 03, 9, 0, 0));

        //            //subtract 1 hour to accomodate server time
        //            bracket = bracket.AddHours(-1);
        //        }
        //        else
        //        {
        //            bracket = new DateTime(2000, 12, 31, 9, 0, 0);
        //        }
        //    }

        //    return bracket;
        //}

        //ENCRYPTION CODE---------------------------------------
        public static string Encrypt(string toEncrypt, string encryptionKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(encryptionKey);

            // Set the secret key for the tripleDES algorithm
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            // Transform the specified region of bytes array to resultArray
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            // Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            //return System.Web.HttpServerUtility.UrlTokenEncode(resultArray);

        }

        public static string Decrypt(string cipherString, string encryptionKey, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace(' ', '+'));
            //byte[] toEncryptArray = System.Web.HttpServerUtility.UrlTokenDecode(cipherString);

            if (useHashing)
            {
                // If hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptionKey));
                hashmd5.Clear();
            }
            else
            {
                // If hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(encryptionKey);
            }

            // Set the secret key for the tripleDES algorithm
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            // Return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        //HELPERS-----------------------------------------------

        private string GetQueryStringValue(string key)
        {
            string value = "";

            try
            {
                value = httpContextAccessor?.HttpContext?.Request?.Query[key].ToString() ?? string.Empty;
            }
            catch { value = ""; }

            return value;
        }

        private CurrentLoggedInUser GetLoggedInPerson()
        {
            CurrentLoggedInUser user = new CurrentLoggedInUser();

            if (CMS.Membership.MembershipContext.AuthenticatedUser != null)
            {
                user.NACSID = CMS.Membership.MembershipContext.AuthenticatedUser.UserName;
                user.DisplayName = CMS.Membership.MembershipContext.AuthenticatedUser.FullName;
            }

            return user;
        }

        private string GetLoggedInPersonKey(string ID)
        {
            string custkey = "";

            try
            {
                NACSAPICustomerSoapClient service = new NACSAPICustomerSoapClient();
                NACS.Helper.CustomerService.NACSIndividual ind = service.Individual_GetById(ID, string.Empty, configuration["NACSAPIKey"]);
                custkey = ind.IndividualKey;
            }
            catch
            {
                custkey = "";
            }

            return custkey;
        }

        private class NACSExhibitorApplicationUser
        {

            public string IndividualKey { get; set; } = string.Empty;
            public string IndividualId { get; set; } = string.Empty;

            [NetForumField("cst_key")]
            public string OrganizationKey { get; set; } = string.Empty;

            [NetForumField("cst_recno")]
            public string OrganizationId { get; set; } = string.Empty;

            [NetForumField("exh_mys_id_ext")]
            public string MYSId { get; set; } = string.Empty;

            [NetForumField("cst_org_name_dn")]
            public string ExhibitorName { get; set; } = string.Empty;  

            [NetForumField("org_pei_member_type_ext")]
            public string PEIMemberType { get; set; } = string.Empty;

            [NetForumField("org_points_current_ext")]
            public string PriorityPoints { get; set; } = string.Empty;

            [NetForumField("OrgMemberType")]
            public string MemberType { get; set; } = string.Empty;

            [NetForumField("MbrExpireDate")]
            public string MemberExpireDate { get; set; } = string.Empty;

            [NetForumField("FirstTimeExhibitor")]
            public bool FirstTimeExhibitor { get; set; }

            [NetForumField("PastCBDExhibitor")]
            public bool PastCBDExhibitor { get; set; }

            public string BracketOpenDate { get; set; } = string.Empty;

            public string StatusMessage { get; set; } = string.Empty;

            public string StatusMessageDetails { get; set; } = string.Empty;

            public string StatusMessageHeader { get; set; } = string.Empty;
            public string StatusMessageSubHeader { get; set; } = string.Empty;
        }

        private CurrentLoggedInUser SetUser()
        {
            CurrentLoggedInUser user = GetLoggedInPerson();

            //PersonID = _txtNACSID.Text;

            if (PersonID != "")
            {
               
                if (HttpContext.Session.GetString("pid") != PersonID)
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("pid",PersonID);
                }
            }
            else
            {
                PersonID = user.NACSID;
                HttpContext.Session.SetString("pid", PersonID);
            }

            return user;
        }

        //protected void _btnSubmitClear_Click(object sender, EventArgs e)
        //{
        //    _txtTestDate.Text = string.Empty;
        //    _txtNACSID.Text = string.Empty;
        //    _txtExpDate.Text = string.Empty;

        //    CurrentLoggedInUser user = SetUser();

        //    //reload page
        //    Response.Redirect("Exhibitor-Application");
        //}

        private string GetProtechMXToken(string ProtechNumber)
        {
            NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();

            NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, configuration["NACSAPIKey"]);

            return serviceUser.Token.ToString();

        }

        //protected void _btnSubmit_Click(object sender, EventArgs e)
        //{
        //    _rblCompanies.Items.Clear();
        //    _lblStatus.Text = "";

        //    PerformLogin();
        //}
    }
}
