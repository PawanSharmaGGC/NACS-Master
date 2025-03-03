using CMS.Core;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Websites.Routing;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using NACS.Helper.AuthService;
using NACS.Helper.CustomerService;
using NACS.Protech.Entities;
using NACS.Protech.Framework;

using NACS_Classes;

using NACSShow.Components.Widgets.ExhibitorPortal;

using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

[assembly: RegisterWidget( ExhibitorPortalViewComponent.IDENTIFIER, typeof(ExhibitorPortalViewComponent), "Exhibitor Portal", typeof(ExhibitorPortalProperties), Description = "Widget for Exhibitor Portal")]

namespace NACSShow.Components.Widgets.ExhibitorPortal
{
    public class CurrentLoggedInUser
    {
        public string NACSID { get; set; }
        public string DisplayName { get; set; }

    }

    public class DirectoryListingSegment2
    {
        public string SegmentName { get; set; }
        public double Rating { get; set; }
        public bool Completed { get; set; }
    }


    public class ExhibitorPortalViewComponent : ViewComponent
    {
        protected string CustomerKey
        {
            get
            {
                return CMS.Membership.MembershipContext.AuthenticatedUser.GetStringValue("ProtechId", "");
            }
        }
        protected string NACSAPIKey = GetNACSAPIKey();
        public const string IDENTIFIER = "NACSShow.ExhibitorPortal";
        public CultureInfo en_US = CultureInfo.CreateSpecificCulture("en-US");
        public string PersonID = "";
        public string MYSID = "";
        public string MYSURL = "";
        public string mxsite = "";
        public string mxtoken = "";
        private string connStringWeb = "";
        private string GemboxLicense = "";
        private bool IsAdmin;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserInfoProvider userInfoProvider;
        private readonly IWebsiteChannelContext websiteChannelContext;
        private readonly IConfiguration config;
        private readonly IEventLogService eventLogService;

        public ExhibitorPortalViewModel viewModel = new ExhibitorPortalViewModel();

        private static string GetNACSAPIKey()
        {
            return System.Configuration.ConfigurationManager.AppSettings["NACSAPIKey"] ?? string.Empty;
        }
        public ExhibitorPortalViewComponent(IHttpContextAccessor httpContextAccessor, IUserInfoProvider userInfoProvider, IWebsiteChannelContext websiteChannelContext, IConfiguration config, IEventLogService eventLogService)
        {
            // Initializes instances of required services using dependency injection
            this.httpContextAccessor = httpContextAccessor;
            this.userInfoProvider = userInfoProvider;
            this.websiteChannelContext = websiteChannelContext;
            this.config = config;
            this.eventLogService = eventLogService;
        }
        public IViewComponentResult Invoke(ExhibitorPortalProperties properties)
        {
            //var al = new GetAttendeeListForSale();
            //var test = al.GetAttendees();

            var connStringWeb = config.GetConnectionString("nacsonline");

            //Get exhibitor records that the logged in person is allowed to manage
            UserInfo userObj = MembershipContext.AuthenticatedUser;
            IsAdmin = userObj.IsAdministrator();
            GemboxLicense = "SN-2022Apr22-nMvh7jv0W6eZZoLvZbM/puxSSHSb2afUbABTv7FyWkTK8vYoYVHX7AR6YyH/kfgoJMaWS/ps6aIJDKWHcirGmZmggVg==A";// ConfigurationManager.AppSettings["Gembox.License"];
            CurrentLoggedInUser user = GetLoggedInPerson();

            //Show or hide panels based on settings in widget
            viewModel.PNLMYSDashboardVisible = properties.ShowPanel_MYSDashboard;
            viewModel.PNLRegistrationVisible = properties.ShowPanel_ExperientRegistration;
            viewModel.PNLHousingVisible = properties.ShowPanel_Housing;
            viewModel.PNLLeadRetrievalVisible = properties.ShowPanel_ExperientLeadRetrieval;
            viewModel.PNLServiceKitVisible = properties.ShowPanel_ExhibitorServiceKit;
            viewModel.PNLMarketingVisible = properties.ShowPanel_MarketingOrders;
            viewModel.PNLAmbassadorsVisible = properties.ShowPanel_Ambassadors;
            viewModel.PNLContractorsVisible = properties.ShowPanel_Contractors;
            viewModel.PNLAttendeeListsVisible = properties.ShowPanel_AttendeeLists;

            //Turn on Features based on webpart toggle settings
            viewModel.PNLMYSDashboardHolderVisible = (properties.Status_MYSDashboard == "open") ? true : false;
            viewModel.PNLRegistrationHolderVisible = (properties.Status_ExperientRegistration == "open") ? true : false;
            viewModel.PNLHousingHolderVisible = (properties.Status_Housing == "open") ? true : false;
            viewModel.PNLLeadRetrievalHolderVisible = (properties.Status_ExperientLeadRetrieval == "open") ? true : false;
            viewModel.PNLServiceKitHolderVisible = (properties.Status_ExhibitorServiceKit == "open") ? true : false;
            viewModel.PNLMarketingHolderVisible = (properties.Status_MarketingOrders == "open") ? true : false;
            viewModel.PNLAmbassadorHolderVisible = (properties.Status_Ambassadors == "open") ? true : false;
            viewModel.PNLContractorHolderVisible = (properties.Status_Contractors == "open") ? true : false;

            //Display Status Messages
            viewModel.LBLStatusMsg_Registration = properties.StatusMsg_ExperientRegistration;
            viewModel.LBLStatusMsg_Housing = properties.StatusMsg_Housing;
            viewModel.LBLStatusMsg_ServiceKit = properties.StatusMsg_ExhibitorServiceKit;
            viewModel.LBLStatusMsg_Marketing = properties.StatusMsg_MarketingOrders;
            viewModel.LBLStatusMsg_Ambassadors = properties.StatusMsg_Ambassadors;
            viewModel.LBLStatusMsg_Contractors = properties.StatusMsg_Contractors;
            viewModel.LBLStatusMsg_AttendeeLists = properties.StatusMsg_AttendeeLists;


            //Display Descriptions HTML
            viewModel.LBLDescription_MYSDashboard = properties.Description_MYSDashboard;
            viewModel.LBLDescription_Registration = properties.Description_ExperientRegistration;
            viewModel.LBLDescription_Housing = properties.Description_Housing;
            viewModel.LBLDescription_LeadRetrieval = properties.Description_ExperientLeadRetrieval;
            viewModel.LBLDescription_ServiceKit = properties.Description_ExhibitorServiceKit;
            viewModel.LBLDescription_Marketing = properties.Description_MarketingOrders;
            viewModel.LBLDescription_Ambassadors = properties.Description_Ambassadors;
            viewModel.LBLDescription_Contractors = properties.Description_Contractors;
            viewModel.LBLDescription_AttendeeLists = properties.Description_AttendeeLists;

            //_pnlMYSDashboard.Controls.Add(new LiteralControl("Status: " + Status_MYSDashboard.ToString()));

            var domain = HttpContext.Request.Host.Value;
            MYSURL = (domain.Contains("staging") == true) ? properties.MapYourShowDashboardURL_Staging : properties.MapYourShowDashboardURL_Production;

            //check for user override in query string                
            //PersonID = NACSUtilities.GetQueryStringValue("CiD");

            if (PersonID != "")
            {
                if (httpContextAccessor.HttpContext.Session.GetString("pID") != PersonID)
                {
                    httpContextAccessor.HttpContext.Session.Clear();
                }
                httpContextAccessor.HttpContext.Session.SetString("pID", PersonID);

                //_lblPersonalMsg.Text = "<p>" + PersonID + " (override), </p>";
            }
            else
            {
                user = GetLoggedInPerson();
                httpContextAccessor.HttpContext.Session.Set("cUser", Encoding.ASCII.GetBytes(user.ToString()));
                PersonID = user.NACSID;
                httpContextAccessor.HttpContext.Session.SetString("pid", PersonID);
                //_lblPersonalMsg.Text = "<p>" + user.DisplayName + ", </p>";
            }

            //need to set for marketing form
            httpContextAccessor.HttpContext.Session.SetString("Exhibiting", "True");

            //MYSID = NACSUtilities.GetQueryStringValue("mid");

            if (MYSID != "")
            {
                httpContextAccessor.HttpContext.Session.SetString("MYSID", MYSID);
            }

            string orgkey = "";

            NACS.Helper.CustomerService.NACSIndividual drUser = new NACS.Helper.CustomerService.NACSIndividual();

            try
            {
                drUser = GetLoggedInPerson_FullRecord(PersonID);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='float:left;display:inline-block;width:70px;padding:0px 10px;'><span class='fal fa-exclamation-circle fa-2x'></span></div>");
                sb.Append(ex.Message.ToString());
                sb.Append("</div>");

                //_lblErrorText = sb.ToString();

                // _lblError.Visible = true;
            }

            if (drUser != null)
            {
                if (drUser.OrganizationId != "0")
                {
                    orgkey = drUser.ParentProtechId;
                }
                else
                {
                    //ExhibitorPortalContainer.Visible = false;
                    //_lblPersonalMsg.Visible = false;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style='float:left;display:inline-block;width:70px;padding:0px 10px;'><span class='fal fa-exclamation-circle fa-2x'></span></div>");
                    sb.Append("<div style='float:left;display:inline-block;'>");
                    sb.Append("     <p>You are not currently authorized to use this portal because you are not linked to any exhibiting company.");
                    sb.Append("     <br /><br />Please <a href='/Exhibit'>contact your NACS Account Manager</a> for assistance.</p>");
                    sb.Append("</div>");

                    //_lblError.Text = sb.ToString();

                    //_lblError.Visible = true;
                }
            }

            //Get list of allowed booth records
            NACSExhibitor_Portal[] _dtAllowedBooths = GetAllowedBoothRecords(PersonID, "simple", IsAdmin);

            if (_dtAllowedBooths != null && _dtAllowedBooths.Length > 0)
            {
                if (_dtAllowedBooths.Length > 1)
                {
                    viewModel.PNLExhibitorSelectionVisible = true;
                }
                else
                {
                    viewModel.PNLExhibitorSelectionVisible = false;
                }

                if (httpContextAccessor.HttpContext.Session.GetString("MYSID") == null)
                {
                    httpContextAccessor.HttpContext.Session.SetString("MYSID", _dtAllowedBooths[0].MYSId);
                }

                bool keyfound = false;
                var mID = _dtAllowedBooths[0].MYSId;

                //Get exhibitor
                NACSExhibitor_Portal dr = GetExhibitor(mID);
                var key = dr.ExhibitKey;

                if (dr != null)
                {
                    keyfound = true;

                    #region Populate MYS Dashboard Button

                    AccountRepository acctRepo = new AccountRepository();
                    var account = acctRepo.GetById(dr.OrganizationKey);
                    string cid = "";
                    if (account != null)
                    {
                        cid = account.PrimaryContactId.ToString();
                    }

                    string encryptedKey = Encrypt(dr.MYSId, false); //changed to MYSExhibitorKey in 2016 - BSM
                    string ContactId = (IsAdmin == true) ? dr.TSCId : CustomerKey.ToLower();
                    string AccountId = dr.OrganizationKey.ToLower();

                    viewModel.MapYourShowDashboardURL = string.Format("{0}?eid={1}&cid={2}&pid={3}", MYSURL, encryptedKey, AccountId, ContactId);

                    #endregion

                    #region Populate Registration Button

                    //Create Src code
                    string sourceCode = "";

                    if (httpContextAccessor.HttpContext.Session.GetString("QSNACSShowRegSourceCode") != null)
                    {
                        sourceCode = httpContextAccessor.HttpContext.Session.GetString("QSNACSShowRegSourceCode");
                    }
                    else
                    {
                        if (NACSUtilities.GetQueryStringValue("nsregsrc") != null)
                        {
                            sourceCode = NACSUtilities.GetQueryStringValue("nsregsrc");
                        }
                        else if (NACSUtilities.GetQueryStringValue("srccode") != null)
                        {
                            sourceCode = NACSUtilities.GetQueryStringValue("srccode");
                        }
                        else
                        {
                            sourceCode = "NACSShowExhibitorPortal";
                        }
                    }
                    viewModel.NACSShowRegURL = string.Format("{0}?exid={1}&srccode={2}", properties.NACSShowRegURL, dr.MYSId, sourceCode);

                    #endregion

                    #region Populate Housing Button

                    viewModel.NACSHousingURL = properties.NACSHousingURL;

                    #endregion

                    #region Populate Lead Retrieval Button

                    viewModel.NACSShowLeadRetrievalURL = properties.NACSShowLeadRetrievalURL;

                    #endregion

                    //load title for portal here - it's just convenient
                    //LBLExhibitorName = "Portal for: <span>" + dr.OrganizationName + " (booth " + dr.BoothNumber + ")</span>";

                    viewModel.ContractorsLink = string.Format("/Exhibit/Portal/AppointedContractors?ekey={0}", dr.MYSId);

                    viewModel.AmbassadorsLink = string.Format("/Exhibit/Portal/Ambassadors?mysid={0}", dr.MYSId);

                    viewModel.AmbassadorsSelfEmailLink = LoadAmbassadors(dr.MYSId);

                    orgkey = dr.OrganizationKey;

                    //LoadPurchasedProductsNEW(false, "", orgkey, 0);
                    //LoadHCDownloadedProductsNEW(false, "", orgkey);

                    //HIDDEN ON 6/15 BSM - UNHIDE WHEN READY TO OPEN FOR 2021
                    if (dr.ExhibitorMemberType.Contains("Hunter Club") || dr.ExhibitorPEIMemberType.Contains("Premium"))
                    {
                        //Free for HC members
                        //_pnlHCFreeLists.Visible = true;
                        //_pnlAttendeeLists_Form.Visible = false;
                    }
                    else if (dr.ExhibitorMemberType.Contains("Press"))
                    {
                        //Press cannot have attendee lists
                        //_pnlAttendeeLists.Visible = false;
                        //_pnlHCFreeLists.Visible = false;
                        //_pnlAttendeeLists_Form.Visible = false;
                    }
                    else
                    {
                        //show for NACS
                        if (dr.OrganizationId == "A-00020178") //A-00020178 = NACS Record Number
                        {
                            //_pnlHCFreeLists.Visible = true;
                            //_pnlAttendeeLists_Form.Visible = false;
                        }
                        else
                        {
                            //For Purchase
                            //_pnlHCFreeLists.Visible = false;
                            //_pnlAttendeeLists_Form.Visible = true;
                            //LoadAvailableProductsNEW();
                        }
                    }

                    string AmbassadorOverride = NACSUtilities.GetQueryStringValue("ao");

                    // _ddlSelectExhibitor.SelectedValue = mID;

                }

                if (keyfound == false)
                {
                    //_lblPersonalMsg.Text = "<p>Unfortunately, you do not have access to this exhibitor's portal."
                    //    + " Only tradeshow contacts and individuals associated with an exhibiting company may access their portal.</p>";
                    //_lblPersonalMsg.Visible = true;
                }
            }
            else
            {
                //ExhibitorPortalContainer.Visible = false;
                //_lblPersonalMsg.Visible = false;

                StringBuilder sb = new StringBuilder();
                sb.Append("<div style='float:left;display:inline-block;width:70px;padding:0px 10px;'><span class='fal fa-exclamation-circle fa-2x'></span></div>");
                sb.Append("<div style='float:left;display:inline-block;'>");
                sb.Append("     <p>Either your exhibitor record does not exist in our system or you are not currently authorized to use this portal.");
                sb.Append("     <br /><br />Please <a href='/Exhibit'>contact your NACS Account Manager</a> for assistance.</p>");
                sb.Append("     <p>If you are looking for the Exhibit Space Application, please visit:  <a href='/exhibitapp'>nacsshow.com/exhibitapp</p>");
                sb.Append("</div>");

                //_lblError.Text = sb.ToString();
                //_lblError.Visible = true;
            }

            return View("~/Components/Widgets/ExhibitorPortal/_ExhibitorPortal.cshtml", viewModel);
        }

        private CurrentLoggedInUser GetLoggedInPerson()
        {
            CurrentLoggedInUser user = new CurrentLoggedInUser();

            if (CMS.Membership.MembershipContext.AuthenticatedUser != null)
            {
                user.NACSID = CMS.Membership.MembershipContext.AuthenticatedUser.UserName; //this is the new ProtechNumber. Won't work yet
                                                                                           //user.NACSID = CMS.Membership.MembershipContext.AuthenticatedUser.GetStringValue("RecNo", "");
                user.DisplayName = CMS.Membership.MembershipContext.AuthenticatedUser.FullName;
            }

            return user;
        }

        private string GetProtechMXToken(string ProtechNumber)
        {
            var task = Task.Run(() =>
            {
                return GetProtechMXTokenFromAPI(ProtechNumber);
            });

            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(5000));

            if (isCompletedSuccessfully)
            {
                return task.Result;
            }
            else
            {
                return "";
            }
        }

        private string GetProtechMXTokenFromAPI(string ProtechNumber)
        {
            NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();
            string mxtoken = "";

            //BEGIN PROTECH API CALL (hide if API is having issues)-----------------------
            try
            {
                var savedtoken = httpContextAccessor.HttpContext.Session.GetString("NACSMXToken");

                if (savedtoken != null)
                {
                    mxtoken = savedtoken;
                }
                else
                {
                    //Get new token from API
                    NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, this.NACSAPIKey);
                    mxtoken = serviceUser.Token.ToString();

                    httpContextAccessor.HttpContext.Session.SetString("NACSMXToken", MYSID);
                }
            }
            catch (Exception ex)
            {
                eventLogService.LogException("Exhibitor Portal MX Auth Token", "ERROR GETTING TOKEN", ex);
            }
            //END PROTECH API CALL ----------------------------------------------

            return mxtoken;
        }

        //gets a list of all exhibitors that the individual can manage (either tradeshow contact, or member of the company)
        private NACSExhibitor_Portal[] GetAllowedBoothRecords(string personid, string mode, bool isadmin)
        {
            List<NACSExhibitor_Portal> exhibitors = new List<NACSExhibitor_Portal>();

            try
            {
                ContactRepository conRepo = new ContactRepository();
                AccountRepository acctRepo = new AccountRepository();
                GeneralRepository genRepo = new GeneralRepository();

                var contact = conRepo.GetByNumber(personid);

                if (contact != null)
                {
                    List<NACSExhibitor> exhibs = new List<NACSExhibitor>();
                    List<NACSExhibitor> exhibs22 = new List<NACSExhibitor>();

                    if (isadmin)
                    {
                        //Show all exhibitors for admin users
                        exhibs = genRepo.GetAll<NACSExhibitor>("nacs_exhibitname", "NACS Show 2024").OrderBy(e => e.ExhibitingName).ToList();
                        //_lblAdmin.Visible = true;
                    }
                    else
                    {
                        //_lblAdmin.Visible = false;
                        //Show only exhibitors that pertain to current user
                        var filter1 = new FetchFilter
                        {
                            FilterType = FilterTypeOperators.Or,
                            Conditions = new List<FilterCondition>
                        {
                            new FilterCondition("nacs_boothcontactprimary", contact.Id.ToString()), //where the current logged-in person is the primary contact
                            new FilterCondition("nacs_boothcontactsecondary", contact.Id.ToString()), //where the current logged-in person is the secondary contact
                            new FilterCondition("nacs_exhibitingaccount", contact.ParentAccountId.ToString()) //where the current logged-in person's company is exhibiting
                        }
                        };

                        var filter2 = new FetchFilter
                        {
                            FilterType = FilterTypeOperators.And,
                            Conditions = new List<FilterCondition>
                        {
                            new FilterCondition("nacs_exhibitname", "NACS Show 2024"), //where exhibitname is current Show
                        }
                        };

                        List<FetchFilter> filters = new List<FetchFilter>();
                        filters.Add(filter2);
                        filters.Add(filter1);

                        exhibs = genRepo.GetAll<NACSExhibitor>(filters).ToList().OrderBy(e => e.ExhibitingName).ToList();

                    }

                    if (exhibs != null && exhibs.Count() > 0)
                    {
                        foreach (var exhib in exhibs)
                        {
                            try
                            {
                                if (mode == "simple")
                                {
                                    exhibitors.Add(new NACSExhibitor_Portal()
                                    {
                                        BoothNumber = exhib.BoothNumber,
                                        DirectoryName = exhib.ExhibitingName,
                                        MYSId = exhib.ExternalId
                                    });
                                }
                                else
                                {

                                    var account = acctRepo.GetById(exhib.ExhibitingAccountId);
                                    string mtype = "";
                                    string peimtype = "";
                                    bool newExh = false;

                                    if (account != null)
                                    {
                                        mtype = (account.NACSPrimaryMembership != null) ? account.NACSPrimaryMembership.MemberTypeName : "";
                                        newExh = account.ExhibitorRecords.Count == 0;
                                        peimtype = (account.PEIMemberType != null) ? account.PEIMemberType : "";
                                    }

                                    exhibitors.Add(new NACSExhibitor_Portal()
                                    {
                                        BoothNumber = exhib.BoothNumber,
                                        DirectoryName = exhib.ExhibitingName,
                                        ExhibitKey = exhib.ExhibitId.ToString(),
                                        ExhibitName = exhib.ExhibitName,
                                        ExhibitorId = exhib.ExhibitorNumber.ToString(),
                                        ExhibitorKey = exhib.Id.ToString(),
                                        ExhibitorMemberType = mtype,
                                        ExhibitorNew = newExh.ToString(),
                                        ExhibitorPEIMemberType = peimtype,
                                        ExhibitYear = exhib.ExhibitName.Substring(exhib.ExhibitName.Length - 4),
                                        MYSId = exhib.ExternalId,
                                        OrganizationId = exhib.ExhibitingAccountNumber,
                                        OrganizationKey = exhib.ExhibitingAccountId.ToString(),
                                        OrganizationName = exhib.ExhibitingAccountName,
                                        PrimaryProductArea = exhib.PrimaryProductArea,
                                        StatusMessage = "Success",
                                        StatusMessageDetails = ""
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                exhibitors.Add(new NACSExhibitor_Portal()
                                {
                                    StatusMessage = "ERROR",
                                    StatusMessageDetails = ex.Message.ToString() + " | STACK TRACE: " + ex.StackTrace.ToString()
                                });
                            }
                        }
                    }
                }

                return exhibitors.ToArray();

            }
            catch (Exception ex)
            {
                //_lblError.Text = "Problem getting Company IDs: " + ex.Message.ToString();
                //_lblError.Visible = true;

                return null;
            }
        }

        private NACSExhibitor_Portal GetExhibitor(string mysid)
        {
            NACSExhibitor_Portal outputExhibitor = new NACSExhibitor_Portal();

            if (ViewBag["SelectedExhibitor"] != null)
            {
                outputExhibitor = (NACSExhibitor_Portal)ViewBag["SelectedExhibitor"];
            }
            else
            {
                //Pull fresh
                GeneralRepository genRepo = new GeneralRepository();
                AccountRepository acctRepo = new AccountRepository();

                var filters = new FetchFilter
                {
                    FilterType = FilterTypeOperators.And,
                    Conditions = new List<FilterCondition>
                {
                    new FilterCondition("nacs_externalid", mysid), //where exhibit mysid is current
                    new FilterCondition("nacs_exhibitname", "NACS Show 2024"), //where exhibitname is current Show
                }
                };

                var exhibitor = genRepo.GetAll<NACSExhibitor>(filters).FirstOrDefault();

                if (exhibitor != null)
                {
                    var account = acctRepo.GetById(exhibitor.ExhibitingAccountId);
                    string mtype = "";
                    string peimtype = "";
                    bool newExh = false;

                    if (account != null)
                    {
                        mtype = (account.NACSPrimaryMembership != null) ? account.NACSPrimaryMembership.MemberTypeName : "";
                        newExh = account.ExhibitorRecords.Count == 0;
                        peimtype = (account.PEIMemberType != null) ? account.PEIMemberType : "";
                    }

                    outputExhibitor.BoothNumber = exhibitor.BoothNumber;
                    outputExhibitor.DirectoryName = exhibitor.ExhibitingName;
                    outputExhibitor.ExhibitKey = exhibitor.ExhibitId.ToString();
                    outputExhibitor.ExhibitName = exhibitor.ExhibitName;
                    outputExhibitor.ExhibitorId = exhibitor.ExhibitorNumber.ToString();
                    outputExhibitor.ExhibitorKey = exhibitor.Id.ToString();
                    outputExhibitor.ExhibitorMemberType = mtype;
                    outputExhibitor.ExhibitorNew = newExh.ToString();
                    outputExhibitor.ExhibitorPEIMemberType = peimtype;
                    outputExhibitor.ExhibitYear = exhibitor.ExhibitName.Substring(exhibitor.ExhibitName.Length - 4);
                    outputExhibitor.MYSId = exhibitor.ExternalId;
                    outputExhibitor.OrganizationId = exhibitor.ExhibitingAccountNumber;
                    outputExhibitor.OrganizationKey = exhibitor.ExhibitingAccountId.ToString();
                    outputExhibitor.OrganizationName = exhibitor.ExhibitingAccountName;
                    outputExhibitor.PrimaryProductArea = exhibitor.PrimaryProductArea;
                    outputExhibitor.TSCId = (exhibitor.PrimaryContact_Id == null) ? account.PrimaryContactId.ToString() : exhibitor.PrimaryContact_Id.ToString();
                    outputExhibitor.StatusMessage = "Success";
                    outputExhibitor.StatusMessageDetails = "";

                    ViewBag["SelectedExhibitor"] = outputExhibitor;
                }
            }

            return outputExhibitor;
        }

        // This method is no longer in use as per legacy webpart v3
        private void Marketing(string NACSID, string CompanyName)
        {
            DataTable _dtOrders = new DataTable();
            var connectionStr = config.GetConnectionString("nacsshowmarketing");
            string SqlStatement = "execute MarketingOrder_SelectOrders @NACSCompanyId='" + NACSID + "'";
            SqlConnection conn = new SqlConnection(connectionStr);
            SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, conn);
            _da.Fill(_dtOrders);
            if (conn.State == ConnectionState.Open) { conn.Close(); }

            _da.Dispose();
            conn.Dispose();
        }

        //Region: Attendee List Purchasing Functions
        #region Attendee List Purchasing Functions and Classes

        public class AttendeeListProduct
        {
            public string ProductName { get; set; }
            public string ProductType { get; set; }
            public int ExportsAllowed { get; set; }
            public string ProductId { get; set; }
            public string ProductCode { get; set; }
            public int ProductYear { get; set; }
            public string ProductDescription { get; set; }
            public string MemberPrice { get; set; }
            public string NonMemberPrice { get; set; }
            public int DataPullLimit { get; set; }
            public string AssociatedRole { get; set; }
            public string AssociatedProtectedContentId { get; set; }
            public DateTime PurchaseUntilDate { get; set; }


            public static List<AttendeeListProduct> GetAllProducts()
            {
                string url = UriHelper.Encode(new System.Uri(RequestContext.URL.AbsoluteUri));
                bool staging = url.ToLower().Contains("staging");

                var list = new List<AttendeeListProduct>();

                list.Add(new AttendeeListProduct()
                {
                    ProductName = "2023 NACS Show On-Demand Attendee List - Final",
                    ProductType = "Static",
                    ExportsAllowed = 1,
                    ProductId = (staging) ? "f10bcc2a-bb27-ee11-9966-0022482a4fa0" : "78182e87-e8ff-ed11-8f6e-0022482a4930",
                    ProductCode = "SHWATTLIST2023",
                    ProductYear = 2023,
                    ProductDescription = "Final list of all attendees registered for the 2023 NACS Show.",
                    MemberPrice = "$500.00",
                    NonMemberPrice = "$500.00",
                    AssociatedRole = "NACS Show Attendee List Buyer - 2023 - One",
                    AssociatedProtectedContentId = (staging) ? "48a53616-252b-ee11-bdf4-0022482a4225" : "bb2c1807-272b-ee11-bdf4-0022482a478a",
                    PurchaseUntilDate = new DateTime(2024, 10, 31)
                });
                list.Add(new AttendeeListProduct()
                {
                    ProductName = "2023 NACS Show Attendees - Multi-use Access",
                    ProductType = "On-Demand",
                    ExportsAllowed = 3,
                    ProductId = (staging) ? "51f6f960-bb27-ee11-9966-0022482a4fa0" : "96bf18b7-e8ff-ed11-8f6e-0022482a4930",
                    ProductCode = "SHWATTLISTMU2023",
                    ProductYear = 2023,
                    ProductDescription = "All attendees registered for the 2023 NACS Show as of list exported date. May be exported up to 3 times prior to December 31, 2023. Once exported, the files can be downloaded as many times as needed.",
                    MemberPrice = "$1000.00",
                    NonMemberPrice = "$1000.00",
                    AssociatedRole = "NACS Show Attendee List Buyer - 2023 - Multi",
                    AssociatedProtectedContentId = (staging) ? "2f20b4bb-252b-ee11-bdf4-0022482a4225" : "e60e14c8-262b-ee11-bdf4-0022482a478a",
                    PurchaseUntilDate = new DateTime(2023, 10, 31)
                });

                return list;
            }

        }

        private void LoadAvailableProductsNEW()
        {
            string CurrentURL = HttpUtility.UrlEncode(RequestContext.URL.AbsoluteUri);

            //Set store URL for Attendee Purchases------------------------------------------------------
            mxtoken = GetProtechMXToken(CMS.Membership.MembershipContext.AuthenticatedUser.UserName);

            if (CurrentURL.ToLower().Contains("staging"))
            {
                mxsite = "https://nacsstagednn1.pcbscloud.com";
            }
            else if (CurrentURL.ToLower().Contains("kentico") || CurrentURL.ToLower().Contains("dev"))
            {
                mxsite = "https://nacsstagednn1.pcbscloud.com";
            }
            else
            {
                mxsite = "https://mynacs.convenience.org";
            }


            _hypPurchaseListInStore.NavigateUrl = mxsite + "/Store/Product-Catalog/Product-Details?productid=%7BB5C08F02-7A32-EC11-B6E5-000D3A9D04A3%7D";

            var products = AttendeeListProduct.GetAllProducts();

            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<table class='products-table'>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<tr>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<th width='40%'>List</th>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<th width='10%' align='center' style='text-align:center'>Type</th>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<th width='10%' align='center' style='text-align:center'>Exports Allowed</th>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<th width='10%' align='center' style='text-align:center' style='text-align:center'>Downloads Allowed</th>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<th width='10%' align='center' style='text-align:center'>Purchase</th>"));
            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("</tr>"));

            foreach (var product in products)
            {
                if (product.PurchaseUntilDate > DateTime.Now)
                {
                    string price = (product.NonMemberPrice == null) ? "n/a" : product.NonMemberPrice.ToString();
                    string productText = "<strong>" + product.ProductName + "</strong><br/>";
                    productText += "<strong>Price: </strong>" + price + "<br/>";
                    productText += "<em>" + product.ProductDescription + "</em>";

                    bool final = (product.ProductName.ToLower().Contains("final")) ? true : false;
                    string exports = (final) ? "n/a" : product.ExportsAllowed.ToString();

                    HyperLink link = new HyperLink();
                    link.NavigateUrl = mxsite + "/Store/Product-Catalog/Product-Details?productid=%7B" + product.ProductId + "%7D&token=" + mxtoken;
                    link.CssClass = "link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue";
                    link.Text = product.NonMemberPrice;// + "&nbsp;<i class='fa fa-external-link-alt'></i>";
                    link.ToolTip = "Buy now in our store";
                    link.Target = "_blank";


                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<tr>"));
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<td valign='middle'><strong>" + product.ProductName + "</strong><br/>" + product.ProductDescription + "</td>"));
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<td valign='middle' align='center' style='text-align:center' nowrap>" + product.ProductType + "</td>"));
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<td valign='middle' align='center' style='text-align:center'>" + exports + "</td>"));
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<td valign='middle' align='center' style='text-align:center'>unlimited</td>"));

                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("<td valign='middle' align='center' style='text-align:center'>"));
                    _pnlPurchaseListInStore.Controls.Add(link);
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("</td>"));
                    _pnlPurchaseListInStore.Controls.Add(new LiteralControl("</tr>"));
                }
            }

            _pnlPurchaseListInStore.Controls.Add(new LiteralControl("</table>"));
        }

        private void LoadPurchasedProductsNEW(bool justadded, string pkey, string okey, int rows)
        {

            /* Attendee List Roles:
             * NACS Show Attendee List Buyer - 2021 - One
             * NACS Show Attendee List Buyer - 2021 - Multi
             * NACS Show Attendee List Buyer - 2021 - Final
             */

            var user = CMS.Membership.MembershipContext.AuthenticatedUser;
            GeneralRepository genRepo = new GeneralRepository();

            var ind = GetLoggedInPerson_FullRecord(user.GetStringValue("ProtechNumber", ""));

            string indkey = ind.ProtechId;
            string orgkey = okey;


            //Get available lists
            var products = AttendeeListProduct.GetAllProducts();

            bool atleastonepurchased = false;

            if (products != null && products.Count > 0)
            {

                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<table cellpadding='10' cellspacing='10' border='0' id='downloads'>"));

                int n = 0;
                int i = 0;

                foreach (var product in products)
                {
                    i++;
                    //Get product limit
                    int limit = product.ExportsAllowed;

                    var filter = new FetchFilter
                    {
                        Conditions = new List<FilterCondition>
                    {
                    new FilterCondition("nacs_purchasedbyaccountid", orgkey),
                    new FilterCondition("nacs_protectedcontentid", product.AssociatedProtectedContentId)
                    }
                    };

                    var purchases = genRepo.GetAll<NACS.Protech.Entities.NACSContentPermission>(filter);

                    int purchased = (purchases != null && purchases.Count > 0) ? purchases.Count : 0;

                    if (purchased > 0)
                    {
                        atleastonepurchased = true;
                        n++;

                        //Get lists downloaded by user
                        DataTable dtDownloads = GetDownloads(indkey, orgkey, product.ProductId);
                        int used = (dtDownloads != null && dtDownloads.Rows.Count > 0) ? dtDownloads.Rows.Count : 0;
                        string lastDownloaded = (dtDownloads != null && dtDownloads.Rows.Count > 0) ? dtDownloads.Rows[0]["DownloadDate"].ToString() : "never";

                        string year = product.ProductYear.ToString();
                        bool final = (product.ProductName.ToLower().Contains("final")) ? true : false;

                        //Get available
                        int available = (limit * purchased) - used;
                        string count_display = (available > 0) ? available.ToString() : "0";
                        string indcolor = (available > 0) ? "var(--color-dark-pastel-green)" : "#777";
                        string indborder = (available > 0) ? "2px" : "0px";

                        if (final)
                        {
                            count_display = "n/a";
                            indcolor = "#777";
                            indborder = "0px";
                        }


                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<tr style='/*background-color:#f7f7f7;*/border-top:1px solid #ccc;background-image:linear-gradient(180deg, #f0f0f0, #ffffff);font-weight:bold;color:#666'>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<td width='40%' valign='middle'>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl(product.ProductName));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</td>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<td width='20%' valign='middle' align='center' style='font-size:1.4em;color:" + indcolor + "'>"));
                        if (!final)
                        {
                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<span class='num' style='border:" + indborder + " solid " + indcolor + "'>" + count_display + "</span>&nbsp;exports left"));
                        }
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</td>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</tr>"));

                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<tr style='background-color:#ffffff'>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<td colspan='3' style='padding-bottom:30px'>"));

                        //if exports have been created, show them
                        if (used > 0)
                        {
                            int x = 0;
                            int cnt = dtDownloads.Rows.Count;

                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<ul class='fa-ul'>"));

                            foreach (DataRow drFile in dtDownloads.Rows)
                            {
                                if (drFile["Filename"].ToString() != "")
                                {

                                    int ord = cnt - x;
                                    x++;

                                    string filetype = (drFile["Filename"].ToString().Contains(".")) ? drFile["Filename"].ToString().Split('.')[1] : "";
                                    string count = ((rows > 0) && (justadded) && (pkey == product.ProductId) && (x == 1)) ? " (" + rows.ToString() + ")" : "";
                                    string fa_icon = (filetype == "xlsx") ? "fa fa-file-excel" : "fa fa-file-csv";

                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<li><span class='fa-li fa-lg' style='color:#419fd7'><i class='" + fa_icon + "'></i></span>"));
                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<strong>Export " + ord.ToString() + ": </strong>"));
                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<a href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("file=" + drFile["DownloadID"].ToString()));
                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));

                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("' onClick=\"return ValidateForm('Download_" + i.ToString() + "')\">Attendees as of " + drFile["DownloadDate"].ToString() + count + "</a>"));
                                    if ((justadded) && (pkey == product.ProductId) && (x == 1))
                                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<img title='New' class='ms-newgif' alt='New' src='/NACSShow/media/Images/new.gif' style='margin:5px;' align='absmiddle' />"));
                                    _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</li>"));

                                }
                            }
                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</ul>"));
                        }

                        if (available > 0)
                        {
                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<div>"));

                            if (final == true)
                            {
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<i class='fa fa-file-download fa-lg' style='color:#419fd7;margin:0px 10px'></i><strong style='color:/*#419fd7*/'>Download File: </strong>"));

                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<a "));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("file=" + year + "xlsx&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("' style='font-weight:normal;' onClick=\"return ValidateForm('Download_" + i.ToString() + "')\"><i class='fa fa-file-excel' style='margin-right:5px;'></i>Excel Format (XLSX)</a>"));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("&nbsp;&nbsp;or&nbsp;&nbsp;"));

                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<a "));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("file=" + year + "csv&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));

                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("' style='font-weight:normal' onClick=\"return ValidateForm('Download_" + i.ToString() + "')\"><i class='fa fa-file-csv' style='margin-right:5px;'></i>Comma-Delimited Format (CSV)</a>"));
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("&nbsp;"));

                            }
                            else
                            {

                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<i class='fas fa-asterisk fa-lg' style='color:#419fd7;margin:0px 10px'></i><strong style='color:#419fd7'>Create a New Export: </strong>"));

                                LinkButton _lbDownloadAttendeeListXLSX = new LinkButton();
                                _lbDownloadAttendeeListXLSX.Command += new CommandEventHandler(_lbDownloadAttendeeList_Click);
                                _lbDownloadAttendeeListXLSX.Text = "<i class='fa fa-file-excel'></i>&nbsp;&nbsp;XLSX";

                                _lbDownloadAttendeeListXLSX.CssClass = "link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue download";
                                _lbDownloadAttendeeListXLSX.Attributes.Add("style", "font-weight:bold;color:#fff;");
                                _lbDownloadAttendeeListXLSX.ID = "_lbDownloadAttendeeListXLSX_" + i.ToString();
                                _lbDownloadAttendeeListXLSX.CommandArgument = "xlsx|" + indkey + "|" + orgkey + "|" + product.ProductId + "|" + year + "|" + i.ToString();
                                _lbDownloadAttendeeListXLSX.ValidationGroup = "Download_" + i.ToString();
                                _pnlAttendeeLists_Purchased.Controls.Add(_lbDownloadAttendeeListXLSX);
                                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("&nbsp;"));

                                LinkButton _lbDownloadAttendeeListCSV = new LinkButton();
                                _lbDownloadAttendeeListCSV.Command += new CommandEventHandler(_lbDownloadAttendeeList_Click);
                                _lbDownloadAttendeeListCSV.Text = "<i class='fa fa-file-csv'></i>&nbsp;&nbsp;CSV";
                                _lbDownloadAttendeeListCSV.CssClass = "link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue download";
                                _lbDownloadAttendeeListCSV.Attributes.Add("style", "font-weight:bold;color:#fff;");
                                _lbDownloadAttendeeListCSV.ID = "_lbDownloadAttendeeListCSV_" + i.ToString();
                                _lbDownloadAttendeeListCSV.CommandArgument = "csv|" + indkey + "|" + orgkey + "|" + product.ProductId + "|" + year + "|" + i.ToString();
                                _lbDownloadAttendeeListCSV.ValidationGroup = "Download_" + i.ToString();
                                _pnlAttendeeLists_Purchased.Controls.Add(_lbDownloadAttendeeListCSV);
                            }

                            CheckBox _cbAgreeToTerms = new CheckBox();
                            _cbAgreeToTerms.Text = "I agree to never sell, distribute, share, or disseminate the downloaded list without written permission from NACS.";
                            _cbAgreeToTerms.ID = "_cbAgreeToTerms_" + i.ToString();
                            _cbAgreeToTerms.ValidationGroup = "Download_" + i.ToString();
                            _cbAgreeToTerms.CssClass = "agree-margin";


                            CustomValidator _valAgreeToTerms = new CustomValidator();
                            _valAgreeToTerms.ID = "_valAgreeToTerms_" + i.ToString();
                            _valAgreeToTerms.ClientValidationFunction = "ValidateAgreement";
                            _valAgreeToTerms.SetFocusOnError = true;
                            _valAgreeToTerms.ValidationGroup = "Download_" + i.ToString();
                            _valAgreeToTerms.Display = ValidatorDisplay.Dynamic;
                            _valAgreeToTerms.ErrorMessage = "<span class='error-msg agree-margin'><i class='fas fa-info-circle'></i>&nbsp;Please agree to terms by checking the box.</span>";
                            _valAgreeToTerms.Text = "<span class='error-msg agree-margin'><i class='fas fa-info-circle'></i>&nbsp;Please agree to terms by checking the box.</span>";

                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<br/>"));
                            _pnlAttendeeLists_Purchased.Controls.Add(_cbAgreeToTerms);
                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<br/>"));
                            _pnlAttendeeLists_Purchased.Controls.Add(_valAgreeToTerms);

                            _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</div>"));

                        }

                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</td>"));
                        _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</tr>"));
                    }

                }
                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</table>"));
            }

            if (!atleastonepurchased)
            {
                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("<table cellpadding='5' cellspacing='5' border='0' id='downloads'><tr><td valign='middle'>"));
                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("None Purchased"));
                _pnlAttendeeLists_Purchased.Controls.Add(new LiteralControl("</td></tr></table>"));
            }
        }

        //TBD : Dependent on GetAttendees
        protected void _lbDownloadAttendeeList_Click(object sender, EventArgs e)
        {

            try
            {
                string[] par = e.CommandArgument.ToString().Split('|');

                //Perform export
                int rows = ExportList(par[0], par[1], par[2], par[3], par[4]);

                //clear the old panel and reload to show new numbers.
                _pnlAttendeeLists_Purchased.Controls.Clear();
                LoadPurchasedProductsNEW(true, par[3], par[2], rows);

                //re-check the agreement box - its been checked already once.
                try
                {
                    CheckBox cb = (CheckBox)FindControlRecursive(_pnlAttendeeLists_Purchased, "_cbAgreeToTerms_" + par[5]);
                    cb.Checked = true;
                }
                catch { }

                //clear the old panel and reload to show new numbers.
                _pnlAttendeeLists_Free.Controls.Clear();
                LoadHCDownloadedProductsNEW(true, par[3], par[2]);

                try
                {
                    CheckBox cb2 = (CheckBox)FindControlRecursive(_pnlAttendeeLists_Free, "_cbAgreeToTermsHC_" + par[5]);
                    cb2.Checked = true;
                }
                catch { }

                //Reload Products For Purchase
                LoadAvailableProductsNEW();

            }
            catch (Exception ex)
            {
                _pnlAttendeeLists_Purchased.Controls.AddAt(0, new LiteralControl(ex.Message.ToString()));
            }

        }

        private void LoadHCDownloadedProductsNEW(bool justadded, string pkey, string orgkey)
        {

            var user = CMS.Membership.MembershipContext.AuthenticatedUser;
            string indkey = user.GetStringValue("ProtechId", "");

            //Get available lists
            var products = AttendeeListProduct.GetAllProducts();

            if (products != null && products.Count > 0)
            {
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<table cellpadding='10' cellspacing='10' border='0' id='downloads'>"));
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<tr>"));
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<td width='35%' align='left' valign='bottom' class='portalheader-in-table' style='font-size:1.4em;border:0px;color:#1b59a6'>List Name</td>"));
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<td width='65%' valign='bottom' class='portalheader-in-table' style='font-size:1.4em;border:0px;color:#1b59a6'>Downloads</td>"));
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</tr>"));

                int i = 0;

                foreach (var product in products)
                {
                    i++;

                    if (product.ProductCode != "SHWATTLIST2023" || product.ProductName.ToLower().Contains("final")) //take out one-time use but leave final - not applicable
                    {
                        Get lists downloaded by user
                        DataTable dtDownloads = GetDownloads(indkey, orgkey, product.ProductId);
                        int used = (dtDownloads != null && dtDownloads.Rows.Count > 0) ? dtDownloads.Rows.Count : 0;
                        string lastDownloaded = (dtDownloads != null && dtDownloads.Rows.Count > 0) ? dtDownloads.Rows[0]["DownloadDate"].ToString() : "never";

                        string prod_name = product.ProductName;
                        string year = product.ProductYear.ToString();
                        string align = "middle";
                        bool final = (product.ProductName.ToLower().Contains("final")) ? true : false;

                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<tr style='background-color:#f7f7f7;border-top:3px solid #f0f0f0;border-bottom:2px solid #ffffff'>"));
                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<td width='35%' valign='" + align + "' >"));
                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl(prod_name + "<br/><em style='font-size:0.8em'>" + product.ProductDescription.Split('.')[0] + ".</em>"));
                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</td>"));

                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<td width='65%'"));

                        if (final == true)
                        {
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl(" valign='middle'>"));

                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<div><a class='' style='color:#419fd7 !important' "));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("file=" + year + "xlsx&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("' style='font-weight:bold;color:#fff;' onClick=\"return ValidateFormHC('DownloadHC_" + i.ToString() + "')\"><i class='fa fa-arrow-alt-to-bottom'></i>&nbsp;Download XLSX</a>"));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<a class='' style='color:#419fd7 !important' "));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("file=" + year + "csv&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("' style='font-weight:bold;color:#fff' onClick=\"return ValidateFormHC('DownloadHC_" + i.ToString() + "')\"><i class='fa fa-arrow-alt-to-bottom'></i>&nbsp;Download CSV</a>"));

                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</div>"));
                        }
                        else
                        {
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl(" valign='top'>"));

                            if (used > 0)
                            {
                                int x = 0;
                                int cnt = dtDownloads.Rows.Count;

                                foreach (DataRow drFile in dtDownloads.Rows)
                                {
                                    if (drFile["Filename"].ToString() != "")
                                    {

                                        int ord = cnt - x;
                                        x++;

                                        string filetype = (drFile["Filename"].ToString().Contains(".")) ? drFile["Filename"].ToString().Split('.')[1] : "";

                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<div style='background-color:#fff'>"));
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("Export " + ord.ToString() + ": "));
                                        if (filetype == "xlsx")
                                        {
                                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<i class='fa fa-file-excel'></i>&nbsp;"));
                                        }
                                        else
                                        {
                                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<i class='fa fa-file-csv'></i>&nbsp;"));
                                        }
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<a href='/Convenience.org/ApplicationPages/ExportAttendeeList.aspx?"));
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("file=" + drFile["DownloadID"].ToString()));
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("&ikey=" + indkey + "&okey=" + orgkey + "&pkey=" + product.ProductId));
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("' onClick=\"return ValidateFormHC('DownloadHC_" + i.ToString() + "')\">Attendees as of " + drFile["DownloadDate"].ToString() + "</a>"));
                                        if ((justadded) && (pkey == product.ProductId) && (x == 1))
                                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<img title='New' class='ms-newgif' alt='New' src='/NACSShow/media/Images/new.gif' style='margin:5px;' align='absmiddle' />"));
                                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</div><br>"));
                                    }
                                }
                            }

                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("Create a New Export: "));

                            LinkButton _lbDownloadAttendeeListXLSX = new LinkButton();
                            _lbDownloadAttendeeListXLSX.Command += new CommandEventHandler(_lbDownloadAttendeeList_Click);
                            _lbDownloadAttendeeListXLSX.Text = "<i class='fa fa-file-excel'></i>&nbsp;&nbsp;XLSX";
                            _lbDownloadAttendeeListXLSX.CssClass = "link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue download";
                            _lbDownloadAttendeeListXLSX.Attributes.Add("style", "font-weight:bold;color:#fff;");
                            _lbDownloadAttendeeListXLSX.ID = "_lbDownloadHCAttendeeListXLSX_" + i.ToString();
                            _lbDownloadAttendeeListXLSX.CommandArgument = "xlsx|" + indkey + "|" + orgkey + "|" + product.ProductId + "|" + year + "|" + i.ToString();
                            _lbDownloadAttendeeListXLSX.ValidationGroup = "DownloadHC_" + i.ToString();
                            _pnlAttendeeLists_Free.Controls.Add(_lbDownloadAttendeeListXLSX);
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("&nbsp;"));



                            LinkButton _lbDownloadAttendeeListCSV = new LinkButton();
                            _lbDownloadAttendeeListCSV.Command += new CommandEventHandler(_lbDownloadAttendeeList_Click);
                            _lbDownloadAttendeeListCSV.Text = "<i class='fa fa-file-csv'></i>&nbsp;&nbsp;CSV";

                            _lbDownloadAttendeeListCSV.CssClass = "link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue download";
                            _lbDownloadAttendeeListCSV.Attributes.Add("style", "font-weight:bold;color:#fff;");
                            _lbDownloadAttendeeListCSV.ID = "_lbDownloadHCAttendeeListCSV_" + i.ToString();
                            _lbDownloadAttendeeListCSV.CommandArgument = "csv|" + indkey + "|" + orgkey + "|" + product.ProductId + "|" + year + "|" + i.ToString();
                            _lbDownloadAttendeeListCSV.ValidationGroup = "DownloadHC_" + i.ToString();
                            _pnlAttendeeLists_Free.Controls.Add(_lbDownloadAttendeeListCSV);
                            _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("&nbsp;"));
                        }

                        CheckBox _cbAgreeToTerms = new CheckBox();
                        _cbAgreeToTerms.Text = "I agree to never sell, distribute, share, or disseminate the downloaded list without written permission from NACS.";
                        _cbAgreeToTerms.ID = "_cbAgreeToTermsHC_" + i.ToString();
                        _cbAgreeToTerms.ValidationGroup = "DownloadHC_" + i.ToString();
                        _cbAgreeToTerms.CssClass = " agree-margin";

                        CustomValidator _valAgreeToTerms = new CustomValidator();
                        _valAgreeToTerms.ID = "_valAgreeToTermsHC_" + i.ToString();
                        _valAgreeToTerms.ClientValidationFunction = "ValidateAgreementHC";
                        _valAgreeToTerms.SetFocusOnError = true;
                        _valAgreeToTerms.ValidationGroup = "DownloadHC_" + i.ToString();
                        _valAgreeToTerms.Display = ValidatorDisplay.Dynamic;
                        _valAgreeToTerms.ErrorMessage = "<span class='error-msg agree-margin'><i class='fas fa-info-circle'></i>&nbsp;Please agree to terms by checking the box.</span>";
                        _valAgreeToTerms.Text = "<span class='error-msg agree-margin'><i class='fas fa-info-circle'></i>&nbsp;Please agree to terms by checking the box.</span>";

                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<br/>"));
                        _pnlAttendeeLists_Free.Controls.Add(_cbAgreeToTerms);
                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("<br/>"));
                        _pnlAttendeeLists_Free.Controls.Add(_valAgreeToTerms);

                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</td>"));
                        _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</tr>"));
                    }
                }
                _pnlAttendeeLists_Free.Controls.Add(new LiteralControl("</table>"));
            }

        }

        //TBD data need to be fetched from smart search index
        //private List<NACSAttendeeForSale> GetAttendees(string yr)
        //{
        //    List<NACSAttendeeForSale> attendees = new List<NACSAttendeeForSale>();

        //    #region Kentico Search Index

        //    SearchIndexInfo index = SearchIndexInfoProvider.GetSearchIndexInfo("NACSShowAttendeeDirectoryData");

        //    if (index != null)
        //    {
        //        // Prepares the search parameters
        //        var condition = new SearchCondition(null, SearchModeEnum.AnyWord, SearchOptionsEnum.FullSearch);
        //        string searchText = "*:*";

        //        SearchParameters parameters = new SearchParameters();

        //        try
        //        {
        //            parameters.SearchFor = searchText;
        //            parameters.SearchSort = "RegistrantLastName";// "##SCORE##";//;
        //            parameters.Path = "/%";
        //            parameters.CurrentCulture = "EN-US";
        //            parameters.DefaultCulture = CultureHelper.EnglishCulture.IetfLanguageTag;
        //            parameters.CombineWithDefaultCulture = false;
        //            parameters.CheckPermissions = false;
        //            parameters.SearchInAttachments = false;
        //            parameters.User = (UserInfo)MembershipContext.AuthenticatedUser;
        //            parameters.SearchIndexes = index.IndexName;
        //            parameters.StartingPosition = 0;
        //            parameters.DisplayResults = 50000;
        //            parameters.NumberOfProcessedResults = 50000;
        //            parameters.NumberOfResults = 50000;
        //            parameters.AttachmentWhere = String.Empty;
        //            parameters.AttachmentOrderBy = String.Empty;
        //            parameters.ClassNames = "";

        //        }
        //        catch (Exception ex)
        //        {
        //            _lblMsg.Text = "Error combining parameters: " + ex.Message.ToString();
        //        }

        //        try
        //        {
        //            SearchResult results = SearchHelper.Search(parameters);

        //            if (parameters.NumberOfResults > 0)
        //            {
        //                TransformationHelper help = new TransformationHelper();
        //                int n = 0;

        //                foreach (SearchResultItem result in results.Items)
        //                {
        //                    n++;
        //                    string badgename = (string)help.GetSearchValue(result, "RegistrantBadgeName");
        //                    string firstname = (string)help.GetSearchValue(result, "RegistrantFirstName");
        //                    string RegTypeCode = (string)help.GetSearchValue(result, "RegistrationType");
        //                    string BusinessType = (string)help.GetSearchValue(result, "BusinessType");
        //                    string FinalRegType = "";

        //                    if (BusinessType == "State Association")
        //                    {
        //                        if (RegTypeCode == "FULL" || RegTypeCode == "ASEL")
        //                        {
        //                            FinalRegType = "Retailer/Fuel Marketer";
        //                        }
        //                        else //if (RegTypeCode == "ASEC" || RegTypeCode == "ASSP")
        //                        {
        //                            FinalRegType = "State Association";
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (RegTypeCode == "XATT" || RegTypeCode == "XATA" || RegTypeCode == "HCFULL")
        //                        {
        //                            FinalRegType = "Exhibitor Full Conference";
        //                        }
        //                        else if (RegTypeCode == "GEST" || RegTypeCode == "MGEST")
        //                        {
        //                            FinalRegType = "NACS Guest";
        //                        }
        //                        else
        //                        {
        //                            FinalRegType = (string)help.GetSearchValue(result, "BusinessType");
        //                        }
        //                    }

        //                    attendees.Add(new NACSAttendeeForSale()
        //                    {
        //                        RegistrantKey = (string)help.GetSearchValue(result, "RegistrantID"),
        //                        RegistrationDate = (string)help.GetSearchValue(result, "RegistrationDate"),
        //                        RegistrantLastName = (string)help.GetSearchValue(result, "RegistrantLastName"),
        //                        RegistrantBadgeName = (!string.IsNullOrWhiteSpace(badgename)) ? badgename : firstname,
        //                        Suffix = (string)help.GetSearchValue(result, "Suffix"),
        //                        Prefix = (string)help.GetSearchValue(result, "Prefix"),
        //                        Title = (string)help.GetSearchValue(result, "Title"),
        //                        CompanyName1 = (string)help.GetSearchValue(result, "CompanyName1"),
        //                        CompanyName2 = (string)help.GetSearchValue(result, "CompanyName2"),
        //                        AddressLine1 = (string)help.GetSearchValue(result, "AddressLine1"),
        //                        AddressLine2 = (string)help.GetSearchValue(result, "AddressLine2"),
        //                        City = (string)help.GetSearchValue(result, "City"),
        //                        State = (string)help.GetSearchValue(result, "State"),
        //                        PostalCode = (string)help.GetSearchValue(result, "PostalCode"),
        //                        CountryName = (string)help.GetSearchValue(result, "CountryName"),
        //                        PhoneCountryPrefix = (string)help.GetSearchValue(result, "PhoneCountryPrefix"),
        //                        PhoneNumber = (string)help.GetSearchValue(result, "PhoneNumber"),
        //                        RegistrationType = FinalRegType,
        //                        StoreClass = (string)help.GetSearchValue(result, "StoreClass")
        //                    });
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //_lblMsg.Text = "Error returning results: " + ex.Message.ToString(); 
        //        }
        //    }

        //    #endregion

        //    return attendees;

        //}

        private int LogDownloadUse(string ind_key, string org_key, string prod_key, string file, bool firstdownload)
        {

            string strCommand = "ProductDownloads_Insert @IndividualKey='" + ind_key + "',@OrganizationKey='" + org_key + "',@ProductKey='" + prod_key + "',@Filename='" + file + "',@OriginalDownload='" + firstdownload.ToString() + "'";

            //Define and open connections
            SqlConnection conn = new SqlConnection(connStringWeb);
            SqlCommand cmd = new SqlCommand(strCommand, conn);
            conn.Open();

            //Execute
            int returnID = Convert.ToInt32(cmd.ExecuteScalar());

            //Clean up
            conn.Close();
            conn.Dispose();
            cmd.Dispose();

            return returnID;
        }

        //TBD dependent on GetAttendees
        private int ExportList(string filetype, string ikey, string okey, string pkey, string year)
        {
            // Set license key to use GemBox.Spreadsheet in a Free mode.
            SpreadsheetInfo.SetLicense(GemboxLicense);
            int row = 0;

            string newFilename = "NACSShowAttendeeList" + year + "_" + DateTime.Now.ToString() + "_" + ikey + "." + filetype;
            newFilename = newFilename.Replace(":", "").Replace("/", "").Replace(" ", "_");

            try
            {
                // Create new empty Excel file.
                ExcelFile workbook = new ExcelFile();
                ExcelWorksheet worksheet = workbook.Worksheets.Add(year + " NACS Show Attendees");

                //add headers
                worksheet.Rows[0].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells[0, 0].Value = "RegistrationDate";
                worksheet.Cells[0, 1].Value = "RegistrantLastName";
                worksheet.Cells[0, 2].Value = "RegistrantBadgeName";
                worksheet.Cells[0, 3].Value = "Suffix";
                worksheet.Cells[0, 4].Value = "Title";
                worksheet.Cells[0, 5].Value = "CompanyName1";
                worksheet.Cells[0, 6].Value = "CompanyName2";
                worksheet.Cells[0, 7].Value = "AddressLine1";
                worksheet.Cells[0, 8].Value = "AddressLine2";
                worksheet.Cells[0, 9].Value = "City";
                worksheet.Cells[0, 10].Value = "State";
                worksheet.Cells[0, 11].Value = "PostalCode";
                worksheet.Cells[0, 12].Value = "CountryName";
                worksheet.Cells[0, 13].Value = "PhoneNumber";
                worksheet.Cells[0, 14].Value = "RegistrationType";
                worksheet.Cells[0, 15].Value = "StoreClass";


                //This was changed from GetAttendees(year) which called the GetAttendees method in this same class file. Now it calls external GetAttendeesForSale.GetAttendees() method.
                //var attendees = GetAttendees(year);
                var attendees = GetAttendeeListForSale.GetAttendees();

                if (attendees != null)
                {
                    foreach (var attendee in attendees)
                    {
                        row++;

                        worksheet.Cells[row, 0].Value = attendee.RegistrationDate;
                        worksheet.Cells[row, 1].Value = attendee.RegistrantLastName;
                        worksheet.Cells[row, 2].Value = attendee.RegistrantBadgeName;
                        worksheet.Cells[row, 3].Value = attendee.Suffix;
                        worksheet.Cells[row, 4].Value = attendee.Title;
                        worksheet.Cells[row, 5].Value = attendee.CompanyName1;
                        worksheet.Cells[row, 6].Value = attendee.CompanyName2;
                        worksheet.Cells[row, 7].Value = attendee.AddressLine1;
                        worksheet.Cells[row, 8].Value = attendee.AddressLine2;
                        worksheet.Cells[row, 9].Value = attendee.City;
                        worksheet.Cells[row, 10].Value = attendee.State;
                        worksheet.Cells[row, 11].Value = attendee.PostalCode;
                        worksheet.Cells[row, 12].Value = attendee.CountryName;
                        worksheet.Cells[row, 13].Value = attendee.PhoneNumber;
                        worksheet.Cells[row, 14].Value = attendee.RegistrationTypeCode;
                        worksheet.Cells[row, 15].Value = attendee.StoreClass;
                    }
                }

                //log the export
                int retID = LogDownloadUse(ikey, okey, pkey, newFilename, true);

                //save the file
                //workbook.Save(@"C:\inetpub\wwwroot\apps.nacsonline.com\AttendeeListExports\" + newFilename);
                XlsxSaveOptions options = new XlsxSaveOptions();
                workbook.Save(@"C:\inetpub\wwwroot\apps.nacsonline.com\AttendeeListExports\" + newFilename);
            }
            catch (Exception ex)
            {
                _lblMsg.Text = ex.Message.ToString();
            }

            return row;
        }

        #endregion

        internal string LoadAmbassadors(string mID)
        {
            string selfRegEmail = string.Empty;

            Uri u = new Uri(HttpContext.Request.Scheme + Uri.SchemeDelimiter + HttpContext.Request.Host + HttpContext.Request.Path);

            string msgbody = "Please help out at the NACS Show as an on-site Ambassador!"
                + "%0D%0A"//line break
                + "%0D%0A"//line break
                + "Simply click the link below to reserve a spot today."
                + "%0D%0A"//line break
                + u.AbsoluteUri.Substring(0, u.AbsoluteUri.LastIndexOf('/') + 1)
                + "Portal/Ambassadors.aspx%3Fmysid=" + mID + "%26sr=1"
                + "%0D%0A"//line break
                + "%0D%0A"//line break
                + "Spaces fill up fast, so don%27t delay!";

            selfRegEmail = "<a href='mailto:?body=" + msgbody
                + "&subject=Be an On-Site Ambassador at the NACS Show!' class='link link--pill text--white fill--gradient-pacific-blue-to-bondi-blue'><i class='fas fa-envelope-open-text'></i>&nbsp;Generate Self-Registration Email</a>";
            return selfRegEmail;
        }

        private bool CheckLogosDocs(string type, string ExhibitorID)
        {
            DataTable _dt = new DataTable();

            string SqlStatement = "execute ExhibitorDirectory_" + type + "_Select @ExhibitorID='" + ExhibitorID + "'";
            SqlConnection conn = new SqlConnection(config.GetConnectionString("nacsshowexhibdir"));
            SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, conn);
            _da.Fill(_dt);
            if (conn.State == ConnectionState.Open) { conn.Close(); }
            _da.Dispose();
            conn.Dispose();

            if (_dt.Rows.Count > 0)
            { return true; }
            else
            { return false; }
        }

        private List<DirectoryListingSegment2> DirectorySegments(DataRow dr)
        {

            List<DirectoryListingSegment2> segments = new List<DirectoryListingSegment2>();

            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Basic Listing",
                Rating = 10,
                Completed = true
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Promo Paragraph",
                Rating = 20,
                Completed = (dr["ExhibitorParagraph"].ToString() != "")
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Logo",
                Rating = 20,
                Completed = (CheckLogosDocs("Logos", dr["ExhibitorId"].ToString()))
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Directory Contact",
                Rating = 20,
                Completed = (dr["SalesContactKey"].ToString() != "")
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Document(s)",
                Rating = 10,
                Completed = (CheckLogosDocs("Documents", dr["ExhibitorId"].ToString()))
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Brand(s)",
                Rating = 10,
                Completed = (dr["ExhibitorBrands"].ToString() != "")
            });
            segments.Add(new DirectoryListingSegment2()
            {
                SegmentName = "Regions Served",
                Rating = 10,
                Completed = (dr["ExhibitorGeoAreas"].ToString() != "")
            });

            return segments;
        }

        private string GetLoggedInPersonKey(string ID)
        {
            string custkey = "";

            try
            {
                NACSAPICustomerSoapClient service = new NACSAPICustomerSoapClient();
                NACS.Helper.CustomerService.NACSIndividual ind = service.Individual_GetById(ID, string.Empty, this.NACSAPIKey);
                custkey = ind.IndividualKey;
            }
            catch
            {
                custkey = "";
            }

            return custkey;
        }

        private NACS.Helper.CustomerService.NACSIndividual GetLoggedInPerson_FullRecord(string ID)
        {
            NACS.Helper.CustomerService.NACSIndividual ind = null;

            try
            {
                NACSAPICustomerSoapClient service = new NACSAPICustomerSoapClient();
                ind = service.Individual_GetById(ID, string.Empty, this.NACSAPIKey);
            }
            catch
            {
                ind = null;
            }

            return ind;
        }

        private string GetOrgKeyFromSelectedExhibitor(string MYSID)
        {
            NACSExhibitor_Portal dt = GetExhibitor(MYSID);
            if (dt != null)
                return dt.OrganizationKey;
            return "";
        }

        private DataTable GetDownloads(string ind_key, string org_key, string prod_key)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            string strSQL = "ProductDownloads_Select @IndividualKey = '" + ind_key + "', @OrganizationKey = '" + org_key + "', @ProductKey = '" + prod_key + "'";

            SqlDataAdapter da = new SqlDataAdapter(strSQL, connStringWeb);
            da.Fill(ds);

            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                //Clean up
                da.Dispose();
                ds.Dispose();
                return dt;
            }
            else
            {
                //Clean up
                da.Dispose();
                ds.Dispose();
                return null;
            }

            
        }

        private void SendConfirmationEmail(string Email, string ListName, string ListCost, string cc, string CompanyName, string FirstName, string LastName, string ConfirmationNumber)
        {

            //Send order confirmation
            try
            {
                string products = "<table>";

                products += "<tr>";
                products += "<td><font face='Arial' size='2'>" + ListName + "</font></td>";
                products += "<td align='right'><font face='Arial' size='2'>" + ListCost + "</font></td>";
                products += "</tr>";

                products += "<tr><td>&nbsp;</td></tr>";

                products += "<tr><td><font face='Arial' size='2'><strong>Total:</strong></font></td>";
                products += "<td align='right'><font face='Arial' size='2'>" + String.Format("{0:c}", ListCost) + "</font></td></tr>";
                products += "<tr><td>&nbsp;</td></tr>";
                products += "<tr><td colspan='2'><font face='Arial' size='2'><strong>Payment Method:&nbsp;</strong>";

                string ccNumber = (cc != "") ? "xxxx" + cc.Substring((cc.Trim().Length - 4), 4) : "";
                products += "Credit Card (" + ccNumber + ")";
                products += "<br /><strong>Company Name:&nbsp;</strong>" + CompanyName;
                products += "<br /><strong>Contact:&nbsp;</strong>" + FirstName + "&nbsp;" + LastName;
                products += "</font></td></tr></table></font>";

                var msg = new EmailMessage();
                //var eti = EmailTemplateProvider.GetEmailTemplate("NACSShow_AttendeeListConfirmation", SiteContext.CurrentSiteID);

                var mcr = MacroResolver.GetInstance();
                mcr.SetNamedSourceData("ConfirmationNumber", ConfirmationNumber);
                mcr.SetNamedSourceData("Company", CompanyName);
                mcr.SetNamedSourceData("FirstName", FirstName);
                mcr.SetNamedSourceData("ProductsTable", products);

                msg.EmailFormat = EmailFormatEnum.Html;
                //msg.From = GetEmailFromAddress(eti.TemplateFrom);
                //msg.BccRecipients = eti.TemplateBcc;
                msg.Recipients = Email;
                msg.Subject = "NACS Show Attendee List - Order Confirmation (#" + ConfirmationNumber + ")";

                //EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, msg, eti, mcr, true);
            }
            catch (Exception ex)
            {
                // _pnlAttendeeLists.Controls.AddAt(0, new LiteralControl(ex.Message.ToString()));
            }

        }

        //ENCRYTPION CODE
        private static readonly string encryptionKey = "NACS15EXHIBS2MYSPORTALS!"; //needs to be 16 or 24 characters long

        public static string Encrypt(string toEncrypt, bool useHashing)
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
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString.Replace(' ', '+'));

            if (useHashing)
            {
                // If hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
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


        [Serializable]
        public class NACSExhibitor_Portal //for Exhibitor Portal
        {
            public string OrganizationId { get; set; }

            public string OrganizationKey { get; set; }

            public string OrganizationName { get; set; }

            public string DirectoryName { get; set; }

            public string PrimaryProductArea { get; set; }

            public string BoothNumber { get; set; }

            public string ExhibitorKey { get; set; }

            public string ExhibitorId { get; set; }

            public string ExhibitKey { get; set; }

            public string ExhibitName { get; set; }

            public string ExhibitYear { get; set; }

            public string MYSId { get; set; }

            public string ExhibitorMemberType { get; set; }

            public string ExhibitorPEIMemberType { get; set; }

            public string ExhibitorNew { get; set; }
            public string TSCId { get; set; }

            public string StatusMessage { get; set; }

            public string StatusMessageDetails { get; set; }
        }

        //These functions have currently been removed //////////////

        //Deletes a single Ambassador Record
        internal string DeleteAmbassador(string aID, string mID)
        {
            string returnValue = "error";

            string SqlStatement = "update Exhibitors_Ambassadors set DeleteFlag='1' where AmbassadorID='" + aID + "'"
                + " and MYSID='" + mID + "'";

            try
            {
                SqlConnection connection = new SqlConnection(config.GetConnectionString("nacsshow"));
                SqlCommand cmd = new SqlCommand(SqlStatement, connection);

                // Open connection and execute command
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                returnValue = cmd.ExecuteScalar().ToString();

                // Clean up
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                connection.Dispose();
                cmd.Dispose();
            }
            catch { returnValue = "error"; }

            return returnValue;
        }

        //Deletes a single Contractor Record
        internal string DeleteContractor(string cID, string mID)
        {
            string returnValue = "error";

            string SqlStatement = "update Exhibitors_Contractors set DeleteFlag = '1' where ID='" + cID + "'"
                + " and ExhibitorKey='" + mID + "'";

            try
            {
                SqlConnection connection = new SqlConnection(config.GetConnectionString("nacsshow"));
                SqlCommand cmd = new SqlCommand(SqlStatement, connection);

                // Open connection and execute command
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                returnValue = cmd.ExecuteScalar().ToString();

                // Clean up
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                connection.Dispose();
                cmd.Dispose();
            }
            catch { returnValue = "error"; }

            return returnValue;
        }
    }
}
