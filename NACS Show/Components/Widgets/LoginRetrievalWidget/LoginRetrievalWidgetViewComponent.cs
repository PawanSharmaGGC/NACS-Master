using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using NACSShow.Components.Widgets.LoginRetrievalWidget;

[assembly: RegisterWidget(
    identifier: LoginRetrievalWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(LoginRetrievalWidgetViewComponent),
    name: "LoginRetrievalWidget",
    Description = "Login Retrieval",
    IconClass = "icon-l-ribbon",
    AllowCache = true)]

namespace NACSShow.Components.Widgets.LoginRetrievalWidget
{
    [ViewComponent]
    public class LoginRetrievalWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "NACSShow.LoginRetrievalWidget";

        private static readonly string encryptionKey = "6789RESET@@PASSWORD12345"; //needs to be 16 or 24 characters long, and match Change Password Control
        private string origin = "";

        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginRetrievalWidgetViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke(ComponentViewModel vm)
        {
            //Get Query String name value pairs
            HttpContext? context = httpContextAccessor.HttpContext;

            var collection = new QueryString();
            if (context != null)
            {
                collection = context.Request.QueryString;
            }

            if (!string.IsNullOrEmpty(collection.Value))
            {
                var queryParams = QueryHelpers.ParseQuery(collection.Value);
                origin = queryParams.ContainsKey("src") ? queryParams["src"].ToString() : "";
            }

            var model = new LoginRetrievalWidgetViewModel();


            return View("~/Components/Widgets/LoginRetrievalWidget/Index.cshtml", model);
        }

    }   
}
