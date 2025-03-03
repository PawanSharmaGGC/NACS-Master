using Convenience.org.Components.Widgets.MailingListUnsubscribe;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

[assembly: RegisterWidget(identifier: MailingListUnsubscribeViewComponent.IDENTIFIER, name: "MailingListUnsubscribe",
    viewComponentType: typeof(MailingListUnsubscribeViewComponent),
    propertiesType: typeof(MailingListUnsubscribeProperties), Description = "MailingListUnsubscribe",
    IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.MailingListUnsubscribe
{
    public class MailingListUnsubscribeViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "MailingListUnsubscribe";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MailingListUnsubscribeViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<MailingListUnsubscribeProperties> properties)
        {
            string qs_nacskey = _httpContextAccessor.HttpContext.Request.Query["nacskey"];
            string qs_mlkey = _httpContextAccessor.HttpContext.Request.Query["mlkey"];
            var vm = new MailingListUnsubscribeViewModel();
            //If info in query string, run unsubscribe Web service
            if (!string.IsNullOrEmpty(qs_nacskey) && !string.IsNullOrEmpty(qs_mlkey))
            {
                vm.ShowErrorPanel = false;
                vm.ShowPromptPanel = true;
                vm.ShowUnsubscribedPanel = false;

                vm.NACSKey = qs_nacskey;
                vm.MailingListKey = qs_mlkey;

                //get display name from web part properties, or default to key.
                //format: mlkey:ListName, mlkey:ListName

                string displayname = qs_mlkey.ToUpper() + " (name unavailable)";

                try
                {
                    string[] displaypairs = properties.Properties.MailingListKeysAndNames.Split(',');

                    foreach (string displaypair in displaypairs)
                    {
                        string[] pair = displaypair.Split(':');

                        if (pair[0].ToLower().Trim() == qs_mlkey.ToLower().Trim())
                        {
                            displayname = pair[1];
                        }
                    }
                }
                catch { }

                vm.MLDisplayName1 = displayname;
                vm.MLDisplayName2 = displayname;
                vm.ListName = displayname;
            }
            else
            {
                vm.ShowErrorPanel = true;
                vm.ShowPromptPanel = false;
                vm.ShowUnsubscribedPanel = false;
                vm.ErrorMessage = "We need more information to unsubscribe you from this mailing list.<br/><br>Please <a href='/About-NACS/Contact-Us'>contact us</a> for help and to unsubscribe.";
            }

            return View("~/Components/Widgets/MailingListUnsubscribe/_MailingListUnsubscribe.cshtml", vm);
        }
    }
}