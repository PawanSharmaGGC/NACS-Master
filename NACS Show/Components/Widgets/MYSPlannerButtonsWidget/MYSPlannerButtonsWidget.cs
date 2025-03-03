using CMS.Membership;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using MYSServiceProxyREST;

using NACSShow.Components.Widgets.MYSPlannerButtonsWidget;

using System.Configuration;
using System.Net;
using System.Web;

using TextInputComponent = Kentico.Forms.Web.Mvc.TextInputComponent;
using CMS.ContentEngine;

using Kentico.Xperience.Admin.Base.FormAnnotations;
using NACS.Helper.CustomerService;
using NACS_Classes;

[assembly: RegisterWidget(
	identifier: MYSPlannerButtonsWidgetViewComponent.IDENTIFIER, 
	viewComponentType: typeof(MYSPlannerButtonsWidgetViewComponent),
	name: "MYSPlannerButtonsWidget", 
	propertiesType: typeof(MYSPlannerButtonsWidgetProperties), 
	Description = "MYS Planner Buttons", 
	IconClass = "icon-l-ribbon",
	AllowCache = true)]

namespace NACSShow.Components.Widgets.MYSPlannerButtonsWidget
{
	[ViewComponent]
	public class MYSPlannerButtonsWidgetViewComponent : ViewComponent
    {

        public const string IDENTIFIER = "NACSShow.MYSPlannerButtonsWidget";

		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IContentQueryExecutor executor;

		protected string NACSAPIKey = GetNACSAPIKey();

		private static string GetNACSAPIKey()
		{
			return ConfigurationManager.AppSettings["NACSAPIKey"] ?? string.Empty;
		}

		private string currentUserEmail = "";
		private string APIEnvironment = "";
		private string MYSAPI_Username = "NACSuser";
		private string MYSAPI_Passcode = "@I66iMjs";
		private string MYSAPI_Showcode = "NACS21";

		public MYSPlannerButtonsWidgetViewComponent(IHttpContextAccessor httpContextAccessor, IContentQueryExecutor executor)
		{
			this.httpContextAccessor = httpContextAccessor;
			this.executor = executor;
		}

		public IViewComponentResult Invoke(ComponentViewModel<MYSPlannerButtonsWidgetProperties> widgetProperties)
		{
			var properties = widgetProperties.Properties;
			string link = "";
			var domain = HttpContext.Request.Host.Value;
			string DestinationURL = (domain.Contains("staging") == true) ? properties.DestinationURL_Staging : properties.DestinationURL_Production;

			APIEnvironment = (domain.Contains("staging") == true) ? "Staging" : "Production";

			string errorMessage = string.Empty;
			string planButtonText = string.Empty;
			try
			{
				//default the destination to the current page
				link = HttpUtility.UrlEncode(HttpContext.Request.Host.Value);
			}
			catch (Exception ex)
			{
				errorMessage = "<p>" + ex.Message.ToString() + "</p>";
			}

			try
			{
				//name the button using Web part setting
				planButtonText = !string.IsNullOrEmpty(properties.ButtonText) ? properties.ButtonText : "Start Planning";
			}
			catch (Exception ex)
			{
				errorMessage = "<p>" + ex.Message.ToString() + "</p>";
			}

			string savedItemsToSync = string.Empty;
			//if user is logged in, send them via the Vendor SSO page
			if (GetLoggedInPerson() != "")
			{
				if (!string.IsNullOrEmpty(DestinationURL) && !string.IsNullOrEmpty(properties.ButtonText))
				{
					UriBuilder url = new(DestinationURL);


					link = "/Convenience.org/ApplicationPages/vendorssologin.aspx?ReturnURL=" + WebUtility.UrlEncode(url.ToString());
				}

				//TODO: Need to generate actual info object for SessionItemInfo
				List<NACS.MyNACSSavedItems.SessionItemInfo> savedSessions = GetSavedItemsForUser("Session");

				if (savedSessions.Count > 0)
				{
					//upload to MYS
				}

				savedItemsToSync = "<p><strong>Sessions saved on this site:</strong></p>";

				foreach (var session in savedSessions)
				{
					//var workshopDocumentQuery = new ContentItemQueryBuilder()
					//			.ForContentType(
					//				NSWorkshop.CONTENT_TYPE_NAME,
					//				config => config
					//				.ForWebsite("NACSShow")
					//				.Where(w => w.WhereEquals("NodeGUID", session.KenticoNodeGUID))
					//				.WithLinkedItems(3)
					//				).Published()
					//				.InLanguage("en")
					//				.FirstOrDefault();

					//var workshop = await executor.GetMappedWebPageResult<NSWorkshop>(workshopDocumentQuery);

					//string id = workshop.GetStringValue("SharePointPageID", "");
					//if (string.IsNullOrEmpty(id))
					//	id = workshop.DocumentID.ToString();

					//string sessionID = workshop.GetStringValue("SessionID", "");
					//if (string.IsNullOrEmpty(sessionID))
					//	sessionID = workshop.NodeAlias;

					//savedItemsToSync += id + ":" + sessionID + ":" + session.KenticoNodeAliasPath + "<br/>";
				}

				savedItemsToSync += "<p><strong>ALL MYS Sessions (via API):</strong></p>";

				foreach (var myssession in GetAllMYSSessions())
				{
					savedItemsToSync += myssession.scheduleid + ":" + myssession.sessionidshow + ":" + myssession.sessionid + ":" + myssession.title + "<br/>";
				}

				savedItemsToSync += "<p><strong>Sessions in MYS planner (via API):</strong></p>";

				savedItemsToSync += "<p><strong>My Planner user account (via API):</strong></p>";

				foreach (var mysuser in GetMYSPlannerUser())
				{
					savedItemsToSync += mysuser.altregid + ":" + mysuser.email + ":" + mysuser.fullname + "<br/>";
				}


			}
			//else if user is not logged in
			else
			{
				//BYPASS LOGIN WITH INDIVIDUALKEY
				//Just create a token, as long as IndividuaKey is provided in QS
				NACSAPICustomerSoapClient service = new NACSAPICustomerSoapClient();
				NACS.Helper.CustomerService.NACSIndividual individual = new();

				string IndividualKey = "";

				try
				{
					IndividualKey = NACSUtilities.GetQueryStringValue("nacskey");
				}
				catch (Exception ex)
				{
					errorMessage = "<p>" + ex.Message.ToString() + "</p>";
				}

				//get user from query string key
				if (!string.IsNullOrEmpty(IndividualKey))
				{
					try
					{
						individual = service.Individual_GetById("", IndividualKey, this.NACSAPIKey);

						//Get SSO Application Id
						string connStringWeb = ConfigurationManager.ConnectionStrings["VendorAPI"].ToString();
						SqlConnection conn = new SqlConnection(connStringWeb);

						string strCommand = "Select_ApplicationByAPIKey @APIKey='3fad8a9b-1821-44eb-b0c8-03cee5acfacd'"; //2019 MYS Direct Login "App"
						SqlCommand cmd = new SqlCommand(strCommand, conn);
						conn.Open();
						SqlDataReader rdr = cmd.ExecuteReader();

						string appid = "";

						try
						{
							while (rdr.Read())
							{
								appid = rdr["ApplicationID"].ToString();
							}
						}
						finally
						{
							rdr.Close();
						}

						conn.Close();

						//Insert Token

						strCommand = "Insert_UserToken @ApplicationID='" + appid + "', @Username='" + individual.IndividualId + "'";
						cmd = new SqlCommand(strCommand, conn);
						conn.Open();
						string UserToken = Convert.ToString(cmd.ExecuteScalar());

						//Clean up
						conn.Close();
						conn.Dispose();
						cmd.Dispose();


						UriBuilder url = new UriBuilder(DestinationURL);
						if (url.Query != null && url.Query.Length > 1)
						{
							url.Query = url.Query.Substring(1) + "&token=" + UserToken;
						}
						else
						{
							url.Query = "token=" + UserToken;
						}

						link = url.ToString();

					}
					catch (Exception ex)
					{
						errorMessage = "<p>" + ex.Message.ToString() + "</p>";
					}



				}
				//force login
				else
				{

					link = "/Convenience.org/ApplicationPages/Login.aspx?Source=https%3a%2f%2f" + domain + "%2fPrepare%2fPlannerLogin";
				}

			}

			var planButtonLink = link;

			var model = new MYSPlannerButtonsWidgetViewModel(widgetProperties.Properties)
			{
                ErrorMessage = errorMessage,
				PlanButtonLink = planButtonLink,
				PlanButtonText = planButtonText,
				SavedItemsToSync = savedItemsToSync,
            };

			return View("~/Components/Widgets/MYSPlannerButtonsWidget/MYSPlannerButtonsWidget.cshtml", model);
		}

		private string GetLoggedInPerson()
		{
			string nacsid = "";

            //TODO: Is this correct value?
			var customerID = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
			try
			{
				if (httpContextAccessor?.HttpContext?.User?.Identity != null)
				{
					if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
					{
						UserInfo currentUserInfo = MembershipContext.AuthenticatedUser;
						nacsid = customerID ?? string.Empty;
						currentUserEmail = currentUserInfo.Email;
					}
				}
			}
			catch (InvalidOperationException)
			{
				return "";
			}

			return nacsid;
		}

		private List<NACS.MyNACSSavedItems.SessionItemInfo> GetSavedItemsForUser(string itemType)
		{
			List<NACS.MyNACSSavedItems.SessionItemInfo> items = new List<NACS.MyNACSSavedItems.SessionItemInfo>();

			var ui = new UserInfoProvider();
			var user = ui.Get(httpContextAccessor?.HttpContext?.User?.Identity?.Name);

            //TODO: Fix query
            items = NACS.MyNACSSavedItems.SessionItemInfoProvider.GetSessionItems()
				.WhereEquals("KenticoUserID", user.UserID)
				.And()
				.WhereEquals("SavedType", itemType)
				.OrderByDescending("SavedDate")
				.ToList();


			return items;
		}

		private List<MYSSession> GetAllMYSSessions()
		{
			bool PullFrom_Production = false;
			PullFrom_Production = (APIEnvironment == "Staging") ? false : true;

			MYSServiceProxyREST.MYSServiceProxyREST service = new MYSServiceProxyREST.MYSServiceProxyREST(PullFrom_Production);

			//get new api key
			string apikey = service.ValidateUser(MYSAPI_Username, MYSAPI_Passcode, MYSAPI_Showcode);

			//Get deserialized exhibitor
			List<MYSSession> sessions = service.GetAllSessions(apikey);
			return sessions;
		}

		private List<MYSPlannerUser> GetMYSPlannerUser()
		{
			bool PullFrom_Production = false;
			PullFrom_Production = (APIEnvironment == "Staging") ? false : true;

			MYSServiceProxyREST.MYSServiceProxyREST service = new MYSServiceProxyREST.MYSServiceProxyREST(PullFrom_Production);

			//get new api key
			string apikey = service.ValidateUser(MYSAPI_Username, MYSAPI_Passcode, MYSAPI_Showcode);

			//Get deserialized exhibitor
			List<MYSPlannerUser> user = service.GetPlannerUser(apikey, currentUserEmail);

			return user;
		}
	}

    public class MYSPlannerButtonsWidgetProperties : IWidgetProperties
	{
        [TextInputComponent(Label = "Button Text")]
		public string ButtonText { get; set; } = "Start Planning";

        [TextInputComponent(Label = "Button Link")]
		public string ButtonLink { get; set; } = "";

        [TextInputComponent(Label = "Destination URL (Staging)")]
		public string DestinationURL_Staging { get; set; } = "https://nacs21.mysstaging.com/8_0/login/login.cfm";

        [TextInputComponent(Label = "Destination URL (Production)")]
		public string DestinationURL_Production { get; set; } = "https://nacs21.mapyourshow.com/8_0/login/login.cfm";


		//public string ErrorMessage { get; set; } = "";
		//public string PlanButtonText { get; set; } = "Start Planning";
		//public string PlanButtonLink { get; set; } = "";
		//public string SavedItemsToSync { get; set; } = "";
	}
}
