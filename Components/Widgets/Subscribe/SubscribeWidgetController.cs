using System.Configuration;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.Widgets.Subscribe
{
    public class SubscribeWidgetController : Controller
    {

        private readonly IBizFormInfoProvider bizFormInfoProvider;
        public SubscribeWidgetController(IBizFormInfoProvider bizFormInfoProvider) {
            this.bizFormInfoProvider = bizFormInfoProvider;    
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(SubscribeViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var formObject = bizFormInfoProvider.Get("Subscribe");

            if (formObject == null) {
                return StatusCode(500, "Form configuration not found");
            }

            var formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
            if (formClass == null)
            {
                return StatusCode(500, "Form Class configuration not found");
            }

            var newFormItem = BizFormItem.New(formClass.ClassName);
            newFormItem.SetValue("Email",ValidationHelper.GetString(model.Email,""));
            newFormItem.Insert();

            var emailBody = $@"<p> Email : {model.Email}</p>";

            var emailMessage = new EmailMessage
            {
                //From = configuration.Getvalue<string>( "" ),
                //Recipients = configuration.Getvalue<string>( "" ),
                //Subject = configuration.Getvalue<string>( "" ),
                //Body = configuration.Getvalue<string>( "" ),
            };


            return Json( new {success = true});
        }
    }
}
