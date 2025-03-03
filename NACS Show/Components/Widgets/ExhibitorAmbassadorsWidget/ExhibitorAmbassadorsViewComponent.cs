using System.Data;
using System.Globalization;
using CMS.Membership;
using NACS.Protech.Framework;
using NACS.Protech.Entities;
using Microsoft.AspNetCore.Http;
using NACSShow.Components.Widgets.ExhibitorAmbassadorsWidget;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.Websites.Routing;
using Microsoft.Extensions.Configuration;
using CMS.Core;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using CMS.EmailEngine;

[assembly: RegisterWidget(ExhibitorAmbassadorsViewComponent.IDENTIFIER, typeof(ExhibitorAmbassadorsViewComponent), "Exhibitor Ambassadors", typeof(ExhibitorAmbassadorsProperties), Description = "Widget for Exhibitor Ambassadors")]

namespace NACSShow.Components.Widgets.ExhibitorAmbassadorsWidget
{
    public class ExhibitorAmbassadorsViewComponent(IHttpContextAccessor httpContextAccessor, IUserInfoProvider userInfoProvider, IWebsiteChannelContext websiteChannelContext, IConfiguration config, IEventLogService eventLogService) : ViewComponent
    {
        public const string IDENTIFIER = "NACSShow.ExhibitorAmbassadors";
        public CultureInfo en_US = CultureInfo.CreateSpecificCulture("en-US");

        //MUST MANUALLY EDIT TIME ZONE INFO IN GENERATEICAL()
        public string IndividualKey = "";
        public string OrganizationKey = "";
        public DataTable _dtUserDetails = null;
        public string clientID = "";
        public string companyClientID = "";
        public string MYSID = "";
        private string connectionStr;
        GeneralRepository genRepo = new GeneralRepository();
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IUserInfoProvider userInfoProvider = userInfoProvider;
        private readonly IWebsiteChannelContext websiteChannelContext = websiteChannelContext;
        private readonly IConfiguration config = config;
        private readonly IEventLogService eventLogService = eventLogService;

        public IViewComponentResult Invoke(ExhibitorAmbassadorsProperties properties)
        {
            var vm = new ExhibitorAmbassadorsViewModel();
            //clientID = this.ClientID; where to get the client ID
            string MYSID = "";
            string checkin = "";
            connectionStr = config.GetConnectionString("nacsshow");

            //Set view model data 
            vm.ShowYear = properties.ShowYear;
            vm.ContactName = properties.ContactName;
            vm.ContactPhone = properties.ContactPhone;
            vm.ContactEmail = "<a href='mailto:" + properties.ContactEmail + "'>" + properties.ContactEmail + "</a>";
            //_lblPageTitle.Text = "NACS Show " + ShowYear + " Ambassador On-Site Check-In";
            //Get values from query string
            MYSID = GetQueryStringValue("mysid");
            checkin = GetQueryStringValue("checkin");

            //try
            //{
            //    _hdnCheckInFlag.Value = checkin;
            //}
            //catch
            //{
            //    _hdnCheckInFlag.Value = "";
            //}


            //Last 4 digits of Anna's office number to get the report
            if ((MYSID != null) && (MYSID.ToString() == "4243"))
            {
                try
                {
                    // Panel _pnlReport = FindControl("_pnlReport") as Panel;
                    //_pnlReport.Visible = true;
                }
                catch (Exception ex)
                {
                    MYSID = "";
                }
            }
            else if ((MYSID != null) && (MYSID != ""))
            {
                var filters = new FetchFilter
                {
                    FilterType = FilterTypeOperators.And,
                    Conditions = new List<FilterCondition>
                    {
                        new FilterCondition("nacs_externalid", MYSID), //where exhibit mysid is current
                        new FilterCondition("nacs_exhibitname", "NACS Show 2023"), //where exhibitname is current Show
                    }
                };

                //var exhibitor = genRepo.GetAll<NACSExhibitor>(filters).FirstOrDefault();

                //load MSYID / exhibitor key from URL query string
                try
                {
                    if (MYSID.Length > 30)
                    {
                        MYSID = GetMSYID(MYSID);
                    }
                }
                catch { MYSID = ""; }


                //LoadExhibitorList();
                //_btnReserveTimeSlots.Visible = false;
                //_pnlError.Visible = false;

                if (MYSID != "")
                {
                    try
                    {
                        //_ddlExhibitors.SelectedValue = MYSID;
                    }
                    catch { }
                }
                //_pnlCheckIn.Visible = true;


                //if an exhibitor key is found, try to load the exhibitor record
                if (MYSID != "")
                {
                    //Session["MYSID"] = _hdnMYSID.Value.ToString();
                    try
                    {
                        LoadExhibitRecord(MYSID);
                    }
                    catch { }
                }

                //block access if no exhibitor record is found and is not On-Site Check-In
                //if (_hdnMYSID.Value == "" && _hdnCheckInFlag.Value != "1")
                //{
                //_pnlError.Visible = true;
                //}

                //if (_hdnMYSID.Value != "")
                //{ LoadMainPage(_hdnExhibitorKey.Value); }
                { LoadMainPage(MYSID, vm.ShowYear); }
            }
            else if (MYSID == null /*&& _hdnCheckInFlag.Value == "1"*/)
            {

                //LoadExhibitorList();
                // _btnReserveTimeSlots.Visible = false;
                //_pnlError.Visible = false;
                //_pnlCheckIn.Visible = true;

            }

            return View("~/Components/Widgets/ExhibitorAmbassadorsWidget/_ExhibitorAmbassadors.cshtml", vm);
        }

        #region "Helpers"
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
        #endregion

        #region "Methods"
        // Get Ambassadors Excel Report Data
        protected DataTable GetAmbassadorsReportData()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Exhibitors_Ambassadors_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }

        internal string GetMSYID(string key)
        {
            string id = "";

            if ((MYSID != null) && (MYSID.ToString() != ""))
            {
                var filters = new FetchFilter
                {
                    FilterType = FilterTypeOperators.And,
                    Conditions = new List<FilterCondition>
                {
                    new FilterCondition("nacs_externalid", MYSID), //where exhibit mysid is current
                    new FilterCondition("nacs_exhibitname", "NACS Show 2023"), //where exhibitname is current Show
                }
                };

                var exhibitor = genRepo.GetAll<NACSExhibitor>(filters).FirstOrDefault();

                id = (exhibitor != null) ? exhibitor.ExternalId : "";
            }
            return id;
        }

        //Load a specific Exhibitor Record
        internal void LoadExhibitRecord(string mID)
        {
            if ((MYSID != null) && (MYSID.ToString() != ""))
            {
                var filters = new FetchFilter
                {
                    FilterType = FilterTypeOperators.And,
                    Conditions = new List<FilterCondition>
                {
                    new FilterCondition("nacs_externalid", MYSID), //where exhibit mysid is current
                    new FilterCondition("nacs_exhibitname", "NACS Show 2023"), //where exhibitname is current Show
                }
                };

                var exhibitor = genRepo.GetAll<NACSExhibitor>(filters).FirstOrDefault();

                if (exhibitor != null && exhibitor.ExternalId.Length > 0)
                {
                    //_hdnExhibitorKeyValue = exhibitor.ExhibitingAccountId.ToString();
                    //_hdnDirectoryNameValue = exhibitor.ExhibitingAccountName.ToString();
                    //_hdnCompanyNameValue = exhibitor.ExhibitingAccountName.ToString();
                    //_lblExhibitingAsValue = exhibitor.ExhibitingName.ToString();
                    //_hdnTSValue = exhibitor.PrimaryContact_Name.ToString();
                    //_lblExhibitContactValue = exhibitor.PrimaryContact_Name.ToString();
                    //_lblExhibitorPhoneValue = exhibitor.PrimaryContact_Telephone.ToString();
                    //_lblExhibitorEmailNavigateUrlValue = "mailto:" + exhibitor.PrimaryContact_Email.ToString();
                    //_lblExhibitorEmailValue = exhibitor.PrimaryContact_Email.ToString();
                    //_hdnEmailValue = exhibitor.PrimaryContact_Email.ToString();
                    //_hdnBoothNumberValue = exhibitor.BoothNumber.ToString();
                    //if (_hdnBoothNumber.Value.Length > 2) { _lblExhibitBooth.Text = _hdnBoothNumber.Value.ToString(); }
                    //else { _lblExhibitBooth.Text = "'unassigned'"; }
                    //_lblCompanyCity.Text = exhibitor.AddressCity.ToString();
                    //_lblCompanyState.Text = exhibitor.AddressState.ToString();
                    //_lblCompanyZip.Text = exhibitor.AddressPostalCode.ToString();
                    //_lblCompanyCountry.Text = exhibitor.AddressCountry.ToString();

                    //Session["ContactName"] = _lblExhibitContact.Text;
                }
            }
        }

        //internal void LoadLogo()
        //{
        //    string savepath = "C:\\inetpub\\wwwroot\\apps.nacsonline.com\\ExhibitorPortal\\Ambassadors\\";
        //    string ENV = "";
        //    if (Request.Url.ToString().Contains("kentico.") || Request.Url.ToString().Contains("dev.")) { ENV = "dev.apps."; }
        //    else if (Request.Url.ToString().Contains("staging.")) { ENV = "staging.apps2."; }
        //    else if (!Request.Url.ToString().Contains("staging.") || !Request.Url.ToString().Contains("kentico.")) { ENV = "apps2."; }

        //    string PrefixCompany = _lblExhibitingAs.Text;
        //    string PrefixBooth = _lblExhibitBooth.Text;
        //    string regexSearch = new string(Path.GetInvalidFileNameChars());
        //    Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
        //    PrefixCompany = r.Replace(PrefixCompany, "");

        //    string FileName = ShowYear + "-LogoFile - " + PrefixCompany + " - Booth " + PrefixBooth;

        //    if (Request.Url.ToString().Contains("kentico.") || Request.Url.ToString().Contains("dev.")) { savepath = savepath.Replace("apps.", "dev.apps."); }
        //    else if (Request.Url.ToString().Contains("staging.")) { savepath = savepath.Replace("apps.", "staging.apps."); }

        //    string[] filePaths = Directory.GetFiles(savepath);

        //    foreach (string file in filePaths)
        //    {
        //        if (file.Contains(FileName))
        //        {
        //            _fuCompanyLogo.Visible = false;
        //            _btnUploadLogo.Visible = false;
        //            _imgCompanyLogo.ImageUrl = "http://" + ENV + "nacsonline.com/ExhibitorPortal/Ambassadors/" + Path.GetFileName(file);
        //            _lnkCompanyLogo.NavigateUrl = "http://" + ENV + "nacsonline.com/ExhibitorPortal/Ambassadors/" + Path.GetFileName(file);
        //            //img.Src = "http://" + ENV + "nacsonline.com/ExhibitorPortal/Ambassadors/" + Path.GetFileName(file); ;
        //            //_imgCompanyLogo.ImageUrl = "http://apps2.nacsonline.com/ExhibitorPortal/Ambassadors/" + Path.GetFileName(file); http://" + ENV + "nacsonline.com/ExhibitorPortal
        //            //_lnkCompanyLogo.NavigateUrl = "http://apps2.nacsonline.com/ExhibitorPortal/Ambassadors/" + Path.GetFileName(file);
        //            _lnkCompanyLogo.Visible = true;
        //            _lblLogoLabel.Text = "Click to view image.";  //"<i class='fas fa-camera-retro fa-lg'></i>" + " " +
        //            _btnDeleteCompanyLogo.Visible = true;
        //            break;
        //        }
        //    }
        //    if (_hdnCheckInFlag.Value == "1" || _hdnSelfRegFlag.Value == "1")
        //    {
        //        _lblLogoLabel.Text = "";
        //        _lnkCompanyLogo.Visible = false;
        //        _fuCompanyLogo.Visible = false;
        //        _btnUploadLogo.Visible = false;
        //        _btnDeleteCompanyLogo.Visible = false;
        //    }
        //}

        internal void LoadMainPage(string mID, string showYear)
        {
            //try { _hdnSelfRegFlag.Value = Request.QueryString["sr"].ToString(); }
            //catch { _hdnSelfRegFlag.Value = ""; }
            //if (_hdnSelfRegFlag.Value == "1")
            if ("1" == "1")
            {
                //try { _txtEmail.Text = Request.QueryString["em"].ToString(); }
                //catch { _txtEmail.Text = ""; }

                LoadAvailableTimeSlots(true,showYear);

                //_fuCompanyLogo.Visible = false;
                //_btnUploadLogo.Visible = false;
                //_trReserveSlots.Visible = false;
                //_trSelfRegReserveSlots.Visible = true;
                //_trSaveDetails.Visible = false;
                //_trSelfRegSaveDetails.Visible = true;
                //_pnlSelectTimeSlots.Visible = true;
            }

            else
            {
                LoadSelectedTimeSlots(mID,showYear);
                //_pnlReservedTimeSlots.Visible = true;
            }

            //LoadLogo();
            //_pnlError.Visible = false;
            //_pnlMain.Visible = true;
        }

        //Load available time slots
        internal DataTable? LoadAvailableTimeSlots(bool selfreg , string showYear)
        {
            DataTable _dtTimeSlots = new DataTable();
            try
            {
                string SqlStatement = "select T.TimeSlotID"
                    + ", TimeSlotStartTime"
                    + ", TimeSlotEndTime"
                    + ", StationID"
                    + ", StationName"
                    + ", Location"
                    + ", Allotment"
                    + ", (Allotment - (select COUNT(*) from Exhibitors_Ambassadors A where A.TimeSlotID = T.TimeSlotID and A.DeleteFlag='0' and A.Year='" + showYear + "')) AS Remaining"
                + " from Exhibitors_Ambassadors_TimeSlotsAtlanta T"
                + " where (select COUNT(*) from Exhibitors_Ambassadors A where A.TimeSlotID = T.TimeSlotID and A.DeleteFlag='0' and A.Year='" + showYear + "') < Allotment"
                + " order by T.TimeSlotID";

                SqlConnection dbConnection = new SqlConnection(connectionStr);
                SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, dbConnection);
                _da.Fill(_dtTimeSlots);

                if (dbConnection.State == ConnectionState.Open) { dbConnection.Close(); }

                _da.Dispose();
                dbConnection.Dispose();
                SqlStatement = null;
            }

            catch { return null; }
            return _dtTimeSlots;
        }

        //Load selected time slots
        internal DataTable? LoadSelectedTimeSlots(string mID, string showYear)
        {
            DataTable _dtSelectedSlots = new DataTable();
            try
            {
                string SqlStatement = "select A.AmbassadorID"
                + ", A.TimeSlotID"
                + ", T.TimeSlotStartTime"
                + ", T.TimeSlotEndTime"
                + ", T.StationID"
                + ", T.StationName"
                + ", T.Location"
                + ", A.FirstName"
                + ", A.LastName"
                + ", A.DateCheckedIn"
                + " from Exhibitors_Ambassadors A"
                    + " join Exhibitors_Ambassadors_TimeSlotsAtlanta T"
                    + " on T.TimeSlotID = A.TimeSlotID"
                    + " and A.DeleteFlag='0'"
                    + " and MYSID='" + mID + "'"
                    + " and Year='" + showYear + "'"
                    + " order by T.StationID";

                SqlConnection connection = new SqlConnection(connectionStr);
                SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, connection);
                _da.Fill(_dtSelectedSlots);

                if (connection.State == ConnectionState.Open) { connection.Close(); }

                _da.Dispose();
                connection.Dispose();
                SqlStatement = null;
                if (_dtSelectedSlots.Rows.Count > 0)
                {

                    //_lblVolunteers.Visible = true;
                    //_btnReserveTimeSlots.Text = "Reserve Additional Time Slots";
                }
            }
            catch
            {
                return null;

            }
            return _dtSelectedSlots;
        }

        //Load single Ambassador entry to edit
        internal void LoadAmbassadorToEdit(string aID)
        {
            string TimeSlotID = "";
            string SqlStatement = "select * from Exhibitors_Ambassadors where AmbassadorID='" + aID + "'";
            SqlConnection connection = new SqlConnection(connectionStr);
            SqlCommand cmd = new SqlCommand(SqlStatement, connection);
            SqlDataReader dataReader = null;
            if (connection.State == ConnectionState.Closed) { connection.Open(); }
            dataReader = cmd.ExecuteReader();

            //while (dataReader.Read())
            //{
            //    _txtFirstName.Text = dataReader["FirstName"].ToString();
            //    _txtLastName.Text = dataReader["LastName"].ToString();
            //    _txtTitle.Text = dataReader["Title"].ToString();
            //    _txtEmail.Text = dataReader["Email"].ToString();
            //    _txtMobile.Text = dataReader["Mobile"].ToString();
            //    _cboLanguages1.Checked = Convert.ToBoolean(dataReader["Language1"]);
            //    _cboLanguages2.Checked = Convert.ToBoolean(dataReader["Language2"]);
            //    _cboLanguages3.Checked = Convert.ToBoolean(dataReader["Language3"]);
            //    _cboLanguages4.Checked = Convert.ToBoolean(dataReader["Language4"]);
            //    _cboLanguages5.Checked = Convert.ToBoolean(dataReader["Language5"]);
            //    _cboLanguages6.Checked = Convert.ToBoolean(dataReader["Language6"]);
            //    TimeSlotID = dataReader["TimeSlotID"].ToString();
            //}

            // Clean up
            if (connection.State == ConnectionState.Open) { connection.Close(); }
            //dataReader.Dispose();
            connection.Dispose();
            cmd.Dispose();
            SqlStatement = null;

            //LoadSingleTimeSlotInfo(TimeSlotID);
        }

        //Loads details about a single time slot
        //internal void LoadSingleTimeSlotInfo(string tID)
        //{
        //    DataTable _dtTimeSlots = new DataTable();
        //    try
        //    {
        //        string SqlStatement = "select * from Exhibitors_Ambassadors_TimeSlotsAtlanta where TimeSlotID='" + tID + "'";
        //        SqlConnection connection = new SqlConnection(connectionStr);
        //        SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, connection);
        //        _da.Fill(_dtTimeSlots);

        //        if (connection.State == ConnectionState.Open) { connection.Close(); }

        //        _da.Dispose();
        //        connection.Dispose();
        //        SqlStatement = null;

        //        bool HasLocation = false;
        //        if (_dtTimeSlots.Rows[0]["Location"].ToString() != "") { HasLocation = true; }

        //        string SlotInfo = ((DateTime)_dtTimeSlots.Rows[0]["TimeSlotStartTime"]).ToString("dddd, MMMM d, h:mmtt", en_US)
        //            + " - " + _dtTimeSlots.Rows[0]["StationName"];
        //        //+ (HasLocation == true ? " at the " + _dtTimeSlots.Rows[0]["Location"] : "");

        //        _lblSlotInfo.Text = SlotInfo;
        //        _lblSlotInfo2.Text = SlotInfo;

        //        Session["TimeSlotDetails"] = _dtTimeSlots.Rows[0];
        //    }
        //    catch { _lblSlotInfo.Text = ""; }
        //}

        //Saves a single Ambassador's details
        //internal string UpdateAmbassadorDetails(string aID)
        //{
        //    string returnValue = "error";

        //    string SqlStatement = "update Exhibitors_Ambassadors set"
        //        + (_txtFirstName.Text != "" ? " FirstName='" + _txtFirstName.Text.Replace("'", "''") + "'" : " FirstName=null")
        //        + (_txtLastName.Text != "" ? ", LastName='" + _txtLastName.Text.Replace("'", "''") + "'" : ", LastName=null")
        //        + (_txtTitle.Text != "" ? ", Title='" + _txtTitle.Text.Replace("'", "''") + "'" : ", Title=null")
        //        + (_txtEmail.Text != "" ? ", Email='" + _txtEmail.Text.Replace("'", "''") + "'" : ", Email=null")
        //        + (_txtMobile.Text != "" ? ", Mobile='" + _txtMobile.Text.Replace("'", "''") + "'" : ", Mobile=null")
        //        + (_cboLanguages1.Checked ? ", Language1='1'" : ", Language1='0'")
        //        + (_cboLanguages2.Checked ? ", Language2='1'" : ", Language2='0'")
        //        + (_cboLanguages3.Checked ? ", Language3='1'" : ", Language3='0'")
        //        + (_cboLanguages4.Checked ? ", Language4='1'" : ", Language4='0'")
        //        + (_cboLanguages5.Checked ? ", Language5='1'" : ", Language5='0'")
        //        + (_cboLanguages6.Checked ? ", Language6='1'" : ", Language6='0'")
        //        + ", DateChanged=GetDate()"
        //        + " where AmbassadorID='" + aID + "'"
        //        //+ " and ExhibitorKey='" + _hdnExhibitorKey.Value + "'"
        //        + " and MYSID='" + _hdnMYSID.Value + "'"
        //        + " and Year='" + ShowYear + "'";

        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(connectionStr);
        //        SqlCommand cmd = new SqlCommand(SqlStatement, connection);

        //        // Open connection and execute command
        //        if (connection.State == ConnectionState.Closed) { connection.Open(); }

        //        returnValue = cmd.ExecuteScalar().ToString();

        //        // Clean up
        //        if (connection.State == ConnectionState.Open) { connection.Close(); }
        //        connection.Dispose();
        //        cmd.Dispose();
        //    }
        //    catch { returnValue = "error"; }

        //    return returnValue;
        //}

        //Deletes a single Ambassador Record
        internal string DeleteAmbassador(string aID)
        {
            string returnValue = "error";

            string SqlStatement = "update Exhibitors_Ambassadors"
                + " set DeleteFlag='1'"
                + " where AmbassadorID='" + aID + "'";
                //+ " and ExhibitorKey='" + _hdnExhibitorKey.Value + "'"
                //+ " and MYSID='" + _hdnMYSID.Value + "'"
                //+ " and Year='" + ShowYear + "'";
            try
            {
                SqlConnection dbConnection = new SqlConnection(connectionStr);
                SqlCommand cmd = new SqlCommand(SqlStatement, dbConnection);

                // Open connection and execute command
                if (dbConnection.State == ConnectionState.Closed) { dbConnection.Open(); }

                returnValue = cmd.ExecuteScalar().ToString();

                // Clean up
                if (dbConnection.State == ConnectionState.Open) { dbConnection.Close(); }
                dbConnection.Dispose();
                cmd.Dispose();
            }
            catch { returnValue = "error"; }

            return returnValue;
        }

        ////Onsite Check-In for a single Ambassador TimeSlot
        //internal string CheckInAmbassador(string aID, bool clear)
        //{
        //    string returnValue = "error";

        //    string SqlStatement = "update Exhibitors_Ambassadors"
        //        + " set DateCheckedIn=" + (clear ? "NULL" : "DATEADD(hour," + ShowHoursOffset.ToString() + ",GETDATE())")
        //        + " where AmbassadorID='" + aID + "'"
        //        //+ " and ExhibitorKey='" + _hdnExhibitorKey.Value + "'"
        //        + " and MYSID='" + _hdnMYSID.Value + "'"
        //        + " and Year='" + ShowYear + "'";

        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(connectionStr);
        //        SqlCommand cmd = new SqlCommand(SqlStatement, connection);

        //        // Open connection and execute command
        //        if (connection.State == ConnectionState.Closed) { connection.Open(); }

        //        returnValue = cmd.ExecuteScalar().ToString();

        //        // Clean up
        //        if (connection.State == ConnectionState.Open) { connection.Close(); }
        //        connection.Dispose();
        //        cmd.Dispose();
        //    }
        //    catch { returnValue = "error"; }

        //    return returnValue;
        //}

        //Load all Exhibitors DDL
        //internal void LoadExhibitorList()
        //{
        //    DataTable _dtCompanies = LoadAmbassadorCompanies();

        //    List<NACSExhibitor> exhibs = new List<NACSExhibitor>();
        //    exhibs = genRepo.GetAll<NACSExhibitor>("nacs_exhibitname", "NACS Show 2023").OrderBy(e => e.ExhibitingName).ToList();
        //    _ddlExhibitors.Items.Insert(0, new ListItem("-", "-1"));

        //    int i = 1;
        //    if (exhibs != null) //&& exhibs.Length > 0)
        //    {
        //        foreach (var booth in exhibs)
        //        {
        //            var found = _dtCompanies.Select("MYSID = '" + booth.ExternalId + "'");
        //            if (found.Length != 0)
        //            {
        //                string TextField = booth.ExhibitingAccountName
        //                    + " (booth " + (booth.BoothNumber != "" ? booth.BoothNumber : "unassigned") + ")";
        //                _ddlExhibitors.Items.Insert(i, new ListItem(TextField, booth.ExternalId));

        //                i++;
        //            }
        //        }
        //    }
        //}

        ////Load all Ambassador Exhibitor Companies
        //internal DataTable LoadAmbassadorCompanies()
        //{
        //    DataTable _dtCompanies = new DataTable();
        //    try
        //    {
        //        string SqlStatement = "select distinct MYSID"
        //            + " from Exhibitors_Ambassadors"
        //            + " where DeleteFlag='0'"
        //            + " and Year='" + ShowYear + "'";
        //        SqlConnection connection = new SqlConnection(connectionStr);
        //        SqlDataAdapter _da = new SqlDataAdapter(SqlStatement, connection);
        //        _da.Fill(_dtCompanies);

        //        if (connection.State == ConnectionState.Open) { connection.Close(); }

        //        _da.Dispose();
        //        connection.Dispose();
        //        SqlStatement = null;
        //    }
        //    catch { }

        //    return _dtCompanies;
        //}

        //internal void ResetAmbassadorToEdit()
        //{
        //    _txtFirstName.Text = "";
        //    _txtLastName.Text = "";
        //    _txtTitle.Text = "";
        //    _txtEmail.Text = "";
        //    _txtMobile.Text = "";
        //    _cboLanguages1.Checked = false;
        //    _cboLanguages2.Checked = false;
        //    _cboLanguages3.Checked = false;
        //    _cboLanguages4.Checked = false;
        //    _cboLanguages5.Checked = false;
        //    _cboLanguages6.Checked = false;
        //    _hdnAmbassadorID.Value = "";
        //}

        //internal void GenerateICAL()
        //{
        //    DataRow slot = (DataRow)Session["TimeSlotDetails"];

        //    TimeSpan tzShow = TimeZoneInfo.FindSystemTimeZoneById(ShowTimeZone).GetUtcOffset(DateTime.Now);
        //    TimeSpan tzServer = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        //    TimeSpan tzDiff = tzServer - tzShow;

        //    DateTime startDate = (DateTime)slot["CheckInTime"];
        //    DateTime endDate = (DateTime)slot["TimeSlotEndTime"];
        //    string organizer = ContactEmail;
        //    string location = (slot["Location"].ToString() != "" ? slot["Location"].ToString() + " - " : "") + slot["StationName"];
        //    string subject = "NACS Show Ambassador Time Slot";
        //    string description = "Please be aware that the time shown above may be displayed differently in your local time zone."
        //        + "  All times are listed in " + TimeZoneTextForICAL + ".  Please check in no later than " + startDate.ToShortTimeString() + " Eastern Time.\\n"
        //        + " \\n"
        //        + "Thanks for your support!";

        //    HttpContext ctx = HttpContext.Current;

        //    ctx.Response.ContentType = "text/calendar";
        //    ctx.Response.AddHeader("Content-disposition", "attachment; filename=NACS Show Ambassador Reservation.ics");

        //    ctx.Response.Write("BEGIN:VCALENDAR");
        //    ctx.Response.Write("\nVERSION:2.0");
        //    ctx.Response.Write("\nMETHOD:PUBLISH");

        //    ctx.Response.Write("\nBEGIN:VTIMEZONE");
        //    ctx.Response.Write("\nTZID:" + ShowTimeZone);
        //    ctx.Response.Write("\nBEGIN:STANDARD");
        //    ctx.Response.Write("\nDTSTART:16011104T020000");
        //    ctx.Response.Write("\nRRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=11");
        //    ctx.Response.Write("\nTZOFFSETFROM:-0700");
        //    ctx.Response.Write("\nTZOFFSETTO:-0800");
        //    ctx.Response.Write("\nEND:STANDARD");
        //    ctx.Response.Write("\nBEGIN:DAYLIGHT");
        //    ctx.Response.Write("\nDTSTART:16010311T020000");
        //    ctx.Response.Write("\nRRULE:FREQ=YEARLY;BYDAY=2SU;BYMONTH=3");
        //    ctx.Response.Write("\nTZOFFSETFROM:-0800");
        //    ctx.Response.Write("\nTZOFFSETTO:-0700");
        //    ctx.Response.Write("\nEND:DAYLIGHT");
        //    ctx.Response.Write("\nEND:VTIMEZONE");

        //    ctx.Response.Write("\nBEGIN:VEVENT");
        //    ctx.Response.Write("\nORGANIZER:MAILTO:" + organizer);
        //    ctx.Response.Write("\nDTSTART;TZID=\"" + ShowTimeZone + "\":" + startDate.Add(tzDiff).ToUniversalTime().ToString("yyyyMMddTHHmmssZ"));
        //    ctx.Response.Write("\nDTEND;TZID=\"" + ShowTimeZone + "\":" + endDate.Add(tzDiff).ToUniversalTime().ToString("yyyyMMddTHHmmssZ"));
        //    ctx.Response.Write("\nLOCATION:" + location);
        //    ctx.Response.Write("\nUID:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "@nacsonline.com");
        //    ctx.Response.Write("\nDTSTAMP:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ"));
        //    ctx.Response.Write("\nSUMMARY:" + subject);
        //    ctx.Response.Write("\nDESCRIPTION:" + description);
        //    ctx.Response.Write("\nPRIORITY:5");
        //    ctx.Response.Write("\nCLASS:PUBLIC");
        //    ctx.Response.Write("\nBEGIN:VALARM");
        //    ctx.Response.Write("\nTRIGGER:-PT15M");
        //    ctx.Response.Write("\nACTION:DISPLAY");
        //    ctx.Response.Write("\nDESCRIPTION:Reminder");
        //    ctx.Response.Write("\nEND:VALARM");
        //    ctx.Response.Write("\nEND:VEVENT");

        //    ctx.Response.Write("\nEND:VCALENDAR");
        //    ctx.Response.End();
        //}

        ////Send Email Confirmation to the Company Primary Contact
        //internal void SendConfirmationEmail()
        //{
        //    string flag = "0";
        //    string AmbassadorID = "";
        //    string StationID = "";
        //    string StationName = "";
        //    string StartTime = "";
        //    string EndTime = "";
        //    string CheckInTime = "";
        //    string Location = "";
        //    string Year = "";
        //    string CcErin = "aserfass@convenience.orgzzz";
        //    string SendTo = "bremoyer@convenience.org";

        //    //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //    SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
        //    StringBuilder sb = new StringBuilder();
        //    var msg = new EmailMessage();
        //    var eti = EmailTemplateProvider.GetEmailTemplate("NACSShow_AmbassadorsSignUpConfirmation", SiteContext.CurrentSiteID);
        //    string strFromMail = GetEmailFromAddress(eti.TemplateFrom);

        //    //Access Ambassador info
        //    DataSet dsAmbassador = new DataSet("Ambassador");
        //    SqlDataAdapter daAmbassador = new
        //    SqlDataAdapter("SELECT * FROM Exhibitors_Ambassadors A LEFT JOIN Exhibitors_Ambassadors_TimeSlotsAtlanta T " +
        //    "ON T.TimeSlotID = A.TimeSlotID  WHERE Year = '" + ShowYear + "' AND MYSID = '" + _hdnMYSID.Value.ToString() + "' AND DeleteFlag ='" + flag + "' ", connStringWeb);   //+ _hdnMYSID.Value.ToString() + 
        //    daAmbassador.Fill(dsAmbassador);
        //    DataTable dtAmbassador = dsAmbassador.Tables[0];

        //    if (ConfigurationManager.AppSettings["Environment"] == "DEV")
        //    {
        //        SendTo = "bremoyer@convenience.org";
        //    }
        //    else if (ConfigurationManager.AppSettings["Environment"] == "STAGING")
        //    {
        //        SendTo = "bremoyer@convenience.org";
        //    }
        //    else
        //    {
        //        SendTo = _hdnEmail.Value.ToString();
        //    }

        //    try
        //    {
        //        if (Session["ContactName"] != null)
        //        {
        //            try
        //            {
        //                string[] names = Session["ContactName"].ToString().Split(',');
        //                sb.Append("<p>Dear " + names[1].ToString() + " " + names[0].ToString() + ", " + "</p>");
        //            }
        //            catch
        //            {

        //            }
        //        }

        //        sb.Append("<p>Thank you for signing up to volunteer as an Ambassador at the " + ShowYear + " NACS Show.</p><p>");

        //        if (dtAmbassador.Rows.Count > 1)
        //        { sb.Append("Below are your scheduled time slots. "); }
        //        else { sb.Append("Below is your scheduled time slot. "); }

        //        sb.Append("Please remember to check in at the Ambassador Check In Desk located near NACS Registration in Building B.</p>");
        //        sb.Append("<p>Your selected station(s):</p>");

        //        for (int i = 0; i < dtAmbassador.Rows.Count; i++)
        //        {
        //            Year = dtAmbassador.Rows[i]["Year"].ToString();
        //            AmbassadorID = dtAmbassador.Rows[i]["AmbassadorID"].ToString();
        //            StationID = dtAmbassador.Rows[i]["StationID"].ToString();
        //            StationName = dtAmbassador.Rows[i]["StationName"].ToString();
        //            StartTime = dtAmbassador.Rows[i]["TimeSlotStartTime"].ToString();
        //            EndTime = dtAmbassador.Rows[i]["TimeSlotEndTime"].ToString();
        //            CheckInTime = dtAmbassador.Rows[i]["CheckInTime"].ToString();
        //            Location = dtAmbassador.Rows[i]["Location"].ToString();

        //            sb.Append("<br/><br />");
        //            //sb.Append("Ambassador ID: "+"<b>" + AmbassadorID + "</b><br/>");
        //            sb.Append("Station ID: " + "<b>" + StationID + "</b><br/>");
        //            sb.Append("Station Name: " + "<b>" + StationName + "</b><br/>");
        //            sb.Append("From: " + "<b>" + StartTime + "</b>" + " to " + "<b>" + EndTime + " Eastern Time</b><br/>");
        //            sb.Append("Check In Time: " + "<b>" + CheckInTime + " Eastern Time</b><br/>");
        //        }

        //        sb.Append("<br/><br/>");
        //        sb.Append("Please contact Anna Serfass at <a href='mailto:aserfass@convenience.org'>aserfass@convenience.org</a> if you have any questions.");
        //        sb.Append("<br/><br/>");
        //        sb.Append("Thank you,");
        //        sb.Append("<br/>");
        //        sb.Append("The NACS Team");

        //        var mcr = MacroResolver.GetInstance();
        //        mcr.SetNamedSourceData("MessageBody", sb.ToString());

        //        msg.EmailFormat = EmailFormatEnum.Html;
        //        msg.Recipients = SendTo.ToString();
        //        msg.From = eti.TemplateFrom;
        //        msg.Subject = eti.TemplateSubject;
        //        msg.BccRecipients = ""; // CcErin.ToString();
        //                                //msg.Body = sb.ToString();

        //        EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, msg, eti, mcr, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        //lbltest1.Text = ex.Message.ToString();
        //    }
        //}

        //Manually Send Email to Company Primary Contact 
        //private void ExecuteCode()
        //{
        //    try
        //    {
        //        string flag = "0";
        //        string Erin = "bremoyer@convenience.org";// "egaray@convenience.org";
        //        string AmbassadorID = "";
        //        string StationName = "";
        //        string StartTime = "";
        //        string EndTime = "";
        //        string CheckInTime = "";
        //        string Location = "";
        //        string Year = "";

        //        //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //        SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
        //        StringBuilder sb = new StringBuilder();
        //        var msg = new EmailMessage();
        //        var eti = EmailTemplateProvider.GetEmailTemplate("NACSShow_AmbassadorsSignUpConfirmation", SiteContext.CurrentSiteID);
        //        string strFromMail = GetEmailFromAddress(eti.TemplateFrom);

        //        var mcr = MacroResolver.GetInstance();

        //        if (Session["ContactName"] != null)
        //        {
        //            mcr.SetNamedSourceData("ContactName", Session["ContactName"].ToString());
        //        }
        //        else
        //        {
        //            mcr.SetNamedSourceData("ContactName", "");
        //        }

        //        //Access Ambassador info
        //        DataSet dsAmbassador = new DataSet("Ambassador");
        //        SqlDataAdapter daAmbassador = new
        //        SqlDataAdapter("SELECT * FROM Exhibitors_Ambassadors A LEFT JOIN Exhibitors_Ambassadors_TimeSlotsAtlanta T " +
        //        "ON T.TimeSlotID = A.TimeSlotID  WHERE A.Year = '2023' AND A.ExhibitorKey = '83B00BEA-7036-EC11-B6E6-000D3A13CDB8' AND DeleteFlag = '0' ", connStringWeb);   //+ _hdnMYSID.Value.ToString() + 
        //        daAmbassador.Fill(dsAmbassador);
        //        DataTable dtAmbassador = dsAmbassador.Tables[0];

        //        if (ConfigurationManager.AppSettings["Environment"] == "DEV")
        //        {
        //            SendTo = "bremoyer@convenience.org";
        //        }
        //        if (ConfigurationManager.AppSettings["Environment"] == "Staging")
        //        {
        //            SendTo = "bremoyer@convenience.org";
        //        }

        //        if (Session["ContactName"] != null)
        //        {
        //            sb.Append("Dear" + " " + Session["ContactName"].ToString() + "," + "<br/><br/>");
        //        }
        //        else
        //        {
        //            sb.Append("Dear," + "<br/><br/>");
        //        }

        //        sb.Append("Thank you for signing up to volunteer as an Ambassador at the " + ShowYear + " NACS Show. Below is your scheduled time slot. " +
        //                  "Please remember to check in at the Ambassador Check In Desk located in the Main Lobby of Building B.");
        //        sb.Append("<br/><br/>");
        //        sb.Append("<span>Your selected station(s):</span>");
        //        //sb.Append("<br/>");

        //        if (dtAmbassador.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtAmbassador.Rows.Count; i++)
        //            {
        //                Year = dtAmbassador.Rows[i]["Year"].ToString();
        //                //AmbassadorID = dtAmbassador.Rows[i]["AmbassadorID"].ToString();
        //                StationName = dtAmbassador.Rows[i]["StationName"].ToString();
        //                StartTime = dtAmbassador.Rows[i]["TimeSlotStartTime"].ToString();
        //                EndTime = dtAmbassador.Rows[i]["TimeSlotEndTime"].ToString();
        //                CheckInTime = dtAmbassador.Rows[i]["CheckInTime"].ToString();
        //                Location = dtAmbassador.Rows[i]["Location"].ToString();


        //                sb.Append("<br/><br />");
        //                //sb.Append("Ambassador ID: "+"<b>" + AmbassadorID + "</b><br/>");
        //                sb.Append("Station Name: " + "<b>" + StationName + "</b><br/>");
        //                sb.Append("Station Location: " + "<b><a href='" + Location + "' target='_blank'>" + Location + "</a></b><br/>");
        //                sb.Append("From: " + "<b>" + StartTime + "</b>" + " to " + "<b>" + EndTime + "</b><br/>");
        //                sb.Append("Check In Time: " + "<b>" + CheckInTime + "</b><br/>");
        //            }

        //            sb.Append("<br/><br/>");
        //            sb.Append("Please contact Anna Serfass at <a href='mailto:aserfass@convenience.org'>aserfass@convenience.org</a> if you have any questions.");
        //            sb.Append("<br/><br/>");
        //            sb.Append("Thank you,");
        //            sb.Append("<br/>");

        //            msg.EmailFormat = EmailFormatEnum.Html;
        //            msg.Recipients = SendTo.ToString();
        //            msg.From = eti.TemplateFrom;
        //            msg.Subject = eti.TemplateSubject;
        //            msg.BccRecipients = ""; // Erin.ToString();
        //            msg.Body = sb.ToString();

        //            for (int i = 0; i <= 0; i++)
        //            {
        //                EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, msg, eti, mcr, true);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        lbltest1.Text = ex.Message.ToString();
        //    }
        //}
        #endregion
    }
}
